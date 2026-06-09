using ZGeometry.Primitives.Point;

namespace ZGeometry.Primitives.Triangle;


/// <summary>
/// Provides static methods for creating instances of <see cref="Triangle{T}"/> with specific types.
/// </summary>
public static class Triangle
{
    /// <summary>
    /// Creates a new instance of <see cref="Triangle{T}"/> with default values.
    /// </summary>
    /// <returns>A new <see cref="Triangle{T}"/> instance.</returns>
    public static Triangle<float> Create() => new();

    /// <summary>
    /// Creates a new instance of <see cref="Triangle{T}"/> with the given vertices.
    /// </summary>
    /// <param name="point1">The first vertex of the triangle.</param>
    /// <param name="point2">The second vertex of the triangle.</param>
    /// <param name="point3">The third vertex of the triangle.</param>
    /// <returns>A new <see cref="Triangle{T}"/> instance.</returns>
    public static Triangle<decimal> Create(Vector2D<decimal> point1, Vector2D<decimal> point2, Vector2D<decimal> point3) => new(point1, point2, point3);

    /// <summary>
    /// Creates a new instance of <see cref="Triangle{T}"/> with the given vertices.
    /// </summary>
    /// <param name="point1">The first vertex of the triangle.</param>
    /// <param name="point2">The second vertex of the triangle.</param>
    /// <param name="point3">The third vertex of the triangle.</param>
    /// <returns>A new <see cref="Triangle{T}"/> instance.</returns>
    public static Triangle<double> Create(Vector2D<double> point1, Vector2D<double> point2, Vector2D<double> point3) => new(point1, point2, point3);

    /// <summary>
    /// Creates a new instance of <see cref="Triangle{T}"/> with the given vertices.
    /// </summary>
    /// <param name="point1">The first vertex of the triangle.</param>
    /// <param name="point2">The second vertex of the triangle.</param>
    /// <param name="point3">The third vertex of the triangle.</param>
    /// <returns>A new <see cref="Triangle{T}"/> instance.</returns>
    public static Triangle<float> Create(Vector2D<float> point1, Vector2D<float> point2, Vector2D<float> point3) => new(point1, point2, point3);

    /// <summary>
    /// Creates a new instance of <see cref="Triangle{T}"/> with the given vertices.
    /// </summary>
    /// <param name="point1">The first vertex of the triangle.</param>
    /// <param name="point2">The second vertex of the triangle.</param>
    /// <param name="point3">The third vertex of the triangle.</param>
    /// <returns>A new <see cref="Triangle{T}"/> instance.</returns>
    public static Triangle<long> Create(Vector2D<long> point1, Vector2D<long> point2, Vector2D<long> point3) => new(point1, point2, point3);

    /// <summary>
    /// Creates a new instance of <see cref="Triangle{T}"/> with the given vertices.
    /// </summary>
    /// <param name="point1">The first vertex of the triangle.</param>
    /// <param name="point2">The second vertex of the triangle.</param>
    /// <param name="point3">The third vertex of the triangle.</param>
    /// <returns>A new <see cref="Triangle{T}"/> instance.</returns>
    public static Triangle<int> Create(Vector2D<int> point1, Vector2D<int> point2, Vector2D<int> point3) => new(point1, point2, point3);

    /// <summary>
    /// Creates a new instance of <see cref="Triangle{T}"/> with the given vertices.
    /// </summary>
    /// <param name="point1">The first vertex of the triangle.</param>
    /// <param name="point2">The second vertex of the triangle.</param>
    /// <param name="point3">The third vertex of the triangle.</param>
    /// <returns>A new <see cref="Triangle{T}"/> instance.</returns>
    public static Triangle<byte> Create(Vector2D<byte> point1, Vector2D<byte> point2, Vector2D<byte> point3) => new(point1, point2, point3);
}
