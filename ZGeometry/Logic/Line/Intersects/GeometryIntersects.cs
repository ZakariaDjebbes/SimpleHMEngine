using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

public static partial class Geometry
{
    /// <summary>Returns the intersection point of two line segments, if any.</summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line1">The first line.</param>
    /// <param name="line2">The second line.</param>
    /// <param name="infinite">When true, treats the lines as infinite rather than bounded segments.</param>
    /// <returns>A single intersection point, or an empty sequence if they do not intersect.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Line<T1> line1, Line<T1> line2, bool infinite = false)
        where T1 : struct, INumber<T1>
    {
        var rd = line1.Vector.CrossProduct(line2.Vector);
        if (rd == T1.Zero) return [];
        rd = T1.One / rd;

        // Cross products:
        var rn = ((line2.End.X - line2.Start.X) * (line1.Start.Y - line2.Start.Y) -
                  (line2.End.Y - line2.Start.Y) * (line1.Start.X - line2.Start.X)) * rd;
        var sn = ((line1.End.X - line1.Start.X) * (line1.Start.Y - line2.Start.Y) -
                  (line1.End.Y - line1.Start.Y) * (line1.Start.X - line2.Start.X)) * rd;

        if (!infinite)
        {
            if (rn < T1.Zero || rn > T1.One || sn < T1.Zero || sn > T1.One)
                return []; // Intersection not within line segments
        }  

        return [line1.Start +  rn * line1.Vector];
    }
    
    /// <summary>Returns the points where a line crosses the edges of a rectangle.</summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line">The line.</param>
    /// <param name="rectangle">The rectangle.</param>
    /// <returns>The distinct intersection points.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Line<T1> line, Rectangle<T1> rectangle)
        where T1 : struct, INumber<T1>
    {
        var intersections = new List<Vector2D<T1>>();

        for (var i = 0; i < rectangle.SideCount; i++)
        {
            var v = Intersects(line, rectangle.Side(i));
            intersections.AddRange(v);
        }

        return intersections.Distinct();
    }

    /// <summary>
    /// Determines the intersection points between a line and a triangle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line">The line to check.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>A collection of intersection points between the line and triangle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Line<T1> line, Triangle<T1> triangle)
        where T1 : struct, INumber<T1>
    {
        var intersections = new List<Vector2D<T1>>();

        for (var i = 0; i < Triangle<T1>.SideCount; i++)
        {
            var v = Intersects(line, triangle.Side(i));
            intersections.AddRange(v);
        }

        return intersections.Distinct();
    }

    /// <summary>
    /// Determines the intersection points between a line and a circle.
    /// </summary>
    /// <typeparam name="T1">The numeric type of the vector components.</typeparam>
    /// <param name="line">The line to check.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>A collection of intersection points between the line and circle.</returns>
    public static IEnumerable<Vector2D<T1>> Intersects<T1>(Line<T1> line, Circle<T1> circle)
        where T1 : struct, INumber<T1>
    {
        var intersections = new HashSet<Vector2D<T1>>();

        // Vector from line start to circle center
        var d = circle.Center - line.Start;
        
        // Project circle center onto line
        var lineDir = line.Vector;
        var lineLengthSquared = lineDir.MagnitudeSquared();
        var projection = lineDir.DotProduct(d) / lineLengthSquared;
        
        // Clamp projection to line segment
        projection = T1.Clamp(projection, T1.Zero, T1.One);
        
        // Closest point on line to circle center
        var closestPoint = line.Start + lineDir * projection;
        
        // Distance from closest point to circle center
        var distanceSquared = (closestPoint - circle.Center).MagnitudeSquared();
        var radiusSquared = circle.Radius * circle.Radius;
        
        // If distance equals radius, we have one intersection point
        if (distanceSquared == radiusSquared)
        {
            intersections.Add(closestPoint);
        }
        // If distance is less than radius, we have two intersection points
        else if (distanceSquared < radiusSquared)
        {
            // Calculate the distance from closest point to intersection points
            var distanceToIntersection = T1.CreateTruncating(MathF.Sqrt(float.CreateTruncating(radiusSquared - distanceSquared)));
            var lineDirNormalized = lineDir / T1.CreateTruncating(MathF.Sqrt(float.CreateTruncating(lineLengthSquared)));
            
            // Calculate both intersection points
            var intersection1 = closestPoint + lineDirNormalized * distanceToIntersection;
            var intersection2 = closestPoint - lineDirNormalized * distanceToIntersection;
            
            // Only add points that are on the line segment
            if (Contains(intersection1, line))
            {
                intersections.Add(intersection1);
            }
            if (Contains(intersection2, line))
            {
                intersections.Add(intersection2);
            }
        }
        
        return intersections;
    }
}