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
    /// Determines whether this point coincides with another point.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="point1">The current point.</param>
    /// <param name="point2">The other point to check.</param>
    /// <returns>True if the points overlap; otherwise, false.</returns>
    public static bool Overlaps<T>(this Vector2D<T> point1, Vector2D<T> point2) where T : struct, INumber<T>
        => Geometry.Overlaps(point1, point2);

    /// <summary>
    /// Determines whether this point lies on a line segment.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="point">The current point.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the point overlaps the line; otherwise, false.</returns>
    public static bool Overlaps<T>(this Vector2D<T> point, Line<T> line) where T : struct, INumber<T>
        => Geometry.Overlaps(point, line);

    /// <summary>
    /// Determines whether this point lies inside or on the boundary of a triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="point">The current point.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>True if the point overlaps the triangle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Vector2D<T> point, Triangle<T> triangle) where T : struct, INumber<T>
        => Geometry.Overlaps(point, triangle);

    /// <summary>
    /// Determines whether this point lies inside or on the boundary of a rectangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="point">The current point.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>True if the point overlaps the rectangle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Vector2D<T> point, Rectangle<T> rectangle) where T : struct, INumber<T>
        => Geometry.Overlaps(point, rectangle);

    /// <summary>
    /// Determines whether this point lies inside or on the boundary of a circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="point">The current point.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>True if the point overlaps the circle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Vector2D<T> point, Circle<T> circle) where T : struct, INumber<T>
        => Geometry.Overlaps(point, circle);
}
