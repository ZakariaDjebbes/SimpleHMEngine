using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for testing whether shapes overlap in a fluent manner.
/// </summary>
public static partial class FluentGeometryOverlaps
{
    /// <summary>
    /// Determines whether this line overlaps a point.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="line">The current line.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point lies on the line; otherwise, false.</returns>
    public static bool Overlaps<T>(this Line<T> line, Vector2D<T> point) where T : struct, INumber<T>
        => Geometry.Overlaps(line, point);

    /// <summary>
    /// Determines whether this line overlaps another line.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="line1">The current line.</param>
    /// <param name="line2">The other line to check.</param>
    /// <returns>True if the segments intersect; otherwise, false.</returns>
    public static bool Overlaps<T>(this Line<T> line1, Line<T> line2) where T : struct, INumber<T>
        => Geometry.Overlaps(line1, line2);

    /// <summary>
    /// Determines whether this line overlaps a triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="line">The current line.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>True if the line touches or enters the triangle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Line<T> line, Triangle<T> triangle) where T : struct, INumber<T>
        => Geometry.Overlaps(line, triangle);

    /// <summary>
    /// Determines whether this line overlaps a rectangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="line">The current line.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>True if the line touches or enters the rectangle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Line<T> line, Rectangle<T> rectangle) where T : struct, INumber<T>
        => Geometry.Overlaps(line, rectangle);

    /// <summary>
    /// Determines whether this line overlaps a circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="line">The current line.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>True if the line touches or enters the circle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Line<T> line, Circle<T> circle) where T : struct, INumber<T>
        => Geometry.Overlaps(line, circle);
}
