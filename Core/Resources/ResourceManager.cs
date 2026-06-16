using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using SFML.Audio;
using SFML.Graphics;

namespace Core.Resources;

/// <summary>
/// Manages the loading and caching of game resources, keyed by file path (or cache key for stream and
/// embedded sources). What counts as a "resource" is open: built-in loaders cover textures, fonts, sound
/// buffers, sprites, colour palettes, raw text (<see cref="string"/>) and raw bytes (<c>byte[]</c>), and
/// callers can register their own through <see cref="ResourceManager.RegisterLoader{T}"/>.
/// </summary>
/// <typeparam name="T">The type of resource to manage. Must be a reference type.</typeparam>
public static class ResourceManager<T> where T : class
{
    private static readonly Dictionary<string, T> Resources = [];

    // Embedded resources are engine-owned, long-lived and shared (the default font/texture also act as
    // fallbacks and are held by Draw), so their cache keys are prefixed and exempted from Clear's disposal.
    private const string EmbeddedKeyPrefix = "embedded::";

    /// <summary>
    /// Loads a resource from a file path, caching it under its full path. On failure, returns the
    /// registered fallback for <typeparamref name="T"/> if one exists; otherwise throws
    /// <see cref="ResourceLoadException"/>.
    /// </summary>
    /// <param name="filePath">The full or relative path to the resource file.</param>
    /// <returns>The loaded (or fallback) resource.</returns>
    /// <exception cref="InvalidOperationException">No loader is registered for <typeparamref name="T"/>.</exception>
    /// <exception cref="ResourceLoadException">Loading failed and no fallback is registered.</exception>
    public static T GetResource(string filePath)
    {
        var key = Normalize(filePath);
        if (Resources.TryGetValue(key, out var cached))
            return cached;

        var loader = PathLoaderOrThrow();
        try
        {
            var resource = (T)loader(filePath);
            Resources[key] = resource;
            return resource;
        }
        catch (Exception exception)
        {
            if (ResourceManager.TryGetFallback(typeof(T), out var fallback))
            {
                Debug.WriteLine($"ResourceManager: failed to load {typeof(T).Name} from '{filePath}'; returning fallback. {exception.Message}");
                return (T)fallback(); // deliberately not cached, so a later attempt can still succeed
            }

            throw new ResourceLoadException(typeof(T), filePath, exception);
        }
    }

    /// <summary>
    /// Attempts to load a resource from a file path without throwing on load failure (a missing or
    /// invalid file yields <c>false</c>).
    /// </summary>
    /// <param name="filePath">The full or relative path to the resource file.</param>
    /// <param name="resource">The loaded resource, or <c>null</c> if loading failed.</param>
    /// <returns><c>true</c> if the resource was loaded or already cached; otherwise <c>false</c>.</returns>
    /// <exception cref="InvalidOperationException">No loader is registered for <typeparamref name="T"/>.</exception>
    public static bool TryGetResource(string filePath, out T resource)
    {
        var key = Normalize(filePath);
        if (Resources.TryGetValue(key, out resource))
            return true;

        var loader = PathLoaderOrThrow();
        try
        {
            resource = (T)loader(filePath);
            Resources[key] = resource;
            return true;
        }
        catch
        {
            resource = null;
            return false;
        }
    }

    /// <summary>
    /// Loads a resource from an open stream, caching it under <paramref name="cacheKey"/>.
    /// </summary>
    /// <param name="cacheKey">A unique key to cache and later retrieve the resource by.</param>
    /// <param name="stream">The stream to read the resource from.</param>
    /// <returns>The loaded resource.</returns>
    /// <exception cref="InvalidOperationException">No stream loader is registered for <typeparamref name="T"/>.</exception>
    /// <exception cref="ResourceLoadException">Loading failed.</exception>
    public static T GetResource(string cacheKey, Stream stream)
    {
        if (Resources.TryGetValue(cacheKey, out var cached))
            return cached;

        var loader = StreamLoaderOrThrow();
        try
        {
            var resource = (T)loader(stream);
            Resources[cacheKey] = resource;
            return resource;
        }
        catch (Exception exception)
        {
            throw new ResourceLoadException(typeof(T), cacheKey, exception);
        }
    }

