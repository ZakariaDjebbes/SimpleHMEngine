using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for working with 2D points and shapes in a fluent manner.
/// </summary>
public static partial class FluentGeometryContains
{
    /// <summary>
    /// Determines whether a point is inside or on the boundary of a <see cref="Rectangle{T}"/>.
    /// </summary>
    /// <param name="rectangle">The current <see cref="Rectangle{T}"/>.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <returns>True if the point is inside or on the boundary of the rectangle; otherwise, false.</returns>
    public static bool Contains<T>(this Rectangle<T> rectangle, Vector2D<T> point) where T : struct, INumber<T>
        => Geometry.Contains(rectangle, point);   
    
    /// <summary>Determines whether this rectangle fully contains another rectangle.</summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="rect1">The current (outer) rectangle.</param>
    /// <param name="rect2">The rectangle to test for containment.</param>
    /// <returns>True if <paramref name="rect2"/> lies entirely within <paramref name="rect1"/>; otherwise, false.</returns>
    public static bool Contains<T>(this Rectangle<T> rect1, Rectangle<T> rect2) where T : struct, INumber<T>
        => Geometry.Contains(rect1, rect2);

    /// <summary>Determines whether this rectangle fully contains a circle.</summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current rectangle.</param>
    /// <param name="circle">The circle to test for containment.</param>
    /// <returns>True if the circle lies entirely within the rectangle; otherwise, false.</returns>
    public static bool Contains<T>(this Rectangle<T> rectangle, Circle<T> circle) where T : struct, INumber<T>
        => Geometry.Contains(rectangle, circle);

    /// <summary>
    /// Determines whether a line segment lies entirely inside or on the boundary of the current rectangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current <see cref="Rectangle{T}"/>.</param>
    /// <param name="line">The <see cref="Line{T}"/> to check.</param>
    /// <returns>True if both endpoints of the line are inside or on the rectangle; otherwise, false.</returns>
    public static bool Contains<T>(this Rectangle<T> rectangle, Line<T> line) where T : struct, INumber<T>
        => Geometry.Contains(rectangle, line);

    /// <summary>
    /// Determines whether a triangle lies entirely inside or on the boundary of the current rectangle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="rectangle">The current <see cref="Rectangle{T}"/>.</param>
    /// <param name="triangle">The <see cref="Triangle{T}"/> to check.</param>
    /// <returns>True if all three vertices of the triangle are inside or on the rectangle; otherwise, false.</returns>
    public static bool Contains<T>(this Rectangle<T> rectangle, Triangle<T> triangle) where T : struct, INumber<T>
        => Geometry.Contains(rectangle, triangle);
}
