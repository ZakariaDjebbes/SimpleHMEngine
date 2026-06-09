using System.Numerics;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for working with line intersections in a fluent manner.
/// </summary>
public static partial class FluentGeometry
{
    /// <summary>
    /// Determines the intersection points between this line and another line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line1">The current line.</param>
    /// <param name="line2">The other line to check for intersections.</param>
    /// <param name="infinite">If true, treats the lines as infinite lines; otherwise, treats them as line segments.</param>
    /// <returns>A collection of intersection points between the lines.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Line<T1> line1, Line<T1> line2, bool infinite = false) where T1 : struct, INumber<T1> => Geometry.Intersects(line1, line2, infinite);

    /// <summary>
    /// Determines the intersection points between this line and a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line">The current line.</param>
    /// <param name="rectangle">The rectangle to check for intersections.</param>
    /// <returns>A collection of intersection points between the line and rectangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Line<T1> line, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1> => Geometry.Intersects(line, rectangle);

    /// <summary>
    /// Determines the intersection points between this line and a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line">The current line.</param>
    /// <param name="triangle">The triangle to check for intersections.</param>
    /// <returns>A collection of intersection points between the line and triangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(this Line<T1> line, Triangle<T1> triangle)
        where T1 : struct, INumber<T1> => Geometry.Intersects(line, triangle);
}