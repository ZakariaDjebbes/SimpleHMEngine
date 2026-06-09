using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for testing whether shapes overlap in a fluent manner.
/// </summary>
public static partial class FluentGeometryOverlaps
{
    /// <summary>
    /// Determines whether this triangle overlaps a point.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current triangle.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point lies inside or on the triangle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Triangle<T> triangle, Vector2D<T> point) where T : struct, INumber<T>
        => Geometry.Overlaps(triangle, point);

    /// <summary>
    /// Determines whether this triangle overlaps a line.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current triangle.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the line touches or enters the triangle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Triangle<T> triangle, Line<T> line) where T : struct, INumber<T>
        => Geometry.Overlaps(triangle, line);

    /// <summary>
    /// Determines whether this triangle overlaps another triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle1">The current triangle.</param>
    /// <param name="triangle2">The other triangle to check.</param>
    /// <returns>True if the triangles share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T>(this Triangle<T> triangle1, Triangle<T> triangle2) where T : struct, INumber<T>
        => Geometry.Overlaps(triangle1, triangle2);

    /// <summary>
    /// Determines whether this triangle overlaps a rectangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current triangle.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>True if the triangle and rectangle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T>(this Triangle<T> triangle, Rectangle<T> rectangle) where T : struct, INumber<T>
        => Geometry.Overlaps(triangle, rectangle);

    /// <summary>
    /// Determines whether this triangle overlaps a circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current triangle.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>True if the triangle and circle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T>(this Triangle<T> triangle, Circle<T> circle) where T : struct, INumber<T>
        => Geometry.Overlaps(triangle, circle);
}
