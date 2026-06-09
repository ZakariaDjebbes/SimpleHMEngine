using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

public static partial class Geometry
{
    /// <summary>
    /// Determines whether a circle overlaps a point.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The circle to check.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point lies inside or on the circle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Circle<T1> circle, Vector2D<T1> point)
        where T1 : struct, INumber<T1> => Overlaps(point, circle);

    /// <summary>
    /// Determines whether a circle overlaps a line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The circle to check.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the line touches or enters the circle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Circle<T1> circle, Line<T1> line)
        where T1 : struct, INumber<T1> => Overlaps(line, circle);

    /// <summary>
    /// Determines whether a circle overlaps a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The circle to check.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>True if the circle and triangle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T1>(Circle<T1> circle, Triangle<T1> triangle)
        where T1 : struct, INumber<T1> => Overlaps(triangle, circle);

    /// <summary>
    /// Determines whether a circle overlaps a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The circle to check.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>True if the circle and rectangle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T1>(Circle<T1> circle, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1> => Overlaps(rectangle, circle);

    /// <summary>
    /// Determines whether two circles overlap.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle1">The first circle.</param>
    /// <param name="circle2">The second circle.</param>
    /// <returns>True if the distance between centers is at most the sum of the radii; otherwise, false.</returns>
    public static bool Overlaps<T1>(Circle<T1> circle1, Circle<T1> circle2)
        where T1 : struct, INumber<T1>
        => (circle1.Center - circle2.Center).Magnitude() <= circle1.Radius + circle2.Radius;
}
