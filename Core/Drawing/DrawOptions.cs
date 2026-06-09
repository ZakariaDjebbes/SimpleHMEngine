using SFML.Graphics;

namespace Core.Drawing;

/// <summary>
/// Represents configurable options for drawing shapes and lines.
/// </summary>
public class DrawOptions
{
    /// <summary>
    /// Gets or sets the fill color of the shape.
    /// If set to <c>null</c>, the default color is used.
    /// </summary>
    public Color? FillColor { get; set; }

    /// <summary>
    /// Gets or sets the outline color of the shape.
    /// If set to <c>null</c>, no outline is drawn.
    /// </summary>
    public Color? OutlineColor { get; set; }

    /// <summary>
    /// Gets or sets the thickness of the outline.
    /// If set to <c>null</c>, the default thickness is used.
    /// </summary>
    public float? OutlineThickness { get; set; }

    /// <summary>
    /// Gets or sets the rotation angle of the shape, in degrees.
    /// If set to <c>null</c>, no rotation is applied.
    /// </summary>
    public float? Rotation { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the shape, ranging from 0.0 (fully transparent)
    /// to 1.0 (fully opaque). If set to <c>null</c>, full opacity is used.
    /// </summary>
    public float? Opacity { get; set; }
}

public class TextDrawOptions : DrawOptions
{
    /// <summary>
    /// Gets or sets the path to the font file used for rendering text.
    /// </summary>
    public string FontPath { get; set; }
    
    /// <summary>
    /// Gets or sets the default character size for rendering text, in pixels.
    /// </summary>
    public uint? CharacterSize { get; set; }
}