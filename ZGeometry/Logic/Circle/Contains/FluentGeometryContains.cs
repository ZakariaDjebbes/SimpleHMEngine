using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for working with 2D points and shapes in a fluent manner.
/// </summary>
public static partial class FluentGeometryContains
{
    /// <summary>
    /// Determines whether a point is inside or on the boundary of the current circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current <see cref="Circle{T}"/>.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <returns>True if the point is inside or on the boundary of the circle; otherwise, false.</returns>
    public static bool Contains<T>(this Circle<T> circle, Vector2D<T> point) where T : struct, INumber<T>
        => Geometry.Contains(circle, point);

    /// <summary>Determines whether this circle fully contains a rectangle.</summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current circle.</param>
    /// <param name="rect">The rectangle to test for containment.</param>
    /// <returns>True if all four corners of the rectangle lie inside the circle; otherwise, false.</returns>
    public static bool Contains<T>(this Circle<T> circle, Rectangle<T> rect) where T : struct, INumber<T>
        => Geometry.Contains(circle, rect);

    /// <summary>
    /// Determines whether a line segment lies entirely inside or on the boundary of the current circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current <see cref="Circle{T}"/>.</param>
    /// <param name="line">The <see cref="Line{T}"/> to check.</param>
    /// <returns>True if both endpoints of the line are inside or on the circle; otherwise, false.</returns>
    public static bool Contains<T>(this Circle<T> circle, Line<T> line) where T : struct, INumber<T>
        => Geometry.Contains(circle, line);

    /// <summary>
    /// Determines whether another circle lies entirely inside or on the boundary of the current circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="outer">The current (containing) <see cref="Circle{T}"/>.</param>
    /// <param name="inner">The <see cref="Circle{T}"/> to check.</param>
    /// <returns>True if the inner circle is fully contained by the outer circle; otherwise, false.</returns>
    public static bool Contains<T>(this Circle<T> outer, Circle<T> inner) where T : struct, INumber<T>
        => Geometry.Contains(outer, inner);

    /// <summary>
    /// Determines whether a triangle lies entirely inside or on the boundary of the current circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current <see cref="Circle{T}"/>.</param>
    /// <param name="triangle">The <see cref="Triangle{T}"/> to check.</param>
    /// <returns>True if all three vertices of the triangle are inside or on the circle; otherwise, false.</returns>
    public static bool Contains<T>(this Circle<T> circle, Triangle<T> triangle) where T : struct, INumber<T>
        => Geometry.Contains(circle, triangle);
}