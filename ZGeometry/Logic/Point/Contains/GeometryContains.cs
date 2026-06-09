using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Triangle;
using ZGeometry.Utils;

namespace ZGeometry.Logic;

/// <summary>
/// Provides static methods for geometric calculations and operations.
/// </summary>
public static partial class Geometry
{
    /// <summary>
    /// Determines whether two points are approximately equal based on their squared distance.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="point1">The first <see cref="Vector2D{T}"/>.</param>
    /// <param name="point2">The second <see cref="Vector2D{T}"/>.</param>
    /// <returns>True if the distance between the points is less than <see cref="Constants.Epsilon"/>; otherwise, false.</returns>
    public static bool Contains<T>(Vector2D<T> point1, Vector2D<T> point2) where T : struct, INumber<T>
        => Convert.ToDouble((point1 - point2).MagnitudeSquared()) < Constants.Epsilon;

    /// <summary>
    /// Determines whether a point lies on a given line.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="line">The <see cref="Line{T}"/> to check.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <returns>True if the point lies on the line; otherwise, false.</returns>
    public static bool Contains<T>(Vector2D<T> point, Line<T> line) where T : struct, INumber<T>
    {
        var d = (point.X - line.Start.X) * (line.End.Y - line.Start.Y) -
                (point.Y - line.Start.Y) * (line.End.X - line.Start.X);
        if (Convert.ToDouble(T.Abs(d)) >= Constants.Epsilon) return false;

        var u = line.Vector.DotProduct(point - line.Start) / line.Vector.MagnitudeSquared();
        return u >= T.Zero && u <= T.One;
    }
    
    /// <summary>
    /// Determines whether a point is inside or on the boundary of a triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="triangle">The <see cref="Triangle{T}"/> to check.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <returns>True if the point is inside or on the boundary of the triangle; otherwise, false.</returns>
    public static bool Contains<T>(Vector2D<T> point, Triangle<T> triangle) where T : struct, INumber<T>
    {
        // Ensure we work with floating-point calculations
        var t0X = Convert.ToDouble(triangle.Position[0].X);
        var t0Y = Convert.ToDouble(triangle.Position[0].Y);
        var t1X = Convert.ToDouble(triangle.Position[1].X);
        var t1Y = Convert.ToDouble(triangle.Position[1].Y);
        var t2X = Convert.ToDouble(triangle.Position[2].X);
        var t2Y = Convert.ToDouble(triangle.Position[2].Y);
        var px = Convert.ToDouble(point.X);
        var py = Convert.ToDouble(point.Y);

        // Calculate the area of the triangle
        var a = 0.5 * (-t1Y * t2X + t0Y * (-t1X + t2X) + t0X * (t1Y - t2Y) + t1X * t2Y);

        double sign = a < 0 ? -1 : 1;

        // Calculate s
        var s = (t0Y * t2X - t0X * t2Y + (t2Y - t0Y) * px + (t0X - t2X) * py) * sign;

        // Calculate v
        var v = (t0X * t1Y - t0Y * t1X + (t0Y - t1Y) * px + (t1X - t0X) * py) * sign;

        // Check if the point is inside the triangle
        return s >= 0 && v >= 0 && (s + v) <= 2 * a * sign;
    }

    /// <summary>
    /// Determines whether a point is inside or on the boundary of a circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <param name="circle">The <see cref="Circle{T}"/> to check.</param>
    /// <returns>True if the point is inside or on the boundary of the circle; otherwise, false.</returns>
    public static bool Contains<T>(Vector2D<T> point, Circle<T> circle) where T : struct, INumber<T>
    {
        var distanceSquared = (point - circle.Center).MagnitudeSquared();
        var radiusSquared = circle.Radius * circle.Radius;
        return distanceSquared <= radiusSquared;
    }
}