    /// <summary>
    /// Loads a resource embedded in an assembly's manifest, caching it under the manifest name. Defaults
    /// to the calling assembly when none is given.
    /// </summary>
    /// <param name="manifestResourceName">The logical name of the embedded resource.</param>
    /// <param name="assembly">The assembly holding the resource. Defaults to the calling assembly.</param>
    /// <returns>The loaded resource.</returns>
    /// <exception cref="InvalidOperationException">No stream loader is registered for <typeparamref name="T"/>.</exception>
    /// <exception cref="ResourceLoadException">The resource was not found or failed to load.</exception>
    public static T GetEmbeddedResource(string manifestResourceName, Assembly assembly = null)
    {
        var key = $"{EmbeddedKeyPrefix}{manifestResourceName}";
        if (Resources.TryGetValue(key, out var cached))
            return cached;

        assembly ??= Assembly.GetCallingAssembly();
        var loader = StreamLoaderOrThrow();
        try
        {
            using var stream = assembly.GetManifestResourceStream(manifestResourceName)
                ?? throw new InvalidOperationException(
                    $"Embedded resource '{manifestResourceName}' was not found in assembly '{assembly.GetName().Name}'.");

            var resource = (T)loader(stream);
            Resources[key] = resource;
            return resource;
        }
        catch (Exception exception)
        {
            throw new ResourceLoadException(typeof(T), manifestResourceName, exception);
        }
    }

    /// <summary>
    /// Loads all resources found in the specified directory, skipping any file that fails to load.
    /// </summary>
    /// <param name="folderName">The directory containing the resources to load.</param>
    /// <returns>The resources that loaded successfully.</returns>
    public static IEnumerable<T> GetResources(string folderName)
    {
        var resources = new List<T>();
        foreach (var file in Directory.GetFiles(folderName))
            if (TryGetResource(file, out var resource))
                resources.Add(resource);

        return resources;
    }

    /// <summary>
    /// Removes a single file-loaded resource from the cache and disposes it if it is
    /// <see cref="IDisposable"/>.
    /// </summary>
    /// <param name="filePath">The path the resource was loaded from.</param>
    /// <returns><c>true</c> if a cached resource was found and removed; otherwise <c>false</c>.</returns>
    public static bool Unload(string filePath)
    {
        if (!Resources.Remove(Normalize(filePath), out var resource))
            return false;

        (resource as IDisposable)?.Dispose();
        return true;
    }

    /// <summary>
    /// Clears all cached resources, disposing any that are <see cref="IDisposable"/> (textures, fonts,
    /// sound buffers and the like wrap native handles that must be released). Engine-owned embedded
    /// resources are kept: they are shared and long-lived, so disposing them would break callers still
    /// holding them (including the fallback defaults).
    /// </summary>
    public static void Clear()
    {
        foreach (var key in Resources.Keys.ToList())
        {
            if (key.StartsWith(EmbeddedKeyPrefix, StringComparison.Ordinal))
                continue;

            (Resources[key] as IDisposable)?.Dispose();
            Resources.Remove(key);
        }
    }

    private static Func<string, object> PathLoaderOrThrow()
        => ResourceManager.TryGetPathLoader(typeof(T), out var loader)
            ? loader
            : throw new InvalidOperationException($"No path loader registered for resource type {typeof(T).Name}.");

    private static Func<Stream, object> StreamLoaderOrThrow()
        => ResourceManager.TryGetStreamLoader(typeof(T), out var loader)
            ? loader
            : throw new InvalidOperationException($"No stream loader registered for resource type {typeof(T).Name}.");

    private static string Normalize(string filePath) => Path.GetFullPath(filePath);
}

/// <summary>
/// The shared registry that decides how each resource type is loaded. Built-in loaders are seeded on
/// first use; register your own with <see cref="RegisterLoader{T}"/> (from a path),
/// <see cref="RegisterStreamLoader{T}"/> (from a stream) and <see cref="RegisterFallback{T}"/> (the value
/// returned when a load fails). This is what makes "what is a resource" open to extension.
/// </summary>
public static class ResourceManager
{
    private static readonly Dictionary<Type, Func<string, object>> PathLoaders = [];
    private static readonly Dictionary<Type, Func<Stream, object>> StreamLoaders = [];
    private static readonly Dictionary<Type, Func<object>> Fallbacks = [];

    // Font bytes loaded from memory must stay alive and unmoved for the font's whole lifetime (see
    // LoadFontFromStream). Pinned handles are retained here and never freed — fonts are cached for the
    // app lifetime, so the pin is intentional, not a leak to fix.
    private static readonly List<GCHandle> PinnedBuffers = [];

    static ResourceManager() => RegisterBuiltIns();

