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
    /// Determines whether a line overlaps a point.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line">The line to check.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point lies on the line; otherwise, false.</returns>
    public static bool Overlaps<T1>(Line<T1> line, Vector2D<T1> point)
        where T1 : struct, INumber<T1> => Overlaps(point, line);

    /// <summary>
    /// Determines whether two line segments overlap.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line1">The first line.</param>
    /// <param name="line2">The second line.</param>
    /// <returns>True if the segments intersect; otherwise, false.</returns>
    public static bool Overlaps<T1>(Line<T1> line1, Line<T1> line2)
        where T1 : struct, INumber<T1> => Intersects(line1, line2).Any();

    /// <summary>
    /// Determines whether a line overlaps a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line">The line to check.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>True if the line touches or enters the triangle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Line<T1> line, Triangle<T1> triangle)
        where T1 : struct, INumber<T1>
        => Contains(line.Start, triangle) || Contains(line.End, triangle) || Intersects(line, triangle).Any();

    /// <summary>
    /// Determines whether a line overlaps a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line">The line to check.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>True if the line touches or enters the rectangle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Line<T1> line, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1>
        => Contains(rectangle, line.Start) || Contains(rectangle, line.End) || Intersects(line, rectangle).Any();

    /// <summary>
    /// Determines whether a line overlaps a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line">The line to check.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>True if the line touches or enters the circle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Line<T1> line, Circle<T1> circle)
        where T1 : struct, INumber<T1>
        => Contains(circle, line.Start) || Contains(circle, line.End) || Intersects(line, circle).Any();
}
