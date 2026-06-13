using System.Runtime.InteropServices;
using SFML.Graphics;

namespace Core.Resources;

/// <summary>
/// Provides access to assets embedded directly in the engine assembly.
/// Embedding means consumers of the NuGet package get a working default font
/// without having to ship their own <c>Resources/default.ttf</c>.
/// </summary>
public static class EmbeddedResources
{
    // Logical name is pinned in Core.csproj so it stays stable regardless of root namespace.
    private const string DefaultFontResource = "Core.Resources.DefaultResources.default.ttf";
    private const string DefaultTextureResource = "Core.Resources.DefaultResources.default.png";

    private static Font _defaultFont;
    private static Texture _defaultTexture;

    // The font bytes are kept referenced AND pinned for the whole app lifetime. SFML's
    // sfFont_createFromMemory keeps a raw pointer into this buffer without copying it and reads
    // glyphs lazily, so the data must never move (GC compaction) or be collected — otherwise SFML
    // reads freed/moved memory and crashes (0xC0000005). Pinning prevents both.
    // (The Font(Stream) overload is NOT a safe alternative: its StreamAdaptor callback delegates
    // get garbage-collected and SFML then invokes a dead delegate.)
    private static byte[] _fontBytes;

    /// <summary>
    /// The engine's built-in font, loaded once from the embedded assembly resource.
    /// Used whenever no explicit font path is supplied.
    /// </summary>
    public static Font DefaultFont => _defaultFont ??= LoadDefaultFont();
    
    /// <summary>
    /// The engine's default texture for sprites with no defined texture, loaded once from the embedded assembly resource.
    /// </summary>
    public static Texture DefaultTexture => _defaultTexture ??= LoadDefaultTexture();

    private static Font LoadDefaultFont()
    {
        var assembly = typeof(EmbeddedResources).Assembly;
        using var resource = assembly.GetManifestResourceStream(DefaultFontResource)
            ?? throw new InvalidOperationException(
                $"Embedded font resource '{DefaultFontResource}' was not found in assembly '{assembly.GetName().Name}'.");

        using var memory = new MemoryStream();
        resource.CopyTo(memory);

        _fontBytes = memory.ToArray();
        GCHandle.Alloc(_fontBytes, GCHandleType.Pinned);

        return new Font(_fontBytes);
    }

    // Unlike Font, SFML's Texture decodes the image and uploads it to the GPU at construction time,
    // keeping no pointer into the source bytes — so there is nothing to pin and the buffer can be
    // collected as soon as the constructor returns.
    private static Texture LoadDefaultTexture()
    {
        var assembly = typeof(EmbeddedResources).Assembly;
        using var resource = assembly.GetManifestResourceStream(DefaultTextureResource)
            ?? throw new InvalidOperationException(
                $"Embedded sprite resource '{DefaultTextureResource}' was not found in assembly '{assembly.GetName().Name}'.");

        using var memory = new MemoryStream();
        resource.CopyTo(memory);

        return new Texture(memory.ToArray());
    }
}
