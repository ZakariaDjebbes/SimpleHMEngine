using System.Globalization;
using SFML.Audio;
using SFML.Graphics;

namespace Core.Resources;

/// <summary>
/// Manages the loading and caching of game resources such as textures, fonts, sound buffers, and color palettes.
/// </summary>
/// <typeparam name="T">The type of resource to manage. Must be a class.</typeparam>
public static class ResourceManager<T> where T : class
{
    private static readonly Dictionary<string, T> Resources;
    
    static ResourceManager() => Resources = [];

    /// <summary>
    /// Loads a resource from the specified file path.
    /// </summary>
    /// <param name="filePath">The full or relative path to the resource file.</param>
    /// <returns>The loaded resource, or null if loading failed.</returns>
    public static T GetResource(string filePath)
    {
        if (Resources.TryGetValue(filePath, out var value))
            return value;

        var resource = LoadResource(filePath);

        if (resource != null) Resources[filePath] = resource;

        return resource;
    }
    
    private static Sprite GetSprite(string texturePath)
    {
        var texture = ResourceManager<Texture>.GetResource(texturePath);
        if (Resources.TryGetValue(texturePath, out var sprite))
            return sprite as Sprite;
        
        var newSprite = new Sprite(texture);
        Resources[texturePath] = newSprite as T;

        return newSprite;
    }

    /// <summary>
    /// Loads all resources found in the specified directory.
    /// </summary>
    /// <param name="folderName">The directory containing the resources to load.</param>
    /// <returns>A collection of loaded resources.</returns>
    public static IEnumerable<T> GetResources(string folderName)
    {
        var resources = new List<T>();
        var files = Directory.GetFiles(folderName);

        foreach (var file in files)
        {
            if (Resources.TryGetValue(file, out var resource))
                resources.Add(resource);
            else
            {
                resource = LoadResource(file);

                if (resource == null) continue;
                
                Resources[file] = resource;
                resources.Add(resource);
            }
        }

        return resources;
    }
    
    /// <summary>
    /// Loads a resource from the specified file.
    /// </summary>
    /// <param name="filename">The path to the resource file.</param>
    /// <returns>The loaded resource, or null if loading failed.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the resource type is not supported.</exception>
    private static T LoadResource(string filename)
    {
        if (typeof(T) == typeof(Texture))
            return new Texture(filename) as T;

        if (typeof(T) == typeof(Font))
            return new Font(filename) as T;

        if (typeof(T) == typeof(SoundBuffer))
            return new SoundBuffer(filename) as T;

        if (typeof(T) == typeof(Dictionary<string, Color>))
            return LoadColors(filename) as T;
        
        if (typeof(T) == typeof(Sprite))
            return GetSprite(filename) as T;
        
        throw new InvalidOperationException("Unsupported resource type");
    }

    /// <summary>
    /// Loads a color palette from a text file.
    /// </summary>
    /// <param name="filename">The path to the color palette file.</param>
    /// <returns>A dictionary mapping color names to their values.</returns>
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

    /// <summary>
    /// Converts a hexadecimal color string to a Color object.
    /// </summary>
    /// <param name="hex">The hexadecimal color string (with or without the '#' prefix).</param>
    /// <returns>The corresponding Color object, or null if the conversion failed.</returns>
    private static Color? ColorFromHex(string hex)
    {
        if (hex.StartsWith('#'))
            hex = hex[1..];

        if (hex.Length == 6 &&
            int.TryParse(hex.AsSpan(0,2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var r) &&
            int.TryParse(hex.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var g) &&
            int.TryParse(hex.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var b))
        {
            return new Color((byte)r, (byte)g, (byte)b);
        }

        return null;
    }
    
    /// <summary>
    /// Clears all cached resources.
    /// </summary>
    public static void Clear() => Resources.Clear();
}