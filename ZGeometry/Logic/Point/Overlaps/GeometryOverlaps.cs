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
    /// Determines whether two points coincide.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point1">The first point.</param>
    /// <param name="point2">The second point.</param>
    /// <returns>True if the points overlap; otherwise, false.</returns>
    public static bool Overlaps<T1>(Vector2D<T1> point1, Vector2D<T1> point2)
        where T1 : struct, INumber<T1> => Contains(point1, point2);

    /// <summary>
    /// Determines whether a point lies on a line segment.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point">The point to check.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the point lies on the line; otherwise, false.</returns>
    public static bool Overlaps<T1>(Vector2D<T1> point, Line<T1> line)
        where T1 : struct, INumber<T1> => Contains(point, line);

    /// <summary>
    /// Determines whether a point lies inside or on the boundary of a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point">The point to check.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>True if the point overlaps the triangle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Vector2D<T1> point, Triangle<T1> triangle)
        where T1 : struct, INumber<T1> => Contains(point, triangle);

    /// <summary>
    /// Determines whether a point lies inside or on the boundary of a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point">The point to check.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>True if the point overlaps the rectangle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Vector2D<T1> point, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1> => Contains(rectangle, point);

    /// <summary>
    /// Determines whether a point lies inside or on the boundary of a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="point">The point to check.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>True if the point overlaps the circle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Vector2D<T1> point, Circle<T1> circle)
        where T1 : struct, INumber<T1> => Contains(point, circle);
}
