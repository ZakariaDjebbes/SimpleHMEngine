using System.Numerics;
using ZGeometry.Primitives.Point;

namespace ZGeometry.Primitives.Circle;

/// <summary>
/// Represents a 2D circle with a center and a radius.
/// </summary>
/// <typeparam name="T">The numeric type used for the radius and center coordinates. Must be a struct that implements <see cref="INumber{T}"/>.</typeparam>
/// <param name="center">The center of the circle.</param>
/// <param name="radius">The radius of the circle.</param>
public struct Circle<T>(Vector2D<T> center = default, T radius = default)
    where T : struct, INumber<T>, IComparisonOperators<T, T, bool>
{
    /// <summary>
    /// Gets or sets the center of the circle.
    /// </summary>
    public Vector2D<T> Center { get; set; } = center;

    /// <summary>
    /// Gets or sets the radius of the circle.
    /// </summary>
    public T Radius { get; set; } = radius;

    public T Diameter { get; set; } = radius * T.CreateChecked(2);
    
    /// <summary>
    /// Creates a new instance of the <see cref="Circle{T}"/> struct with <see cref="T"/> as the numeric type.
    /// </summary>
    /// <param name="center">The position of the rectangle.</param>
    /// <param name="radius">The size of the rectangle.</param>
    /// <returns>A new <see cref="Circle{T}"/> instance with default values.</returns>
    public static Circle<T> Create(Vector2D<T> center, T radius) => new(center, radius);

    /// <summary>
    /// Creates a new instance of the <see cref="Circle{T}"/> struct with <see cref="T"/> as the numeric type.
    /// </summary>
    /// <returns>A new <see cref="Circle{T}"/> instance with default values.</returns>
    public static Circle<T> Create() => new();
    
    /// <summary>
    /// Calculates the area of the circle.
    /// </summary>
    public T Area => T.CreateChecked(Math.PI) * Radius * Radius;

    /// <summary>
    /// Calculates the perimeter of the circle.
    /// </summary>
    public T Perimeter => T.CreateChecked(2 * Math.PI) * Radius;

    /// <summary>
    /// Gets the circumference of the circle, which is the same as the perimeter.
    /// </summary>
    public T Circumference => Perimeter;
}
