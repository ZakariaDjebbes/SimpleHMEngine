using System.Numerics;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;

namespace ZGeometry.Logic;

/// <summary>
/// Provides extension methods for working with 2D points and shapes in a fluent manner.
/// </summary>
public static partial class FluentGeometryContains
{
    /// <summary>
    /// Determines whether a point lies on the current line.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="line">The current <see cref="Line{T}"/>.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <returns>True if the point lies on the line; otherwise, false.</returns>
    public static bool Contains<T>(this Line<T> line, Vector2D<T> point) where T : struct, INumber<T>
        => Geometry.Contains(line, point);
}
