using System.Numerics;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Triangle;
using ZGeometry.Utils;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for working with 2D points and shapes in a fluent manner.
/// </summary>
public static partial class FluentGeometryContains
{
    /// <summary>
    /// Creates a clone of the current <see cref="Vector2D{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">The numeric type of the <see cref="Vector2D{T}"/> components.</typeparam>
    /// <param name="current">The current <see cref="Vector2D{T}"/> instance to clone.</param>
    /// <returns>A new <see cref="Vector2D{T}"/> instance with the same values as the current instance.</returns>
    public static Vector2D<T> Clone<T>(this Vector2D<T> current) where T : struct, INumber<T> => new(current);

    /// <summary>
    /// Determines whether the current point is approximately equal to another point based on their squared distance.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="point1">The current <see cref="Vector2D{T}"/> instance.</param>
    /// <param name="point2">The other <see cref="Vector2D{T}"/> to compare against.</param>
    /// <returns>True if the distance between the points is less than <see cref="Constants.Epsilon"/>; otherwise, false.</returns>
    public static bool Contains<T>(this Vector2D<T> point1, Vector2D<T> point2) where T : struct, INumber<T>
        => Geometry.Contains(point1, point2);

    /// <summary>
    /// Determines whether a point lies on the current line.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="line">The current <see cref="Line{T}"/>.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <returns>True if the point lies on the line; otherwise, false.</returns>
    public static bool Contains<T>(this Vector2D<T> point, Line<T> line) where T : struct, INumber<T>
        => Geometry.Contains(point, line);

    /// <summary>
    /// Determines whether a point is inside or on the boundary of a <see cref="Triangle{T}"/>.
    /// </summary>
    /// <param name="triangle">The current <see cref="Triangle{T}"/>.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <returns>True if the point is inside or on the boundary of the triangle; otherwise, false.</returns>
    public static bool Contains<T>(this Vector2D<T> point, Triangle<T> triangle) where T : struct, INumber<T>
        => Geometry.Contains(point, triangle);

    /// <summary>
    /// Determines whether a point is inside or on the boundary of a circle.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <param name="circle">The <see cref="Circle{T}"/> to check.</param>
    /// <returns>True if the point is inside or on the boundary of the circle; otherwise, false.</returns>
    public static bool Contains<T>(this Vector2D<T> point, Circle<T> circle) where T : struct, INumber<T>
        => Geometry.Contains(point, circle);
}
