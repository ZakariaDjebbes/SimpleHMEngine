using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for working with triangle intersections in a fluent manner.
/// </summary>
public static partial class FluentGeometry
{
    /// <summary>
    /// Determines the intersection points between this triangle and a point.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current triangle.</param>
    /// <param name="point">The point to check for intersection.</param>
    /// <returns>A collection containing the point if it lies on the triangle's boundary, otherwise empty.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Triangle<T1> triangle, Vector2D<T1> point)
        where T1 : struct, INumber<T1> => Geometry.Intersects(triangle, point);

    /// <summary>
    /// Determines the intersection points between this triangle and a line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current triangle.</param>
    /// <param name="line">The line to check for intersections.</param>
    /// <returns>A collection of intersection points between the triangle and line.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Triangle<T1> triangle, Line<T1> line)
        where T1 : struct, INumber<T1> => Geometry.Intersects(triangle, line);

    /// <summary>
    /// Determines the intersection points between this triangle and another triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle1">The current triangle.</param>
    /// <param name="triangle2">The other triangle to check for intersections.</param>
    /// <returns>A collection of intersection points between the triangles.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Triangle<T1> triangle1, Triangle<T1> triangle2)
        where T1 : struct, INumber<T1> => Geometry.Intersects(triangle1, triangle2);

    /// <summary>
    /// Determines the intersection points between this triangle and a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current triangle.</param>
    /// <param name="rectangle">The rectangle to check for intersections.</param>
    /// <returns>A collection of intersection points between the triangle and rectangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Triangle<T1> triangle, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1> => Geometry.Intersects(triangle, rectangle);

    /// <summary>
    /// Determines the intersection points between this triangle and a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The current triangle.</param>
    /// <param name="circle">The circle to check for intersections.</param>
    /// <returns>A collection of intersection points between the triangle and circle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Triangle<T1> triangle, Circle<T1> circle)
        where T1 : struct, INumber<T1> => Geometry.Intersects(triangle, circle);
}
