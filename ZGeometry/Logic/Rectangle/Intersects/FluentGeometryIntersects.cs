using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for working with rectangle intersections in a fluent manner.
/// </summary>
public static partial class FluentGeometry
{
    /// <summary>
    /// Determines the intersection points between this rectangle and a line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current rectangle.</param>
    /// <param name="line">The line to check for intersections.</param>
    /// <returns>A collection of intersection points between the rectangle and line.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Rectangle<T1> rectangle, Line<T1> line)
        where T1 : struct, INumber<T1> => Geometry.Intersects(rectangle, line);
    
    /// <summary>
    /// Determines the intersection points between this rectangle and another rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle1">The current rectangle.</param>
    /// <param name="rectangle2">The other rectangle to check for intersections.</param>
    /// <returns>A collection of intersection points between the rectangles.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Rectangle<T1> rectangle1, Rectangle<T1> rectangle2)
        where T1 : struct, INumber<T1> => Geometry.Intersects(rectangle1, rectangle2);

    /// <summary>
    /// Determines the intersection points between this rectangle and a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current rectangle.</param>
    /// <param name="circle">The circle to check for intersections.</param>
    /// <returns>A collection of intersection points between the rectangle and circle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Rectangle<T1> rectangle, Circle<T1> circle) where T1 : struct, INumber<T1> => Geometry.Intersects(rectangle, circle);

    /// <summary>
    /// Determines the intersection points between this rectangle and a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current rectangle.</param>
    /// <param name="triangle">The triangle to check for intersections.</param>
    /// <returns>A collection of intersection points between the rectangle and triangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Rectangle<T1> rectangle, Triangle<T1> triangle)
        where T1 : struct, INumber<T1> => Geometry.Intersects(rectangle, triangle);
}