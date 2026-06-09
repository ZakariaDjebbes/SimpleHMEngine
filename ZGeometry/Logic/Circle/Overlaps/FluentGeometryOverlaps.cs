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
    /// Determines whether this circle overlaps a point.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current circle.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point lies inside or on the circle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Circle<T> circle, Vector2D<T> point) where T : struct, INumber<T>
        => Geometry.Overlaps(circle, point);

    /// <summary>
    /// Determines whether this circle overlaps a line.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current circle.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the line touches or enters the circle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Circle<T> circle, Line<T> line) where T : struct, INumber<T>
        => Geometry.Overlaps(circle, line);

    /// <summary>
    /// Determines whether this circle overlaps a triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current circle.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>True if the circle and triangle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T>(this Circle<T> circle, Triangle<T> triangle) where T : struct, INumber<T>
        => Geometry.Overlaps(circle, triangle);

    /// <summary>
    /// Determines whether this circle overlaps a rectangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle">The current circle.</param>
    /// <param name="rectangle">The rectangle to check.</param>
    /// <returns>True if the circle and rectangle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T>(this Circle<T> circle, Rectangle<T> rectangle) where T : struct, INumber<T>
        => Geometry.Overlaps(circle, rectangle);

    /// <summary>
    /// Determines whether this circle overlaps another circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="circle1">The current circle.</param>
    /// <param name="circle2">The other circle to check.</param>
    /// <returns>True if the circles share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T>(this Circle<T> circle1, Circle<T> circle2) where T : struct, INumber<T>
        => Geometry.Overlaps(circle1, circle2);
}
