using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

public static partial class Geometry
{
    /// <summary>Returns the points where a rectangle's edges cross a line.</summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The rectangle.</param>
    /// <param name="line">The line.</param>
    /// <returns>The distinct intersection points.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Rectangle<T1> rectangle, Line<T1> line)
        where T1 : struct, INumber<T1>
        => Intersects(line, rectangle);

    /// <summary>
    /// Determines the intersection points between a rectangle and a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>A collection of intersection points between the rectangle and triangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Rectangle<T1> rectangle, Triangle<T1> triangle)
        where T1 : struct, INumber<T1>
        => Intersects(triangle, rectangle);
    
    /// <summary>Returns the points where the edges of two rectangles cross.</summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle1">The first rectangle.</param>
    /// <param name="rectangle2">The second rectangle.</param>
    /// <returns>The distinct intersection points.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Rectangle<T1> rectangle1, Rectangle<T1> rectangle2)
        where T1 : struct, INumber<T1>
    {
        var intersections = new List<Vector2D<T1>>();

        for (var i = 0; i < rectangle2.SideCount; i++)
        {
            var v = Intersects(rectangle2.Side(i), rectangle1);
            intersections.AddRange(v);
        }

        return intersections.Distinct();
    }

    /// <summary>
    /// Determines the intersection points between a rectangle and a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>A collection of intersection points between the rectangle and circle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Rectangle<T1> rectangle, Circle<T1> circle)
        where T1 : struct, INumber<T1>
    {
        var intersections = new HashSet<Vector2D<T1>>();

        // Check intersections with each side of the rectangle
        for (var i = 0; i < rectangle.SideCount; i++)
        {
            var side = rectangle.Side(i);
            var sideIntersections = Intersects(side, circle);
            foreach (var point in sideIntersections)
            {
                intersections.Add(point);
            }
        }

        // Check if any corner of the rectangle is inside the circle
        var corners = new[]
        {
            rectangle.Position,
            rectangle.Position + Vector2D<T1>.Create(rectangle.Size.X, T1.Zero),
            rectangle.Position + rectangle.Size,
            rectangle.Position + Vector2D<T1>.Create(T1.Zero, rectangle.Size.Y)
        };

        foreach (var corner in corners)
        {
            if (circle.Contains(corner))
            {
                intersections.Add(corner);
            }
        }

        return intersections;
    }
}