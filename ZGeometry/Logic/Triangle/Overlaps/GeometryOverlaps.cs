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
    {
        // The circle overlaps the triangle if its centre is inside the triangle (covers "circle inside
        // triangle"), or any edge comes within the radius of the centre. The edge-distance test also
        // covers a vertex poking into the circle and the triangle sitting entirely inside the circle,
        // so this no longer depends on the intersection-point set.
        if (Contains(circle.Center, triangle))
            return true;

        var radiusSquared = circle.Radius * circle.Radius;
        for (var i = 0; i < Triangle<T1>.SideCount; i++)
            if (SegmentDistanceSquared(triangle.Side(i), circle.Center) <= radiusSquared)
                return true;

        return false;
    }

    // Squared distance from a point to a line segment (projection clamped to the segment).
    private static T1 SegmentDistanceSquared<T1>(Line<T1> segment, Vector2D<T1> point)
        where T1 : struct, INumber<T1>
    {
        var direction = segment.Vector;
        var lengthSquared = direction.MagnitudeSquared();
        if (lengthSquared == T1.Zero)
            return (point - segment.Start).MagnitudeSquared();

        var t = T1.Clamp(direction.DotProduct(point - segment.Start) / lengthSquared, T1.Zero, T1.One);
        var closest = segment.Start + direction * t;
        return (point - closest).MagnitudeSquared();
    }
}
