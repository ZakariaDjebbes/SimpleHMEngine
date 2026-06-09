using System.Numerics;
using Core.Engine;
using Core.Resources;
using SFML.Graphics;
using SFML.System;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace Core.Drawing;

/// <summary>
/// Provides drawing utilities for various shapes, such as circles, rectangles, triangles, and lines, using SFML.
/// </summary>
public static class Draw
{
    private static RenderWindow _window = GameContext.CurrentWindow;

    private static readonly Dictionary<float, CircleShape> CircleCache = new();
    private static readonly Dictionary<Vector2f, RectangleShape> RectangleCache = new();
    private static readonly ConvexShape TriangleShape = new(3);
    private static readonly Dictionary<(Vector2f, Vector2f), VertexArray> LineCache = new();
    private static readonly Text TextDrawable = new() { Font = EmbeddedResources.DefaultFont };

    /// <summary>
    /// Draws a circle at the specified center with the given radius.
    /// </summary>
    /// <param name="center">The center position of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Circle(Vector2f center, float radius, DrawOptions options = null)
    {
        if (!CircleCache.TryGetValue(radius, out var circle))
        {
            circle = new CircleShape(radius)
            {
                Origin = new Vector2f(radius, radius)
            };
            CircleCache[radius] = circle;
        }

        circle.Position = center;
        ApplyDrawOptions(circle, options);
        _window.Draw(circle);
    }

    /// <summary>
    /// Draws a circle defined by a generic <see cref="Circle{T}"/> object.
    /// </summary>
    /// <typeparam name="T">The numeric type of the circle's properties.</typeparam>
    /// <param name="circle">The circle to draw.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Circle<T>(Circle<T> circle, DrawOptions options = null)
        where T : struct, INumber<T>
    {
        var x = float.CreateTruncating(circle.Center.X);
        var y = float.CreateTruncating(circle.Center.Y);
        var r = float.CreateTruncating(circle.Radius);
        Circle(new Vector2f(x, y), r, options);
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
        var position = new Vector2f(x, y);
        var size = new Vector2f(width, height);

        if (!RectangleCache.TryGetValue(size, out var rectangle))
        {
            rectangle = new RectangleShape(size);
            RectangleCache[size] = rectangle;
        }

        rectangle.Position = position;
        ApplyDrawOptions(rectangle, options);
        _window.Draw(rectangle);
    }

    /// <summary>
    /// Draws a rectangle defined by a generic <see cref="Rectangle{T}"/> object.
    /// </summary>
    /// <typeparam name="T">The numeric type of the rectangle's properties.</typeparam>
    /// <param name="rectangle">The rectangle to draw.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Rectangle<T>(Rectangle<T> rectangle, DrawOptions options = null)
        where T : struct, INumber<T>
    {
        var x = float.CreateTruncating(rectangle.Position.X);
        var y = float.CreateTruncating(rectangle.Position.Y);
        var width = float.CreateTruncating(rectangle.Size.X);
        var height = float.CreateTruncating(rectangle.Size.Y);
        Rectangle(x, y, width, height, options);
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
    /// Draws a triangle defined by a generic <see cref="Triangle{T}"/> object.
    /// </summary>
    /// <typeparam name="T">The numeric type of the triangle's properties.</typeparam>
    /// <param name="triangle">The triangle to draw.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Triangle<T>(Triangle<T> triangle, DrawOptions options = null)
        where T : struct, INumber<T>
    {
        var x1 = float.CreateTruncating(triangle.Position[0].X);
        var y1 = float.CreateTruncating(triangle.Position[0].Y);
        var x2 = float.CreateTruncating(triangle.Position[1].X);
        var y2 = float.CreateTruncating(triangle.Position[1].Y);
        var x3 = float.CreateTruncating(triangle.Position[2].X);
        var y3 = float.CreateTruncating(triangle.Position[2].Y);

        Triangle(new Vector2f(x1, y1), new Vector2f(x2, y2), new Vector2f(x3, y3), options);
    }


    /// <summary>
    /// Draws a line between two points.
    /// </summary>
    /// <param name="start">The start point of the line.</param>
    /// <param name="end">The end point of the line.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Line(Vector2f start, Vector2f end, DrawOptions options = null)
    {
        var key = (start, end);

        if (!LineCache.TryGetValue(key, out var line))
        {
            line = new VertexArray(PrimitiveType.Lines, 2)
            {
                [0] = new Vertex(start),
                [1] = new Vertex(end)
            };

            LineCache[key] = line;
        }

        var color = options?.FillColor ?? Color.White;
        line[0] = new Vertex(start, color);
        line[1] = new Vertex(end, color);

        _window.Draw(line);
    }

    /// <summary>
    /// Draws a line defined by a generic <see cref="Line{T}"/> object.
    /// </summary>
    /// <typeparam name="T">The numeric type of the line's properties.</typeparam>
    /// <param name="line">The line to draw.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Line<T>(Line<T> line, DrawOptions options = null)
        where T : struct, INumber<T>
    {
        var x1 = float.CreateTruncating(line.Start.X);
        var y1 = float.CreateTruncating(line.Start.Y);
        var x2 = float.CreateTruncating(line.End.X);
        var y2 = float.CreateTruncating(line.End.Y);
        Line(new Vector2f(x1, y1), new Vector2f(x2, y2), options);
    }

    /// <summary>
    /// Draws a string of text at the specified position with the given character size.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="position">The position of the text's top-left corner.</param>
    /// <param name="characterSize">The character size in pixels.</param>
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
    /// Sets the current rendering window.
    /// </summary>
    /// <param name="window">The rendering window to use.</param>
    public static void SetDrawingWindow(RenderWindow window) => _window = window;

    private static void ApplyDrawOptions(Drawable drawable, DrawOptions options)
    {
        if (options is null)
            return;

        switch (drawable)
        {
            case Text text when options is TextDrawOptions textOptions:
                text.Font = textOptions.FontPath is null
                    ? EmbeddedResources.DefaultFont
                    : ResourceManager<Font>.GetResource(textOptions.FontPath);
                text.CharacterSize = textOptions.CharacterSize ?? 16;
                ApplyCommon(
                    fill: c => text.FillColor = c,
                    outline: c => text.OutlineColor = c,
                    thickness: v => text.OutlineThickness = v,
                    rotation: v => text.Rotation = v,
                    getFill: () => text.FillColor,
                    setFill: c => text.FillColor = c,
                    options);
                break;

            case Shape shape:
                ApplyCommon(
                    fill: c => shape.FillColor = c,
                    outline: c => shape.OutlineColor = c,
                    thickness: v => shape.OutlineThickness = v,
                    rotation: v => shape.Rotation = v,
                    getFill: () => shape.FillColor,
                    setFill: c => shape.FillColor = c,
                    options);
                break;
        }
    }

    private static void ApplyCommon(
        Action<Color> fill,
        Action<Color> outline,
        Action<float> thickness,
        Action<float> rotation,
        Func<Color> getFill,
        Action<Color> setFill,
        DrawOptions options)
    {
        if (options.FillColor is { } fillColor)
            fill(fillColor);

        if (options.OutlineColor is { } outlineColor)
            outline(outlineColor);

        if (options.OutlineThickness is { } outlineThickness)
            thickness(outlineThickness);

        if (options.Rotation is { } rot)
            rotation(rot);

        if (options.Opacity is { } opacity)
        {
            var color = getFill();
            color.A = (byte)(opacity * 255);
            setFill(color);
        }
    }
}