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
    /// Determines whether a point is inside or on the boundary of a circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The <see cref="Circle{T}"/> to check.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <returns>True if the point is inside or on the boundary of the circle; otherwise, false.</returns>
    public static bool Contains<T>(Circle<T> circle, Vector2D<T> point) where T : struct, INumber<T>
        => (circle.Center - point).MagnitudeSquared() <= circle.Radius * circle.Radius;

    public static bool Contains<T1>(Circle<T1> circle, Rectangle<T1> rect)
        where T1 : struct, INumber<T1>
        => Contains(circle, rect.Position) &&
           Contains(circle, Vector2D<T1>.Create(rect.Position.X + rect.Size.X, rect.Position.Y)) &&
           Contains(circle, Vector2D<T1>.Create(rect.Position.X, rect.Position.Y + rect.Size.Y)) &&
           Contains(circle, rect.Position + rect.Size);

    /// <summary>
    /// Determines whether a line segment lies entirely inside or on the boundary of a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The <see cref="Circle{T1}"/> to check.</param>
    /// <param name="line">The <see cref="Line{T1}"/> to check.</param>
    /// <returns>True if both endpoints of the line are inside or on the circle; otherwise, false.</returns>
    public static bool Contains<T1>(Circle<T1> circle, Line<T1> line)
        where T1 : struct, INumber<T1>
        => Contains(circle, line.Start) && Contains(circle, line.End);

    /// <summary>
    /// Determines whether a circle lies entirely inside or on the boundary of another circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="outer">The containing <see cref="Circle{T1}"/>.</param>
    /// <param name="inner">The <see cref="Circle{T1}"/> to check.</param>
    /// <returns>True if the inner circle is fully contained by the outer circle; otherwise, false.</returns>
    public static bool Contains<T1>(Circle<T1> outer, Circle<T1> inner)
        where T1 : struct, INumber<T1>
        => (outer.Center - inner.Center).Magnitude() + inner.Radius <= outer.Radius;

    /// <summary>
    /// Determines whether a triangle lies entirely inside or on the boundary of a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The <see cref="Circle{T1}"/> to check.</param>
    /// <param name="triangle">The <see cref="Triangle{T1}"/> to check.</param>
    /// <returns>True if all three vertices of the triangle are inside or on the circle; otherwise, false.</returns>
    public static bool Contains<T1>(Circle<T1> circle, Triangle<T1> triangle)
        where T1 : struct, INumber<T1>
        => Contains(circle, triangle.Position[0]) &&
           Contains(circle, triangle.Position[1]) &&
           Contains(circle, triangle.Position[2]);
}