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
    /// Determines whether this rectangle overlaps a point.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current rectangle.</param>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point lies inside or on the rectangle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Rectangle<T> rectangle, Vector2D<T> point) where T : struct, INumber<T>
        => Geometry.Overlaps(rectangle, point);

    /// <summary>
    /// Determines whether this rectangle overlaps a line.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current rectangle.</param>
    /// <param name="line">The line to check.</param>
    /// <returns>True if the line touches or enters the rectangle; otherwise, false.</returns>
    public static bool Overlaps<T>(this Rectangle<T> rectangle, Line<T> line) where T : struct, INumber<T>
        => Geometry.Overlaps(rectangle, line);

    /// <summary>
    /// Determines whether this rectangle overlaps a triangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current rectangle.</param>
    /// <param name="triangle">The triangle to check.</param>
    /// <returns>True if the rectangle and triangle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T>(this Rectangle<T> rectangle, Triangle<T> triangle) where T : struct, INumber<T>
        => Geometry.Overlaps(rectangle, triangle);

    /// <summary>
    /// Determines whether this rectangle overlaps another rectangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle1">The current rectangle.</param>
    /// <param name="rectangle2">The other rectangle to check.</param>
    /// <returns>True if the rectangles share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T>(this Rectangle<T> rectangle1, Rectangle<T> rectangle2) where T : struct, INumber<T>
        => Geometry.Overlaps(rectangle1, rectangle2);

    /// <summary>
    /// Determines whether this rectangle overlaps a circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current rectangle.</param>
    /// <param name="circle">The circle to check.</param>
    /// <returns>True if the rectangle and circle share any area or boundary; otherwise, false.</returns>
    public static bool Overlaps<T>(this Rectangle<T> rectangle, Circle<T> circle) where T : struct, INumber<T>
        => Geometry.Overlaps(rectangle, circle);
}
