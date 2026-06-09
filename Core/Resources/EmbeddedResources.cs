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

    private static Font _defaultFont;

    // Kept alive for the whole app lifetime on purpose: SFML reads font glyphs lazily from this
    // stream, so it must never be disposed or garbage-collected while the font is in use.
    private static MemoryStream _defaultFontStream;

    /// <summary>
    /// The engine's built-in font, loaded once from the embedded assembly resource.
    /// Used whenever no explicit font path is supplied.
    /// </summary>
    public static Font DefaultFont => _defaultFont ??= LoadDefaultFont();

    private static Font LoadDefaultFont()
    {
        var assembly = typeof(EmbeddedResources).Assembly;
        using var resource = assembly.GetManifestResourceStream(DefaultFontResource)
            ?? throw new InvalidOperationException(
                $"Embedded font resource '{DefaultFontResource}' was not found in assembly '{assembly.GetName().Name}'.");

        // Copy into a seekable MemoryStream and keep it rooted in a static field. We must use the
        // Stream constructor (not Font(byte[])): SFML's createFromMemory holds a raw pointer to the
        // buffer without copying it, so the byte[] would be moved/collected by the GC and SFML would
        // later read freed memory (a random 0xC0000005 crash when glyphs are lazily loaded). The
        // Stream constructor instead reads through a retained managed adaptor, which is GC-safe.
        _defaultFontStream = new MemoryStream();
        resource.CopyTo(_defaultFontStream);
        _defaultFontStream.Position = 0;

        return new Font(_defaultFontStream);
    }
}
