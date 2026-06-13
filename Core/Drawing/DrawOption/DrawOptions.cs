using SFML.Graphics;

namespace Core.Drawing.DrawOption;

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

    /// <summary>
    /// Creates options with only a fill color set, letting a <see cref="Color"/> be passed directly to a
    /// drawing call — for example <c>Draw.Circle(center, radius, Color.Red)</c>.
    /// </summary>
    /// <param name="color">The fill color.</param>
    public static implicit operator DrawOptions(Color color) => new() { FillColor = color };

    /// <summary>Creates options with the given fill color, as the start of a fluent chain.</summary>
    /// <param name="color">The fill color.</param>
    /// <returns>A new <see cref="DrawOptions"/> instance.</returns>
    public static DrawOptions Fill(Color color) => new() { FillColor = color };

    /// <summary>Creates options with the given outline, as the start of a fluent chain.</summary>
    /// <param name="color">The outline color.</param>
    /// <param name="thickness">The outline thickness, in pixels.</param>
    /// <returns>A new <see cref="DrawOptions"/> instance.</returns>
    public static DrawOptions Outline(Color color, float thickness)
        => new() { OutlineColor = color, OutlineThickness = thickness };

    /// <summary>Creates options with the given rotation, as the start of a fluent chain.</summary>
    /// <param name="degrees">The rotation angle, in degrees.</param>
    /// <returns>A new <see cref="DrawOptions"/> instance.</returns>
    public static DrawOptions Rotated(float degrees) => new() { Rotation = degrees };

    /// <summary>Sets the fill color and returns this instance for chaining.</summary>
    /// <param name="color">The fill color.</param>
    /// <returns>This instance.</returns>
    public DrawOptions WithFill(Color color)
    {
        FillColor = color;
        return this;
    }

    /// <summary>Sets the outline color and thickness and returns this instance for chaining.</summary>
    /// <param name="color">The outline color.</param>
    /// <param name="thickness">The outline thickness, in pixels.</param>
    /// <returns>This instance.</returns>
    public DrawOptions WithOutline(Color color, float thickness)
    {
        OutlineColor = color;
        OutlineThickness = thickness;
        return this;
    }

    /// <summary>Sets the rotation and returns this instance for chaining.</summary>
    /// <param name="degrees">The rotation angle, in degrees.</param>
    /// <returns>This instance.</returns>
    public DrawOptions WithRotation(float degrees)
    {
        Rotation = degrees;
        return this;
    }

    /// <summary>Sets the opacity and returns this instance for chaining.</summary>
    /// <param name="opacity">The opacity, from 0.0 (transparent) to 1.0 (opaque).</param>
    /// <returns>This instance.</returns>
    public DrawOptions WithOpacity(float opacity)
    {
        Opacity = opacity;
        return this;
    }
}