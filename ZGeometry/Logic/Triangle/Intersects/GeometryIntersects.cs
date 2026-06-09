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
    /// Determines the intersection points between a triangle and a point.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The triangle to check.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>A collection containing the point if it lies on the triangle's boundary, otherwise empty.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Triangle<T1> triangle, Vector2D<T1> point)
        where T1 : struct, INumber<T1> => Intersects(point, triangle);

    /// <summary>
    /// Determines the intersection points between a triangle and a line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The triangle to check.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>A collection of intersection points between the triangle and line.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Triangle<T1> triangle, Line<T1> line)
        where T1 : struct, INumber<T1> => Intersects(line, triangle);

    /// <summary>
    /// Determines the intersection points between two triangles.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle1">The first triangle.</param>
    /// <param name="triangle2">The second triangle.</param>
    /// <returns>A collection of intersection points between the triangles.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Triangle<T1> triangle1, Triangle<T1> triangle2)
        where T1 : struct, INumber<T1>
    {
        var intersections = new List<Vector2D<T1>>();

        for (var i = 0; i < Triangle<T1>.SideCount; i++)
        {
            var v = Intersects(triangle2.Side(i), triangle1);
            intersections.AddRange(v);
        }

        return intersections.Distinct();
    }

    /// <summary>
    /// Determines the intersection points between a triangle and a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The triangle to check.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>A collection of intersection points between the triangle and rectangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Triangle<T1> triangle, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1>
    {
        var intersections = new List<Vector2D<T1>>();

        for (var i = 0; i < Triangle<T1>.SideCount; i++)
        {
            var v = Intersects(triangle.Side(i), rectangle);
            intersections.AddRange(v);
        }

        return intersections.Distinct();
    }

    /// <summary>
    /// Determines the intersection points between a triangle and a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The triangle to check.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>A collection of intersection points between the triangle and circle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Triangle<T1> triangle, Circle<T1> circle)
        where T1 : struct, INumber<T1>
    {
        var intersections = new HashSet<Vector2D<T1>>();

        // Check intersections with each side of the triangle
        for (var i = 0; i < Triangle<T1>.SideCount; i++)
        {
            var side = triangle.Side(i);
            var sideIntersections = Intersects(side, circle);
            foreach (var point in sideIntersections)
            {
                intersections.Add(point);
            }
        }

        // Check if any vertex of the triangle is inside the circle
        foreach (var vertex in triangle.Position)
        {
            if (Contains(circle, vertex))
            {
                intersections.Add(vertex);
            }
        }

        return intersections;
    }
}
