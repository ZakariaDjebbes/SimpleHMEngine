using System.Numerics;
using ZGeometry.Primitives.Point;
using ZGeometry.Utils;

namespace ZGeometry.Primitives.Line;

/// <summary>
/// Represents a line segment in a 2D space with generic numeric type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The numeric type used for coordinates, which must be a struct and implement <see cref="INumber{T}"/>.</typeparam>
///  <param name="start">The start point of the line.</param>
/// <param name="end">The end point of the line.</param>
public struct Line<T>(Vector2D<T> start = default, Vector2D<T> end = default)
    where T : struct, INumber<T>
{
    /// <summary>
    /// Gets or sets the start point of the line.
    /// </summary>
    public Vector2D<T> Start { get; set; } = start;

    /// <summary>
    /// Gets or sets the end point of the line.
    /// </summary>
    public Vector2D<T> End { get; set; } = end;

    /// <summary>
    /// Creates an instance of <see cref="Line{T}"/> with specified start and end points.
    /// </summary>
    /// <param name="x">The start point of the line.</param>
    /// <param name="y">The end point of the line.</param>
    /// <returns>An instance of <see cref="Line{T}"/>.</returns>
    public static Line<T> Create(Vector2D<T> x, Vector2D<T> y) => new(x, y);

    /// <summary>
    /// Creates a new instance of the <see cref="Line{T}"/> struct with <typeparamref name="T"/> as the numeric type.
    /// </summary>
    /// <returns>A new <see cref="Line{T}"/> instance with default values.</returns>
    public static Line<T> Create() => new();
    
    /// <summary>
    /// Gets the vector pointing from the start to the end of the line.
    /// </summary>
    /// <returns>A <see cref="Vector2D{T}"/> representing the vector from the start to the end.</returns>
    public Vector2D<T> Vector => End - Start;

    /// <summary>
    /// Calculates the length of the line segment.
    /// </summary>
    /// <returns>The length of the line segment as type <typeparamref name="T"/>.</returns>
    public T Length => T.CreateChecked(Vector.Magnitude());

    /// <summary>
    /// Calculates the squared length of the line segment.
    /// </summary>
    /// <returns>The squared length of the line segment as type <typeparamref name="T"/>.</returns>
    public T LengthSquared => Vector.MagnitudeSquared();

    /// <summary>
    /// Calculates a point along the line at a given distance from the start point.
    /// </summary>
    /// <param name="distance">The distance from the start point along the line.</param>
    /// <returns>The point along the line at the specified distance.</returns>
    public Vector2D<T> RPoint(T distance) => Start + Vector.Normalize() * distance;

    /// <summary>
    /// Calculates a point along the line at a given unit distance from the start point.
    /// </summary>
    /// <param name="distance">The unit distance from the start point along the line.</param>
    /// <returns>The point along the line at the specified unit distance.</returns>
    public Vector2D<T> UPoint(T distance) => Start + Vector * distance;

    /// <summary>
    /// Determines which side of the line a given point lies on.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>-1 if the point is on the left side, 1 if on the right side, and 0 if on the line.</returns>
    public int Side(Vector2D<T> point)
    {
        var d = Vector.CrossProduct(point - Start);
        if (d < T.Zero) return -1;
        if (d > T.Zero) return 1;
        return 0;
    }

    /// <summary>
    /// Computes the coefficients of the line equation "mx + a", where:
    /// <list type="bullet">
    /// <item><description>x is the slope (m)</description></item>
    /// <item><description>y is the y-intercept (a)</description></item>
    /// </list>
    /// </summary>
    /// <returns>A tuple containing the coefficients (m, a) of the line equation.</returns>
    /// <remarks>
    /// Returns {inf, inf} if the absolute difference between the x-coordinates of the start and end points is less than a small epsilon value.
    /// </remarks>
    public (T m, T a) Coefficients()
    {
        var x1 = Start.X;
        var x2 = End.X;
        var y1 = Start.Y;
        var y2 = End.Y;

        if (Convert.ToDouble(T.Abs(x2 - x1)) < Constants.Epsilon)
        {
            return (T.CreateChecked(double.PositiveInfinity), T.CreateChecked(double.PositiveInfinity));
        }

        var m = (y2 - y1) / (x2 - x1);
        return (m, -m * x1 + y1);
    }
}