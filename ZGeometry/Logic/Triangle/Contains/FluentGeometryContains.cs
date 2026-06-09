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
    /// Determines whether a point is inside or on the boundary of the current triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current <see cref="Triangle{T}"/>.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <returns>True if the point is inside or on the boundary of the triangle; otherwise, false.</returns>
    public static bool Contains<T>(this Triangle<T> triangle, Vector2D<T> point) where T : struct, INumber<T>
        => Geometry.Contains(triangle, point);

    /// <summary>
    /// Determines whether a line segment lies entirely inside or on the boundary of the current triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current <see cref="Triangle{T}"/>.</param>
    /// <param name="line">The <see cref="Line{T}"/> to check.</param>
    /// <returns>True if both endpoints of the line are inside or on the triangle; otherwise, false.</returns>
    public static bool Contains<T>(this Triangle<T> triangle, Line<T> line) where T : struct, INumber<T>
        => Geometry.Contains(triangle, line);

    /// <summary>
    /// Determines whether another triangle lies entirely inside or on the boundary of the current triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="outer">The current (containing) <see cref="Triangle{T}"/>.</param>
    /// <param name="inner">The <see cref="Triangle{T}"/> to check.</param>
    /// <returns>True if the inner triangle is fully contained by the outer triangle; otherwise, false.</returns>
    public static bool Contains<T>(this Triangle<T> outer, Triangle<T> inner) where T : struct, INumber<T>
        => Geometry.Contains(outer, inner);

    /// <summary>
    /// Determines whether a rectangle lies entirely inside or on the boundary of the current triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current <see cref="Triangle{T}"/>.</param>
    /// <param name="rect">The <see cref="Rectangle{T}"/> to check.</param>
    /// <returns>True if all four corners of the rectangle are inside or on the triangle; otherwise, false.</returns>
    public static bool Contains<T>(this Triangle<T> triangle, Rectangle<T> rect) where T : struct, INumber<T>
        => Geometry.Contains(triangle, rect);

    /// <summary>
    /// Determines whether a circle lies entirely inside or on the boundary of the current triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current <see cref="Triangle{T}"/>.</param>
    /// <param name="circle">The <see cref="Circle{T}"/> to check.</param>
    /// <returns>True if the circle is fully contained by the triangle; otherwise, false.</returns>
    public static bool Contains<T>(this Triangle<T> triangle, Circle<T> circle) where T : struct, INumber<T>
        => Geometry.Contains(triangle, circle);
}
