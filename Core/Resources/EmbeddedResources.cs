using SFML.Graphics;

namespace Core.Resources;

/// <summary>
/// Provides access to assets embedded directly in the engine assembly.
/// Embedding means consumers of the NuGet package get a working default font and texture
/// without having to ship their own. Loading and caching go through
/// <see cref="ResourceManager{T}.GetEmbeddedResource"/>; these defaults also act as the fallbacks
/// the resource manager returns when a font or texture fails to load.
/// </summary>
public static class EmbeddedResources
{
    // Logical names are pinned in Core.csproj so they stay stable regardless of root namespace.
    private const string DefaultFontResource = "Core.Resources.DefaultResources.default.ttf";
    private const string DefaultTextureResource = "Core.Resources.DefaultResources.default.png";

    private static readonly System.Reflection.Assembly EngineAssembly = typeof(EmbeddedResources).Assembly;

    /// <summary>
    /// The engine's built-in font, loaded once from the embedded assembly resource.
    /// Used whenever no explicit font path is supplied.
    /// </summary>
    public static Font DefaultFont => ResourceManager<Font>.GetEmbeddedResource(DefaultFontResource, EngineAssembly);

    /// <summary>
    /// The engine's default texture for sprites with no defined texture, loaded once from the embedded assembly resource.
    /// </summary>
    public static Texture DefaultTexture => ResourceManager<Texture>.GetEmbeddedResource(DefaultTextureResource, EngineAssembly);
}
