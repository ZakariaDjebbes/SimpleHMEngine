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
    /// Determines whether a point is inside or on the boundary of a rectangle.
    /// </summary>
    /// <param name="rectangle">The <see cref="Rectangle{T}"/> to check.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <returns>True if the point is inside or on the boundary of the rectangle; otherwise, false.</returns>
    public static bool Contains<T>(Rectangle<T> rectangle, Vector2D<T> point) where T : struct, INumber<T>
        => !(point.X < rectangle.Position.X || point.Y < rectangle.Position.Y ||
             point.X > rectangle.Position.X + rectangle.Size.X || point.Y > rectangle.Position.Y + rectangle.Size.Y);

    public static bool Contains<T1>(Rectangle<T1> r1, Rectangle<T1> r2)
        where T1 : struct, INumber<T1>
        => r2.Position.X >= r1.Position.X && r2.Position.X + r2.Size.X <= r1.Position.X + r1.Size.X &&
           r2.Position.Y >= r1.Position.Y && r2.Position.Y + r2.Size.Y <= r1.Position.Y + r1.Size.Y;

    public static bool Contains<T1>(Rectangle<T1> rect, Circle<T1> circle)
        where T1 : struct, INumber<T1>
        => rect.Position.X + circle.Radius <= circle.Center.X
           && circle.Center.X <= rect.Position.X + rect.Size.X - circle.Radius
           && rect.Position.Y + circle.Radius <= circle.Center.Y
           && circle.Center.Y <= rect.Position.Y + rect.Size.Y - circle.Radius;

    /// <summary>
    /// Determines whether a line segment lies entirely inside or on the boundary of a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rect">The <see cref="Rectangle{T1}"/> to check.</param>
    /// <param name="line">The <see cref="Line{T1}"/> to check.</param>
    /// <returns>True if both endpoints of the line are inside or on the rectangle; otherwise, false.</returns>
    public static bool Contains<T1>(Rectangle<T1> rect, Line<T1> line)
        where T1 : struct, INumber<T1>
        => Contains(rect, line.Start) && Contains(rect, line.End);

    /// <summary>
    /// Determines whether a triangle lies entirely inside or on the boundary of a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rect">The <see cref="Rectangle{T1}"/> to check.</param>
    /// <param name="triangle">The <see cref="Triangle{T1}"/> to check.</param>
    /// <returns>True if all three vertices of the triangle are inside or on the rectangle; otherwise, false.</returns>
    public static bool Contains<T1>(Rectangle<T1> rect, Triangle<T1> triangle)
        where T1 : struct, INumber<T1>
        => Contains(rect, triangle.Position[0]) &&
           Contains(rect, triangle.Position[1]) &&
           Contains(rect, triangle.Position[2]);
}