using Core.Engine;
using Core.Resources;
using SFML.Graphics;
using SFML.System;

namespace Core.Drawing;

/// <summary>
/// Provides drawing utilities for various shapes, such as circles, rectangles, triangles, and lines, using SFML.
/// </summary>
public static class Draw
{
    private static RenderWindow _window = GameContext.CurrentWindow;

    private static readonly CircleShape CircleShape = new();
    private static readonly RectangleShape RectangleShape = new();
    private static readonly ConvexShape TriangleShape = new(3);
    private static readonly ConvexShape PolygonShape = new();
    private static readonly VertexArray LineVertices = new(PrimitiveType.Lines, 2);
    private static readonly Text TextDrawable = new() { Font = EmbeddedResources.DefaultFont };

    /// <summary>
    /// Draws a circle at the specified center with the given radius.
    /// </summary>
    /// <param name="center">The center position of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Circle(Vector2f center, float radius, DrawOptions options = null)
    {
        CircleShape.Radius = radius;
        CircleShape.Origin = new Vector2f(radius, radius);
        CircleShape.Position = center;
        ApplyDrawOptions(CircleShape, options);
        _window.Draw(CircleShape);
    }

    /// <summary>
    /// Draws a rectangle with the specified position, width, and height.
    /// </summary>
    /// <param name="x">The X-coordinate of the rectangle's position.</param>
    /// <param name="y">The Y-coordinate of the rectangle's position.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Rectangle(float x, float y, float width, float height, DrawOptions options = null)
    {
        RectangleShape.Size = new Vector2f(width, height);
        RectangleShape.Position = new Vector2f(x, y);
        ApplyDrawOptions(RectangleShape, options);
        _window.Draw(RectangleShape);
    }

    /// <summary>
    /// Draws a triangle defined by three points.
    /// </summary>
    /// <param name="point1">The first point of the triangle.</param>
    /// <param name="point2">The second point of the triangle.</param>
    /// <param name="point3">The third point of the triangle.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Triangle(Vector2f point1, Vector2f point2, Vector2f point3, DrawOptions options = null)
    {
        TriangleShape.SetPoint(0, point1);
        TriangleShape.SetPoint(1, point2);
        TriangleShape.SetPoint(2, point3);

        ApplyDrawOptions(TriangleShape, options);
        _window.Draw(TriangleShape);
    }

    /// <summary>
    /// Draws a filled convex polygon through the given points, in order.
    /// The points must describe a convex shape; concave outlines will render incorrectly.
    /// </summary>
    /// <param name="points">The polygon's vertices, in winding order. At least three are required.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void ConvexPolygon(IReadOnlyList<Vector2f> points, DrawOptions options = null)
    {
        if (points.Count < 3)
            throw new ArgumentException("A convex polygon needs at least three points.", nameof(points));

        PolygonShape.SetPointCount((uint)points.Count);
        for (uint i = 0; i < points.Count; i++)
            PolygonShape.SetPoint(i, points[(int)i]);

        ApplyDrawOptions(PolygonShape, options);
        _window.Draw(PolygonShape);
    }


    /// <summary>
    /// Draws a line between two points.
    /// </summary>
    /// <param name="start">The start point of the line.</param>
    /// <param name="end">The end point of the line.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Line(Vector2f start, Vector2f end, DrawOptions options = null)
    {
        var color = options?.FillColor ?? Color.White;
        LineVertices[0] = new Vertex(start, color);
        LineVertices[1] = new Vertex(end, color);
        _window.Draw(LineVertices);
    }

    /// <summary>
    /// Draws a prebuilt vertex array directly. The caller owns the array, making this the right
    /// entry point for static geometry (curves, grids, meshes) that is built once and drawn each frame.
    /// </summary>
    /// <param name="vertices">The vertex array to draw.</param>
    public static void Vertices(VertexArray vertices) => _window.Draw(vertices);

