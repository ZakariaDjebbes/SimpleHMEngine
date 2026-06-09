using SFML.Graphics;

namespace Core.Drawing.UserInterface;

/// <summary>
/// Shared visual defaults for the UI widgets. Mutable so a game can re-skin the whole UI in one place.
/// </summary>
public static class UiTheme
{
    /// <summary>Base background color for surfaces such as cards and buttons.</summary>
    public static Color Surface { get; set; } = new(44, 46, 54);

    /// <summary>A lighter surface shade, used for gradients and raised elements.</summary>
    public static Color SurfaceVariant { get; set; } = new(64, 68, 82);

    /// <summary>Surface color shown while an element is hovered.</summary>
    public static Color Hover { get; set; } = new(92, 98, 116);

    /// <summary>Surface color shown while an element is pressed.</summary>
    public static Color Pressed { get; set; } = new(30, 32, 38);

    /// <summary>Color used for element outlines.</summary>
    public static Color Outline { get; set; } = new(120, 128, 148);

    /// <summary>Foreground color for text and icons drawn on a surface.</summary>
    public static Color OnSurface { get; set; } = Color.White;

    /// <summary>Primary accent color used to highlight elements.</summary>
    public static Color Accent { get; set; } = new(179, 202, 201);

    /// <summary>A darker variant of the accent color.</summary>
    public static Color AccentDark { get; set; } = new(85, 105, 103);

    /// <summary>Default character size, in pixels, for UI text.</summary>
    public static uint FontSize { get; set; } = 20;

    /// <summary>
    /// Optional path to a font file. When <c>null</c>, the engine's embedded default font is used.
    /// Set this to re-skin all UI text with a custom font.
    /// </summary>
    public static string FontPath { get; set; }
}
