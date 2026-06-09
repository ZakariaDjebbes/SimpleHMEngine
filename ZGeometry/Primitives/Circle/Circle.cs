using ZGeometry.Primitives.Point;

namespace ZGeometry.Primitives.Circle;


/// <summary>
/// Provides factory methods for creating instances of the <see cref="Circle{T}"/> struct with various numeric types.
/// </summary>
public static class Circle
{
    /// <summary>
    /// Creates a new instance of the <see cref="Circle{T}"/> struct with <see cref="float"/> as the numeric type.
    /// </summary>
    /// <returns>A new <see cref="Circle{T}"/> instance with default values.</returns>
    public static Circle<float> Create() => new();

    /// <summary>
    /// Creates a new instance of the <see cref="Circle{T}"/> struct with <see cref="decimal"/> as the numeric type.
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A new <see cref="Circle{T}"/> instance with the specified center and radius.</returns>
    public static Circle<decimal> Create(Vector2D<decimal> center, decimal radius) => new(center, radius);

    /// <summary>
    /// Creates a new instance of the <see cref="Circle{T}"/> struct with <see cref="double"/> as the numeric type.
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A new <see cref="Circle{T}"/> instance with the specified center and radius.</returns>
    public static Circle<double> Create(Vector2D<double> center, double radius) => new(center, radius);

    /// <summary>
    /// Creates a new instance of the <see cref="Circle{T}"/> struct with <see cref="float"/> as the numeric type.
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A new <see cref="Circle{T}"/> instance with the specified center and radius.</returns>
    public static Circle<float> Create(Vector2D<float> center, float radius) => new(center, radius);

    /// <summary>
    /// Creates a new instance of the <see cref="Circle{T}"/> struct with <see cref="long"/> as the numeric type.
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A new <see cref="Circle{T}"/> instance with the specified center and radius.</returns>
    public static Circle<long> Create(Vector2D<long> center, long radius) => new(center, radius);

    /// <summary>
    /// Creates a new instance of the <see cref="Circle{T}"/> struct with <see cref="int"/> as the numeric type.
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A new <see cref="Circle{T}"/> instance with the specified center and radius.</returns>
    public static Circle<int> Create(Vector2D<int> center, int radius) => new(center, radius);

    /// <summary>
    /// Creates a new instance of the <see cref="Circle{T}"/> struct with <see cref="byte"/> as the numeric type.
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A new <see cref="Circle{T}"/> instance with the specified center and radius.</returns>
    public static Circle<byte> Create(Vector2D<byte> center, byte radius) => new(center, radius);
}