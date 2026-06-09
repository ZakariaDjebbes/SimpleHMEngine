using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for working with circle intersections in a fluent manner.
/// </summary>
public static partial class FluentGeometry
{
    /// <summary>
    /// Determines the intersection points between this circle and another circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle1">The current circle.</param>
    /// <param name="circle2">The other circle to check for intersections.</param>
    /// <returns>A collection of intersection points between the circles.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Circle<T1> circle1, Circle<T1> circle2)
        where T1 : struct, INumber<T1> => Geometry.Intersects(circle1, circle2);

    /// <summary>
    /// Determines the intersection points between this circle and a line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current circle.</param>
    /// <param name="line">The line to check for intersections.</param>
    /// <returns>A collection of intersection points between the circle and line.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Circle<T1> circle, Line<T1> line)
        where T1 : struct, INumber<T1> => Geometry.Intersects(circle, line);

    /// <summary>
    /// Determines the intersection points between this circle and a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current circle.</param>
    /// <param name="rectangle">The rectangle to check for intersections.</param>
    /// <returns>A collection of intersection points between the circle and rectangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Circle<T1> circle, Rectangle<T1> rectangle) where T1 : struct, INumber<T1> => Geometry.Intersects(circle, rectangle);

    /// <summary>
    /// Determines the intersection points between this circle and a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current circle.</param>
    /// <param name="triangle">The triangle to check for intersections.</param>
    /// <returns>A collection of intersection points between the circle and triangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Circle<T1> circle, Triangle<T1> triangle)
        where T1 : struct, INumber<T1> => Geometry.Intersects(circle, triangle);
}