    /// <summary>
    /// Draws a connected polyline through the given points.
    /// </summary>
    /// <param name="points">The points to connect, in order.</param>
    /// <param name="options">Optional drawing options. <c>FillColor</c> sets the line color.</param>
    /// <param name="closed">When <c>true</c>, connects the last point back to the first.</param>
    public static void Polyline(IReadOnlyList<Vector2f> points, DrawOptions options = null, bool closed = false)
    {
        if (points.Count < 2)
            return;

        var color = options?.FillColor ?? Color.White;
        var count = (uint)(points.Count + (closed ? 1 : 0));
        var strip = new VertexArray(PrimitiveType.LineStrip, count);

        for (uint i = 0; i < points.Count; i++)
            strip[i] = new Vertex(points[(int)i], color);
        if (closed)
            strip[(uint)points.Count] = new Vertex(points[0], color);

        _window.Draw(strip);
    }

    /// <summary>
    /// Draws a string of text at the specified position with the given character size.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="position">The position of the text's top-left corner.</param>
    /// <param name="options">Optional drawing options. <c>FillColor</c> sets the text color.</param>
    public static void Text(string text, Vector2f position, TextDrawOptions options = null)
    {
        TextDrawable.DisplayedString = text;

        // SFML glyph bounds carry an internal offset; subtract it so the visual top-left sits at position.
        var bounds = TextDrawable.GetLocalBounds();
        TextDrawable.Position = new Vector2f(position.X - bounds.Left, position.Y - bounds.Top);

        ApplyDrawOptions(TextDrawable, options);
        _window.Draw(TextDrawable);
    }

    /// <summary>
    /// Clears the drawing window to a solid color, erasing the previous frame. Call this once at the
    /// start of rendering. Unlike clearing through <see cref="GameContext"/>, this targets the same
    /// window <see cref="Draw"/> renders to the drawing window has been
    /// redirected via <see cref="SetDrawingWindow"/>.
    /// </summary>
    /// <param name="color">The color to fill the window with. Defaults to <see cref="Color.Black"/>.</param>
    public static void Clear(Color? color = null) => _window.Clear(color ?? Color.Black);

    /// <summary>
    /// Sets the current rendering window.
    /// </summary>
    /// <param name="window">The rendering window to use.</param>
    public static void SetDrawingWindow(RenderWindow window) => _window = window;

    // The shape/text instances are shared and reused across every draw, so each call must fully
    // restate the styling it wants. Properties are assigned unconditionally with explicit fallbacks
    // (never left untouched), otherwise a draw would silently inherit rotation/outline/fill left
    // behind by a previous draw on the same instance. This is also why null options still resets.
    private static void ApplyDrawOptions(Drawable drawable, DrawOptions options)
    {
        switch (drawable)
        {
            case Text text:
                var textOptions = options as TextDrawOptions;
                text.Font = textOptions?.FontPath is null
                    ? EmbeddedResources.DefaultFont
                    : ResourceManager<Font>.GetResource(textOptions.FontPath);
                text.CharacterSize = textOptions?.CharacterSize ?? 16;
                text.OutlineColor = options?.OutlineColor ?? Color.Transparent;
                text.OutlineThickness = options?.OutlineThickness ?? 0f;
                text.Rotation = options?.Rotation ?? 0f;
                text.FillColor = ResolveFill(options);
                break;

            case Shape shape:
                shape.OutlineColor = options?.OutlineColor ?? Color.Transparent;
                shape.OutlineThickness = options?.OutlineThickness ?? 0f;
                shape.Rotation = options?.Rotation ?? 0f;
                shape.FillColor = ResolveFill(options);
                break;
        }
    }

    /// <summary>
    /// Resolves the fill color from the options, defaulting to white when none is given, then
    /// applies opacity to that resolved color. Opacity therefore modifies the requested fill rather
    /// than whatever color a previous draw left on a shared instance.
    /// </summary>
    private static Color ResolveFill(DrawOptions options)
    {
        var color = options?.FillColor ?? Color.White;
        if (options?.Opacity is { } opacity)
            color.A = (byte)(opacity * 255);
        return color;
    }
}