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
    /// Determines the intersection points between a circle and a point.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The circle to check.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>A collection containing the point if it lies on the circle's boundary, otherwise empty.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Circle<T1> circle, Vector2D<T1> point)
        where T1 : struct, INumber<T1> => Intersects(point, circle);

    /// <summary>
    /// Determines the intersection points between two circles.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle1">The first circle.</param>
    /// <param name="circle2">The second circle.</param>
    /// <returns>A collection of intersection points between the circles.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Circle<T1> circle1, Circle<T1> circle2)
        where T1 : struct, INumber<T1>
    {
        var intersections = new HashSet<Vector2D<T1>>();
        
        // Calculate distance between centers
        var d = (circle1.Center - circle2.Center).Magnitude();
        var r1 = circle1.Radius;
        var r2 = circle2.Radius;
        
        // Check if circles are separate or one contains the other
        if (d > r1 + r2 || d < T1.Abs(r1 - r2)) return intersections;
        
        // Check if circles are coincident
        if (d == T1.Zero && r1 == r2) return intersections;
        
        // Calculate intersection points
        var a = (r1 * r1 - r2 * r2 + d * d) / (T1.CreateChecked(2) * d);
        var h = T1.CreateTruncating(MathF.Sqrt(float.CreateTruncating(r1 * r1 - a * a)));
        
        var p2 = circle1.Center + a * (circle2.Center - circle1.Center) / d;
        
        var x3 = p2.X + h * (circle2.Center.Y - circle1.Center.Y) / d;
        var y3 = p2.Y - h * (circle2.Center.X - circle1.Center.X) / d;
        var x4 = p2.X - h * (circle2.Center.Y - circle1.Center.Y) / d;
        var y4 = p2.Y + h * (circle2.Center.X - circle1.Center.X) / d;
        
        intersections.Add(Vector2D<T1>.Create(x3, y3));
        if (x3 != x4 || y3 != y4)
        {
            intersections.Add(Vector2D<T1>.Create(x4, y4));
        }
        
        return intersections;
    }

    /// <summary>
    /// Determines the intersection points between a circle and a line.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The circle to check.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>A collection of intersection points between the circle and line.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Circle<T1> circle, Line<T1> line)
        where T1 : struct, INumber<T1> => Intersects(line, circle);

    /// <summary>
    /// Determines the intersection points between a circle and a rectangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The circle to check.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>A collection of intersection points between the circle and rectangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Circle<T1> circle, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1> => Intersects(rectangle, circle);

    /// <summary>
    /// Determines the intersection points between a circle and a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The circle to check.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>A collection of intersection points between the circle and triangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Circle<T1> circle, Triangle<T1> triangle)
        where T1 : struct, INumber<T1> => Intersects(triangle, circle);
}