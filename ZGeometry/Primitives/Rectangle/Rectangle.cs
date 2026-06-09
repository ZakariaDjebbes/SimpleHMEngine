using ZGeometry.Primitives.Point;

namespace ZGeometry.Primitives.Rectangle;

/// <summary>
/// Provides factory methods for creating instances of the <see cref="Rectangle{T}"/> struct with various numeric types.
/// </summary>
public static class Rectangle
{
    /// <summary>
    /// Creates a new instance of the <see cref="Rectangle{T}"/> struct with <see cref="float"/> as the numeric type.
    /// </summary>
    /// <returns>A new <see cref="Rectangle{T}"/> instance with default values.</returns>
    public static Rectangle<float> Create() => new();

    /// <summary>
    /// Creates a new instance of the <see cref="Rectangle{T}"/> struct with <see cref="decimal"/> as the numeric type.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <returns>A new <see cref="Rectangle{T}"/> instance with the specified position and size.</returns>
    public static Rectangle<decimal> Create(Vector2D<decimal> position, Vector2D<decimal> size) => new(position, size);

    /// <summary>
    /// Creates a new instance of the <see cref="Rectangle{T}"/> struct with <see cref="double"/> as the numeric type.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <returns>A new <see cref="Rectangle{T}"/> instance with the specified position and size.</returns>
    public static Rectangle<double> Create(Vector2D<double> position, Vector2D<double> size) => new(position, size);

    /// <summary>
    /// Creates a new instance of the <see cref="Rectangle{T}"/> struct with <see cref="float"/> as the numeric type.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <returns>A new <see cref="Rectangle{T}"/> instance with the specified position and size.</returns>
    public static Rectangle<float> Create(Vector2D<float> position, Vector2D<float> size) => new(position, size);

    /// <summary>
    /// Creates a new instance of the <see cref="Rectangle{T}"/> struct with <see cref="long"/> as the numeric type.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <returns>A new <see cref="Rectangle{T}"/> instance with the specified position and size.</returns>
    public static Rectangle<long> Create(Vector2D<long> position, Vector2D<long> size) => new(position, size);

    /// <summary>
    /// Creates a new instance of the <see cref="Rectangle{T}"/> struct with <see cref="int"/> as the numeric type.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <returns>A new <see cref="Rectangle{T}"/> instance with the specified position and size.</returns>
    public static Rectangle<int> Create(Vector2D<int> position, Vector2D<int> size) => new(position, size);

    /// <summary>
    /// Creates a new instance of the <see cref="Rectangle{T}"/> struct with <see cref="byte"/> as the numeric type.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <returns>A new <see cref="Rectangle{T}"/> instance with the specified position and size.</returns>
    public static Rectangle<byte> Create(Vector2D<byte> position, Vector2D<byte> size) => new(position, size);
}