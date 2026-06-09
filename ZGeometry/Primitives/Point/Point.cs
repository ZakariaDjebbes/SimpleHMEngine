using System.Numerics;

namespace ZGeometry.Primitives.Point;

/// <summary>
/// Provides factory methods for creating instances of the <see cref="Point{T}"/> struct with various generic types.
/// </summary>
public static class Point<T> where T : struct, INumber<T>
{
    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/>.
    /// </summary>
    /// <param name="x">The X position of the vector.</param>
    /// <param name="y">The Y position of the vector.</param>
    /// <returns>A <see cref="Vector2D{T}"/> with the given coordinates.</returns>
    public static Vector2D<T> Create(T x, T y) => new(x, y);
    
    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/>.
    /// </summary>
    /// <returns>A <see cref="Vector2D{T}"/> with the (0, 0) coordinates.</returns>
    public static Vector2D<T> Create() => new();
}

/// <summary>
/// Provides factory methods for creating instances of the <see cref="Point{T}"/> struct with various numeric types.
/// </summary>
public static class Point
{
    /// <summary>
    /// Creates a default <see cref="Vector2D{T}"/> with both components set to 0.
    /// </summary>
    /// <returns>A default <see cref="Vector2D{T}"/> instance.</returns>
    public static Vector2D<float> Create() => new();

    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/> with the specified x and y coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the point.</param>
    /// <param name="y">The y-coordinate of the point.</param>
    /// <returns>A <see cref="Vector2D{T}"/> instance.</returns>
    public static Vector2D<decimal> Create(decimal x, decimal y) => new(x, y);

    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/> with the specified x and y coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the point.</param>
    /// <param name="y">The y-coordinate of the point.</param>
    /// <returns>A <see cref="Vector2D{T}"/> instance.</returns>
    public static Vector2D<double> Create(double x, double y) => new(x, y);

    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/> with the specified x and y coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the point.</param>
    /// <param name="y">The y-coordinate of the point.</param>
    /// <returns>A <see cref="Vector2D{T}"/> instance.</returns>
    public static Vector2D<float> Create(float x, float y) => new(x, y);

    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/> with the specified x and y coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the point.</param>
    /// <param name="y">The y-coordinate of the point.</param>
    /// <returns>A <see cref="Vector2D{T}"/> instance.</returns>
    public static Vector2D<long> Create(long x, long y) => new(x, y);

    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/> with the specified x and y coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the point.</param>
    /// <param name="y">The y-coordinate of the point.</param>
    /// <returns>A <see cref="Vector2D{T}"/> instance.</returns>
    public static Vector2D<int> Create(int x, int y) => new(x, y);

    /// <summary>
    /// Creates a <see cref="Vector2D{T}"/> with the specified x and y coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the point.</param>
    /// <param name="y">The y-coordinate of the point.</param>
    /// <returns>A <see cref="Vector2D{T}"/> instance.</returns>
    public static Vector2D<byte> Create(byte x, byte y) => new(x, y);

    /// <summary>
    /// Clones a <see cref="Vector2D{T}"/> instance.
    /// </summary>
    /// <param name="vector2D">The vector to clone.</param>
    /// <returns>A new <see cref="Vector2D{T}"/> instance with the same values as <paramref name="vector2D"/>.</returns>
    public static Vector2D<decimal> Clone(Vector2D<decimal> vector2D) => new(vector2D);

    /// <summary>
    /// Clones a <see cref="Vector2D{T}"/> instance.
    /// </summary>
    /// <param name="vector2D">The vector to clone.</param>
    /// <returns>A new <see cref="Vector2D{T}"/> instance with the same values as <paramref name="vector2D"/>.</returns>
    public static Vector2D<double> Clone(Vector2D<double> vector2D) => new(vector2D);

    /// <summary>
    /// Clones a <see cref="Vector2D{T}"/> instance.
    /// </summary>
    /// <param name="vector2D">The vector to clone.</param>
    /// <returns>A new <see cref="Vector2D{T}"/> instance with the same values as <paramref name="vector2D"/>.</returns>
    public static Vector2D<float> Clone(Vector2D<float> vector2D) => new(vector2D);

    /// <summary>
    /// Clones a <see cref="Vector2D{T}"/> instance.
    /// </summary>
    /// <param name="vector2D">The vector to clone.</param>
    /// <returns>A new <see cref="Vector2D{T}"/> instance with the same values as <paramref name="vector2D"/>.</returns>
    public static Vector2D<long> Clone(Vector2D<long> vector2D) => new(vector2D);

    /// <summary>
    /// Clones a <see cref="Vector2D{T}"/> instance.
    /// </summary>
    /// <param name="vector2D">The vector to clone.</param>
    /// <returns>A new <see cref="Vector2D{T}"/> instance with the same values as <paramref name="vector2D"/>.</returns>
    public static Vector2D<int> Clone(Vector2D<int> vector2D) => new(vector2D);

    /// <summary>
    /// Clones a <see cref="Vector2D{T}"/> instance.
    /// </summary>
    /// <param name="vector2D">The vector to clone.</param>
    /// <returns>A new <see cref="Vector2D{T}"/> instance with the same values as <paramref name="vector2D"/>.</returns>
    public static Vector2D<byte> Clone(Vector2D<byte> vector2D) => new(vector2D);
}