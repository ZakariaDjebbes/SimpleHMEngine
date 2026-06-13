using System.Numerics;
using Core.Drawing.DrawOption;
using SFML.System;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace Core.Drawing;

/// <summary>
/// Provides extension methods for drawing ZGeometry primitives using a fluent API.
/// This is the single adapter layer between the generic geometry types and <see cref="Draw"/>,
/// which speaks only SFML's float-based vocabulary. Numeric components are truncated to
/// <see cref="float"/> here, since SFML renders in float regardless of the source type.
/// </summary>
public static class FluentDraw
{
    /// <summary>
    /// Converts generic numeric types to a <see cref="Vector2f"/> for drawing.
    /// </summary>
    /// <param name="x">
    ///    The X-coordinate of the vector, which will be truncated to a float for SFML rendering.
    /// </param>
    /// <param name="y">
    ///     The Y-coordinate of the vector, which will be truncated to a float for SFML rendering.
    /// </param>
    /// <typeparam name="T">The numeric type of the primitive (e.g., <see cref="int"/>, <see cref="float"/>, or <see cref="double"/>).</typeparam>
    /// <returns>
    ///  A <see cref="Vector2f"/> containing the truncated float values of the input coordinates, suitable for use with SFML drawing methods.
    /// </returns>
    // ReSharper disable once InconsistentNaming
    private static Vector2f ToVector2f<T>(T x, T y) where T : struct, INumber<T>
        => new(float.CreateTruncating(x), float.CreateTruncating(y));

    /// <summary>
    /// Draws a circle using the specified draw options.
    /// </summary>
    /// <typeparam name="T">The numeric type of the circle's dimensions (e.g., <see cref="int"/>, <see cref="float"/>, or <see cref="double"/>).</typeparam>
    /// <param name="circle">The circle to be drawn.</param>
    /// <param name="options">Optional drawing parameters.</param>
    public static void DrawSelf<T>(this Circle<T> circle, DrawOptions options = null)
        where T : struct, INumber<T>
        => Draw.Circle(ToVector2f(circle.Center.X, circle.Center.Y), float.CreateTruncating(circle.Radius), options);

    /// <summary>
    /// Draws a rectangle using the specified draw options.
    /// </summary>
    /// <typeparam name="T">The numeric type of the rectangle's dimensions (e.g., <see cref="int"/>, <see cref="float"/>, or <see cref="double"/>).</typeparam>
    /// <param name="rectangle">The rectangle to be drawn.</param>
    /// <param name="options">Optional drawing parameters</param>
    public static void DrawSelf<T>(this Rectangle<T> rectangle, DrawOptions options = null)
        where T : struct, INumber<T>
        => Draw.Rectangle(
            float.CreateTruncating(rectangle.Position.X),
            float.CreateTruncating(rectangle.Position.Y),
            float.CreateTruncating(rectangle.Size.X),
            float.CreateTruncating(rectangle.Size.Y),
            options);

    /// <summary>
    /// Draws a line using the specified draw options.
    /// </summary>
    /// <typeparam name="T">The numeric type of the line's dimensions (e.g., <see cref="int"/>, <see cref="float"/>, or <see cref="double"/>).</typeparam>
    /// <param name="line">The line to be drawn.</param>
    /// <param name="options">Optional drawing parameters.</param>
    public static void DrawSelf<T>(this Line<T> line, DrawOptions options = null)
        where T : struct, INumber<T>
        => Draw.Line(ToVector2f(line.Start.X, line.Start.Y), ToVector2f(line.End.X, line.End.Y), options);

    /// <summary>
    /// Draws a triangle using the specified draw options.
    /// </summary>
    /// <typeparam name="T">The numeric type of the triangle's dimensions (e.g., <see cref="int"/>, <see cref="float"/>, or <see cref="double"/>).</typeparam>
    /// <param name="triangle">The triangle to be drawn.</param>
    /// <param name="options">Optional drawing parameters.</param>
    public static void DrawSelf<T>(this Triangle<T> triangle, DrawOptions options = null)
        where T : struct, INumber<T>
        => Draw.Triangle(
            ToVector2f(triangle.Position[0].X, triangle.Position[0].Y),
            ToVector2f(triangle.Position[1].X, triangle.Position[1].Y),
            ToVector2f(triangle.Position[2].X, triangle.Position[2].Y),
            options);
}