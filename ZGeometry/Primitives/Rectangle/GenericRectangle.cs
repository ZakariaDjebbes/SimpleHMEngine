using System.Numerics;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;

namespace ZGeometry.Primitives.Rectangle;

/// <summary>
/// Represents a rectangle defined by a position and size, where the components are of a generic numeric type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">A numeric type that implements the <see cref="INumber{T}"/> interface.</typeparam>
public struct Rectangle<T>(Vector2D<T> position = default, Vector2D<T> size = default)
    where T : struct, INumber<T>
{
    /// <summary>
    /// Gets or sets the position of the rectangle, defined as a <see cref="Vector2D{T}"/>.
    /// </summary>
    public Vector2D<T> Position { get; set; } = position;

    /// <summary>
    /// Gets or sets the size of the rectangle, defined as a <see cref="Vector2D{T}"/>.
    /// </summary>
    public Vector2D<T> Size { get; set; } = size;

    /// <summary>
    /// The number of sides in the rectangle, which is always 4.
    /// </summary>
    public int SideCount = 4;

    /// <summary>
    /// Creates a new instance of the <see cref="Rectangle{T}"/> struct with <see cref="T"/> as the numeric type.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <returns>A new <see cref="Rectangle{T}"/> instance with default values.</returns>
    public static Rectangle<T> Create(Vector2D<T> position, Vector2D<T> size) => new(position, size);

    /// <summary>
    /// Creates a new instance of the <see cref="Rectangle{T}"/> struct with <see cref="T"/> as the numeric type.
    /// </summary>
    /// <returns>A new <see cref="Rectangle{T}"/> instance with default values.</returns>
    public static Rectangle<T> Create() => new();
    
    /// <summary>
    /// Gets the middle point of the rectangle
    /// </summary>
    public Vector2D<double> Middle => new(
        Convert.ToDouble(Position.X) + Convert.ToDouble(Size.X) * 0.5,
        Convert.ToDouble(Position.Y) + Convert.ToDouble(Size.Y) * 0.5
    );

    /// <summary>
    /// Gets the top side of the rectangle as a <see cref="Line{T}"/> from the left to the right.
    /// </summary>
    public Line<T> Top => new(Position, new Vector2D<T>(Position.X + Size.X, Position.Y));

    /// <summary>
    /// Gets the bottom side of the rectangle as a <see cref="Line{T}"/> from the left to the right.
    /// </summary>
    public Line<T> Bottom => new(new Vector2D<T>(Position.X, Position.Y + Size.Y), Position + Size);

    /// <summary>
    /// Gets the left side of the rectangle as a <see cref="Line{T}"/> from the top to the bottom.
    /// </summary>
    public Line<T> Left => new(Position, new Vector2D<T>(Position.X, Position.Y + Size.Y));

    /// <summary>
    /// Gets the right side of the rectangle as a <see cref="Line{T}"/> from the top to the bottom.
    /// </summary>
    public Line<T> Right => new(new Vector2D<T>(Position.X + Size.X, Position.Y), Position + Size);

    /// <summary>
    /// Gets the area of the rectangle, calculated as the product of its width and height.
    /// </summary>
    public T Area => Size.X * Size.Y;

    /// <summary>
    /// Gets the perimeter of the rectangle, calculated as 2 times the sum of its width and height.
    /// </summary>
    public T Perimeter => T.CreateChecked(2) * (Size.X + Size.Y);

    /// <summary>
    /// Gets one of the four sides of the rectangle based on the index.
    /// This uses a bitwise calculation.
    /// </summary>
    /// <param name="index">The index of the side (0 for top, 1 for right, 2 for bottom, 3 for left, ...ect).</param>
    /// <returns>The corresponding side as a <see cref="Line{T}"/>.</returns>
    public Line<T> Side(int index) =>
        (index & 0b11) switch
        {
            0 => Top,
            1 => Right,
            2 => Bottom,
            3 => Left,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
}