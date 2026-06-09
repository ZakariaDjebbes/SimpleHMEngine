using System.Numerics;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;

namespace ZGeometry.Primitives.Triangle;

/// <summary>
/// Represents a triangle with generic numeric type <typeparamref name="T"/> for coordinates.
/// </summary>
/// <typeparam name="T">The numeric type for the coordinates, constrained to types that implement <see cref="INumber{T}"/>.</typeparam>
public readonly struct Triangle<T> where T : struct, INumber<T>
{
    /// <summary>
    /// Gets the position of the three vertices of the triangle.
    /// </summary>
    public Vector2D<T>[] Position { get; } = new Vector2D<T>[3];

    /// <summary>
    /// The number of sides in a triangle, which is always 3.
    /// </summary>
    public const int SideCount = 3;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Triangle{T}"/> struct with the given vertices.
    /// </summary>
    /// <param name="point1">The first vertex of the triangle.</param>
    /// <param name="point2">The second vertex of the triangle.</param>
    /// <param name="point3">The third vertex of the triangle.</param>
    public Triangle(Vector2D<T> point1, Vector2D<T> point2, Vector2D<T> point3)
    {
        Position[0] = point1;
        Position[1] = point2;
        Position[2] = point3;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Triangle{T}"/> with the given vertices.
    /// </summary>
    /// <param name="point1">The first vertex of the triangle.</param>
    /// <param name="point2">The second vertex of the triangle.</param>
    /// <param name="point3">The third vertex of the triangle.</param>
    /// <returns>A new <see cref="Triangle{T}"/> instance.</returns>
    public static Triangle<T> Create(Vector2D<T> point1, Vector2D<T> point2, Vector2D<T> point3) => new(point1, point2, point3);

    /// <summary>
    /// Creates a new instance of <see cref="Triangle{T}"/> with default values.
    /// </summary>
    /// <returns>A new <see cref="Triangle{T}"/> instance with default values.</returns>
    public static Triangle<T> Create() => new();
    
    /// <summary>
    /// Sets a point at the specified index (0, 1, or 2) to a new value.
    /// </summary>
    /// <param name="index">The index of the vertex to set (0, 1, or 2).</param>
    /// <param name="point">The new value for the vertex.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown if the index is not 0, 1, or 2.</exception>
    public void SetPoint(int index, Vector2D<T> point)
    {
        if (index is > 2 or < 0)
            throw new IndexOutOfRangeException($"A triangle has three points, cannot set point at position {index}.");

        Position[index] = point;
    }

    /// <summary>
    /// Gets a line representing the side of the triangle at the specified index.
    /// The sides are indexed in clockwise order starting from the first vertex.
    /// </summary>
    /// <param name="index">The index of the side (0, 1, or 2).</param>
    /// <returns>A <see cref="Line{T}"/> representing the side of the triangle.</returns>
    public Line<T> Side(int index) => new(Position[index % 3], Position[(index + 1) % 3]);

    /// <summary>
    /// Calculates the area of the triangle using the determinant formula.
    /// </summary>
    /// <returns>The area of the triangle as a <see cref="double"/>.</returns>
    public double Area() => 0.5 * Convert.ToDouble(T.Abs(Position[0].X * (Position[1].Y - Position[2].Y) +
                                                         Position[1].X * (Position[2].Y - Position[0].Y) +
                                                         Position[2].X * (Position[0].Y - Position[1].Y)));

    /// <summary>
    /// Calculates the perimeter of the triangle, which is the sum of the lengths of its sides.
    /// </summary>
    /// <returns>The perimeter of the triangle as a value of type <typeparamref name="T"/>.</returns>
    public T Perimeter() => new Line<T>(Position[0], Position[1]).Length
                            + new Line<T>(Position[1], Position[2]).Length
                            + new Line<T>(Position[2], Position[0]).Length;
    
    public static implicit operator Triangle<T>((Vector2D<T>, Vector2D<T>, Vector2D<T>) values) => new(values.Item1, values.Item2, values.Item3);
}