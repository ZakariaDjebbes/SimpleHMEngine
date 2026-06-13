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

        var lineDir = line.Vector;
        var lineLengthSquared = lineDir.MagnitudeSquared();
        if (lineLengthSquared == T1.Zero)
            return intersections; // Degenerate (zero-length) segment.

        // Foot of the perpendicular from the circle centre onto the INFINITE line. This must not be
        // clamped to the segment: the half-chord below is sqrt(r^2 - perpendicular^2), which is only
        // valid when measured from the true perpendicular foot. Clamping to an endpoint first makes the
        // computed points drift off the circle near segment ends. The on-segment filter happens after,
        // by discarding whichever candidate falls outside the segment.
        var projection = lineDir.DotProduct(circle.Center - line.Start) / lineLengthSquared;
        var foot = line.Start + lineDir * projection;

        var perpendicularSquared = (foot - circle.Center).MagnitudeSquared();
        var radiusSquared = circle.Radius * circle.Radius;
        if (perpendicularSquared > radiusSquared)
            return intersections; // The line stays farther than the radius: no crossing.

        var lineLength = T1.CreateTruncating(MathF.Sqrt(float.CreateTruncating(lineLengthSquared)));
        var halfChord = T1.CreateTruncating(MathF.Sqrt(float.CreateTruncating(radiusSquared - perpendicularSquared)));

        // Positions of the two crossings as a parameter along the segment (0 = Start, 1 = End). The
        // points are on the line by construction, so keep the ones whose parameter falls within the
        // segment directly. This deliberately avoids Contains(point, line), whose 1e-10 collinearity
        // tolerance rejects float-rounded points and would drop every real crossing. A tangent
        // (halfChord == 0) gives t1 == t2, and the HashSet collapses it to a single point.
        var step = halfChord / lineLength;
        var t1 = projection + step;
        var t2 = projection - step;

        if (t1 >= T1.Zero && t1 <= T1.One)
            intersections.Add(line.Start + lineDir * t1);
        if (t2 >= T1.Zero && t2 <= T1.One)
            intersections.Add(line.Start + lineDir * t2);

        return intersections;
    }
}