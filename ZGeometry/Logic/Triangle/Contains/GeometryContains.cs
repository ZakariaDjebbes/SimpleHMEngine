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
    /// Determines whether a point is inside or on the boundary of a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The <see cref="Triangle{T1}"/> to check.</param>
    /// <param name="point">The <see cref="Vector2D{T1}"/> to check.</param>
    /// <returns>True if the point is inside or on the boundary of the triangle; otherwise, false.</returns>
    public static bool Contains<T1>(Triangle<T1> triangle, Vector2D<T1> point) where T1 : struct, INumber<T1>
        => Contains(point, triangle);

    /// <summary>
    /// Determines whether a line segment lies entirely inside or on the boundary of a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The <see cref="Triangle{T1}"/> to check.</param>
    /// <param name="line">The <see cref="Line{T1}"/> to check.</param>
    /// <returns>True if both endpoints of the line are inside or on the triangle; otherwise, false.</returns>
    public static bool Contains<T1>(Triangle<T1> triangle, Line<T1> line) where T1 : struct, INumber<T1>
        => Contains(line.Start, triangle) && Contains(line.End, triangle);

    /// <summary>
    /// Determines whether a triangle lies entirely inside or on the boundary of another triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="outer">The containing <see cref="Triangle{T1}"/>.</param>
    /// <param name="inner">The <see cref="Triangle{T1}"/> to check.</param>
    /// <returns>True if all three vertices of the inner triangle are inside or on the outer triangle; otherwise, false.</returns>
    public static bool Contains<T1>(Triangle<T1> outer, Triangle<T1> inner) where T1 : struct, INumber<T1>
        => Contains(inner.Position[0], outer) &&
           Contains(inner.Position[1], outer) &&
           Contains(inner.Position[2], outer);

    /// <summary>
    /// Determines whether a rectangle lies entirely inside or on the boundary of a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The <see cref="Triangle{T1}"/> to check.</param>
    /// <param name="rect">The <see cref="Rectangle{T1}"/> to check.</param>
    /// <returns>True if all four corners of the rectangle are inside or on the triangle; otherwise, false.</returns>
    public static bool Contains<T1>(Triangle<T1> triangle, Rectangle<T1> rect) where T1 : struct, INumber<T1>
        => Contains(rect.Position, triangle) &&
           Contains(Vector2D<T1>.Create(rect.Position.X + rect.Size.X, rect.Position.Y), triangle) &&
           Contains(rect.Position + rect.Size, triangle) &&
           Contains(Vector2D<T1>.Create(rect.Position.X, rect.Position.Y + rect.Size.Y), triangle);

    /// <summary>
    /// Determines whether a circle lies entirely inside or on the boundary of a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The <see cref="Triangle{T1}"/> to check.</param>
    /// <param name="circle">The <see cref="Circle{T1}"/> to check.</param>
    /// <returns>True if the circle's center is inside the triangle and every edge is at least the radius away; otherwise, false.</returns>
    public static bool Contains<T1>(Triangle<T1> triangle, Circle<T1> circle) where T1 : struct, INumber<T1>
    {
        if (!Contains(circle.Center, triangle)) return false;

        for (var i = 0; i < Triangle<T1>.SideCount; i++)
        {
            var side = triangle.Side(i);
            var distance = T1.Abs(side.Vector.CrossProduct(circle.Center - side.Start)) / side.Vector.Magnitude();
            if (distance < circle.Radius) return false;
        }

        return true;
    }
}
