using SFML.Graphics;

namespace Core.Drawing.UserInterface;

/// <summary>
/// Shared visual defaults for the UI widgets. Mutable so a game can re-skin the whole UI in one place.
/// </summary>
public static class UiTheme
{
    public static Color Surface { get; set; } = new(44, 46, 54);
    public static Color SurfaceVariant { get; set; } = new(64, 68, 82);
    public static Color Hover { get; set; } = new(92, 98, 116);
    public static Color Pressed { get; set; } = new(30, 32, 38);
    public static Color Outline { get; set; } = new(120, 128, 148);
    public static Color OnSurface { get; set; } = Color.White;
    public static Color Accent { get; set; } = new(179, 202, 201);
    public static Color AccentDark { get; set; } = new(85, 105, 103);

    public static uint FontSize { get; set; } = 20;

    /// <summary>
    /// Optional path to a font file. When <c>null</c>, the engine's embedded default font is used.
    /// Set this to re-skin all UI text with a custom font.
    /// </summary>
    public static string FontPath { get; set; }
}
