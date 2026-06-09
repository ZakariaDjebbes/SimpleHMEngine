using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace Core.Drawing;

/// <summary>
/// Provides extension methods for drawing geometric primitives using a fluent API.
/// </summary>
public static class FluentDraw
{
    /// <summary>
    /// Draws a circle using the specified draw options.
    /// </summary>
    /// <typeparam name="T">The numeric type of the circle's dimensions (e.g., <see cref="int"/>, <see cref="float"/>, or <see cref="double"/>).</typeparam>
    /// <param name="circle">The circle to be drawn.</param>
    /// <param name="options">Optional drawing parameters.</param>
    public static void DrawSelf<T>(this Circle<T> circle, DrawOptions options = null)
        where T : struct, INumber<T> => Draw.Circle(circle, options);

    /// <summary>
    /// Draws a rectangle using the specified draw options.
    /// </summary>
    /// <typeparam name="T">The numeric type of the rectangle's dimensions (e.g., <see cref="int"/>, <see cref="float"/>, or <see cref="double"/>).</typeparam>
    /// <param name="rectangle">The rectangle to be drawn.</param>
    /// <param name="options">Optional drawing parameters</param>
    public static void DrawSelf<T>(this Rectangle<T> rectangle, DrawOptions options = null)
        where T : struct, INumber<T> => Draw.Rectangle(rectangle, options);

    /// <summary>
    /// Draws a line using the specified draw options.
    /// </summary>
    /// <typeparam name="T">The numeric type of the line's dimensions (e.g., <see cref="int"/>, <see cref="float"/>, or <see cref="double"/>).</typeparam>
    /// <param name="line">The line to be drawn.</param>
    /// <param name="options">Optional drawing parameters.</param>
    public static void DrawSelf<T>(this Line<T> line, DrawOptions options = null)
        where T : struct, INumber<T> => Draw.Line(line, options);
    
    /// <summary>
    /// Draws a triangle using the specified draw options.
    /// </summary>
    /// <typeparam name="T">The numeric type of the triangle's dimensions (e.g., <see cref="int"/>, <see cref="float"/>, or <see cref="double"/>).</typeparam>
    /// <param name="triangle">The triangle to be drawn.</param>
    /// <param name="options">Optional drawing parameters.</param>
    public static void DrawSelf<T>(this Triangle<T> triangle, DrawOptions options = null)
        where T : struct, INumber<T> => Draw.Triangle(triangle, options);
}
