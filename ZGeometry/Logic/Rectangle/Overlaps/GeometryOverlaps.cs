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
    /// Determines whether a rectangle overlaps a point.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point lies inside or on the rectangle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Rectangle<T1> rectangle, Vector2D<T1> point)
        where T1 : struct, INumber<T1> => Overlaps(point, rectangle);

    /// <summary>
    /// Determines whether a rectangle overlaps a line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the line touches or enters the rectangle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Rectangle<T1> rectangle, Line<T1> line)
        where T1 : struct, INumber<T1> => Overlaps(line, rectangle);

    /// <summary>
    /// Determines whether a rectangle overlaps a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>True if the rectangle and triangle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T1>(Rectangle<T1> rectangle, Triangle<T1> triangle)
        where T1 : struct, INumber<T1> => Overlaps(triangle, rectangle);

    /// <summary>
    /// Determines whether two rectangles overlap.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle1">The first rectangle.</param>
    /// <param name="rectangle2">The second rectangle.</param>
    /// <returns>True if the rectangles share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T1>(Rectangle<T1> rectangle1, Rectangle<T1> rectangle2)
        where T1 : struct, INumber<T1>
        => rectangle1.Position.X <= rectangle2.Position.X + rectangle2.Size.X &&
           rectangle2.Position.X <= rectangle1.Position.X + rectangle1.Size.X &&
           rectangle1.Position.Y <= rectangle2.Position.Y + rectangle2.Size.Y &&
           rectangle2.Position.Y <= rectangle1.Position.Y + rectangle1.Size.Y;

    /// <summary>
    /// Determines whether a rectangle overlaps a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>True if the rectangle and circle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T1>(Rectangle<T1> rectangle, Circle<T1> circle)
        where T1 : struct, INumber<T1>
    {
        var closest = circle.Center.Clamp(rectangle.Position, rectangle.Position + rectangle.Size);
        return (closest - circle.Center).MagnitudeSquared() <= circle.Radius * circle.Radius;
    }
}