    /// <summary>Registers how to load a resource of type <typeparamref name="T"/> from a file path. Overwrites any existing loader.</summary>
    /// <typeparam name="T">The resource type the loader produces.</typeparam>
    /// <param name="loader">A function that turns a file path into a resource.</param>
    public static void RegisterLoader<T>(Func<string, T> loader) where T : class
        => PathLoaders[typeof(T)] = path => loader(path);

    /// <summary>Registers how to load a resource of type <typeparamref name="T"/> from a stream. Overwrites any existing loader.</summary>
    /// <typeparam name="T">The resource type the loader produces.</typeparam>
    /// <param name="loader">A function that turns a stream into a resource.</param>
    public static void RegisterStreamLoader<T>(Func<Stream, T> loader) where T : class
        => StreamLoaders[typeof(T)] = stream => loader(stream);

    /// <summary>
    /// Registers a fallback returned by <see cref="ResourceManager{T}.GetResource(string)"/> when a load
    /// fails, instead of throwing. The factory is invoked lazily, only when a fallback is actually needed.
    /// </summary>
    /// <typeparam name="T">The resource type the fallback applies to.</typeparam>
    /// <param name="fallback">A factory producing the fallback resource.</param>
    public static void RegisterFallback<T>(Func<T> fallback) where T : class
        => Fallbacks[typeof(T)] = () => fallback();

    internal static bool TryGetPathLoader(Type type, out Func<string, object> loader)
        => PathLoaders.TryGetValue(type, out loader);

    internal static bool TryGetStreamLoader(Type type, out Func<Stream, object> loader)
        => StreamLoaders.TryGetValue(type, out loader);

    internal static bool TryGetFallback(Type type, out Func<object> fallback)
        => Fallbacks.TryGetValue(type, out fallback);

    private static void RegisterBuiltIns()
    {
        RegisterLoader<Texture>(path => new Texture(path));
        RegisterLoader<Font>(path => new Font(path));
        RegisterLoader<SoundBuffer>(path => new SoundBuffer(path));
        RegisterLoader<Dictionary<string, Color>>(LoadColors);
        RegisterLoader<Sprite>(path => new Sprite(ResourceManager<Texture>.GetResource(path)));
        RegisterLoader<string>(File.ReadAllText);
        RegisterLoader<byte[]>(File.ReadAllBytes);

        RegisterStreamLoader<Texture>(stream => new Texture(ReadAllBytes(stream)));
        RegisterStreamLoader<Font>(LoadFontFromStream);
        RegisterStreamLoader<SoundBuffer>(stream => new SoundBuffer(ReadAllBytes(stream)));
        RegisterStreamLoader<string>(stream => Encoding.UTF8.GetString(ReadAllBytes(stream)));
        RegisterStreamLoader<byte[]>(ReadAllBytes);

        // Defaults shipped in the engine assembly double as fallbacks, so a missing texture/font degrades
        // to the built-in placeholder rather than crashing.
        RegisterFallback<Texture>(() => EmbeddedResources.DefaultTexture);
        RegisterFallback<Font>(() => EmbeddedResources.DefaultFont);
    }

    // SFML's sfFont_createFromMemory keeps a raw pointer into the byte buffer and reads glyphs lazily, so
    // the buffer must never move (GC compaction) or be collected while the font lives. We read the stream
    // into an array, pin it for good, and build the font from those bytes. The Font(Stream) overload is
    // NOT a safe alternative: its StreamAdaptor callback delegates get collected and SFML then calls a
    // dead delegate.
    private static Font LoadFontFromStream(Stream stream)
    {
        var bytes = ReadAllBytes(stream);
        PinnedBuffers.Add(GCHandle.Alloc(bytes, GCHandleType.Pinned));
        return new Font(bytes);
    }

    private static byte[] ReadAllBytes(Stream stream)
    {
        if (stream is MemoryStream memoryStream)
            return memoryStream.ToArray();

        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }

    private static Dictionary<string, Color> LoadColors(string filename)
    {
        var colors = new Dictionary<string, Color>();

        foreach (var line in File.ReadAllLines(filename))
        {
            var parts = line.Split('=');
            if (parts.Length != 2) continue;

            var name = parts[0].Trim();
            var hex = parts[1].Trim();

            if (ColorFromHex(hex) is { } color)
                colors[name] = color;
        }

        return colors;
    }

    private static Color? ColorFromHex(string hex)
    {
        if (hex.StartsWith('#'))
            hex = hex[1..];

        if (hex.Length == 6 &&
            int.TryParse(hex.AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var r) &&
            int.TryParse(hex.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var g) &&
            int.TryParse(hex.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var b))
        {
            return new Color((byte)r, (byte)g, (byte)b);
        }

        return null;
    }
}
