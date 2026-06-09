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
    /// Determines whether a triangle overlaps a point.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The triangle to check.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point lies inside or on the triangle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Triangle<T1> triangle, Vector2D<T1> point)
        where T1 : struct, INumber<T1> => Overlaps(point, triangle);

    /// <summary>
    /// Determines whether a triangle overlaps a line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The triangle to check.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the line touches or enters the triangle; otherwise, false.</returns>
    public static bool Overlaps<T1>(Triangle<T1> triangle, Line<T1> line)
        where T1 : struct, INumber<T1> => Overlaps(line, triangle);

    /// <summary>
    /// Determines whether two triangles overlap.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle1">The first triangle.</param>
    /// <param name="triangle2">The second triangle.</param>
    /// <returns>True if the triangles share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T1>(Triangle<T1> triangle1, Triangle<T1> triangle2)
        where T1 : struct, INumber<T1>
    {
        foreach (var vertex in triangle2.Position)
            if (Contains(vertex, triangle1)) return true;

        foreach (var vertex in triangle1.Position)
            if (Contains(vertex, triangle2)) return true;

        return Intersects(triangle1, triangle2).Any();
    }

    /// <summary>
    /// Determines whether a triangle overlaps a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The triangle to check.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>True if the triangle and rectangle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T1>(Triangle<T1> triangle, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1>
    {
        foreach (var vertex in triangle.Position)
            if (Contains(rectangle, vertex)) return true;

        var corners = new[]
        {
            rectangle.Position,
            rectangle.Position + Vector2D<T1>.Create(rectangle.Size.X, T1.Zero),
            rectangle.Position + rectangle.Size,
            rectangle.Position + Vector2D<T1>.Create(T1.Zero, rectangle.Size.Y)
        };

        foreach (var corner in corners)
            if (Contains(corner, triangle)) return true;

        return Intersects(triangle, rectangle).Any();
    }

    /// <summary>
    /// Determines whether a triangle overlaps a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The triangle to check.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>True if the triangle and circle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T1>(Triangle<T1> triangle, Circle<T1> circle)
        where T1 : struct, INumber<T1>
        => Contains(circle.Center, triangle) || Intersects(triangle, circle).Any();
}
