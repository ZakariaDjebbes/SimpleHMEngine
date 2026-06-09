using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for working with point intersections in a fluent manner.
/// </summary>
public static partial class FluentGeometry
{
    /// <summary>
    /// Determines if this point intersects with another point.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point1">The current point.</param>
    /// <param name="point2">The other point to check for intersection.</param>
    /// <returns>A collection containing the point if they intersect, otherwise empty.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Vector2D<T1> point1, Vector2D<T1> point2)
        where T1 : struct, INumber<T1> => Geometry.Intersects(point1, point2);

    /// <summary>
    /// Determines if this point intersects with a line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point">The current point.</param>
    /// <param name="line">The line to check for intersection.</param>
    /// <returns>A collection containing the point if it lies on the line, otherwise empty.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Vector2D<T1> point, Line<T1> line)
        where T1 : struct, INumber<T1> => Geometry.Intersects(point, line);

    /// <summary>
    /// Determines if this point intersects with a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point">The current point.</param>
    /// <param name="circle">The circle to check for intersection.</param>
    /// <returns>A collection containing the point if it lies on the circle's boundary, otherwise empty.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Vector2D<T1> point, Circle<T1> circle)
        where T1 : struct, INumber<T1> => Geometry.Intersects(point, circle);

    /// <summary>
    /// Determines if this point intersects with a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point">The current point.</param>
    /// <param name="rectangle">The rectangle to check for intersection.</param>
    /// <returns>A collection containing the point if it lies on the rectangle's boundary, otherwise empty.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Vector2D<T1> point, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1> => Geometry.Intersects(point, rectangle);

    /// <summary>
    /// Determines if this point intersects with a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point">The current point.</param>
    /// <param name="triangle">The triangle to check for intersection.</param>
    /// <returns>A collection containing the point if it lies on the triangle's boundary, otherwise empty.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Vector2D<T1> point, Triangle<T1> triangle)
        where T1 : struct, INumber<T1> => Geometry.Intersects(point, triangle);
}