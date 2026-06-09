using ZGeometry.Primitives.Point;

namespace ZGeometry.Primitives.Line;


/// <summary>
/// Provides static methods for creating instances of <see cref="Line{T}"/>.
/// </summary>
public static class Line
{
    /// <summary>
    /// Creates a default instance of <see cref="Line{T}"/>.
    /// </summary>
    /// <returns>A default instance of <see cref="Line{T}"/>.</returns>
    public static Line<float> Create() => new();

    /// <summary>
    /// Creates an instance of <see cref="Line{T}"/> with specified start and end points.
    /// </summary>
    /// <param name="x">The start point of the line.</param>
    /// <param name="y">The end point of the line.</param>
    /// <returns>An instance of <see cref="Line{T}"/>.</returns>
    public static Line<decimal> Create(Vector2D<decimal> x, Vector2D<decimal> y) => new(x, y);

    /// <summary>
    /// Creates an instance of <see cref="Line{T}"/> with specified start and end points.
    /// </summary>
    /// <param name="x">The start point of the line.</param>
    /// <param name="y">The end point of the line.</param>
    /// <returns>An instance of <see cref="Line{T}"/>.</returns>
    public static Line<double> Create(Vector2D<double> x, Vector2D<double> y) => new(x, y);

    /// <summary>
    /// Creates an instance of <see cref="Line{T}"/> with specified start and end points.
    /// </summary>
    /// <param name="x">The start point of the line.</param>
    /// <param name="y">The end point of the line.</param>
    /// <returns>An instance of <see cref="Line{T}"/>.</returns>
    public static Line<float> Create(Vector2D<float> x, Vector2D<float> y) => new(x, y);

    /// <summary>
    /// Creates an instance of <see cref="Line{T}"/> with specified start and end points.
    /// </summary>
    /// <param name="x">The start point of the line.</param>
    /// <param name="y">The end point of the line.</param>
    /// <returns>An instance of <see cref="Line{T}"/>.</returns>
    public static Line<long> Create(Vector2D<long> x, Vector2D<long> y) => new(x, y);

    /// <summary>
    /// Creates an instance of <see cref="Line{T}"/> with specified start and end points.
    /// </summary>
    /// <param name="x">The start point of the line.</param>
    /// <param name="y">The end point of the line.</param>
    /// <returns>An instance of <see cref="Line{T}"/>.</returns>
    public static Line<int> Create(Vector2D<int> x, Vector2D<int> y) => new(x, y);

    /// <summary>
    /// Creates an instance of <see cref="Line{T}"/> with specified start and end points.
    /// </summary>
    /// <param name="x">The start point of the line.</param>
    /// <param name="y">The end point of the line.</param>
    /// <returns>An instance of <see cref="Line{T}"/>.</returns>
    public static Line<byte> Create(Vector2D<byte> x, Vector2D<byte> y) => new(x, y);
}