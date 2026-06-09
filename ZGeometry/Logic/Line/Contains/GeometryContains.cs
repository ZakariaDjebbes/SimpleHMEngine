using System.Numerics;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;

namespace ZGeometry.Logic;

public static partial class Geometry
{
    /// <summary>
    /// Determines whether a point lies on a given line.
    /// </summary>
    /// <typeparam name="T">The numeric type of the vector components.</typeparam>
    /// <param name="line">The <see cref="Line{T}"/> to check.</param>
    /// <param name="point">The <see cref="Vector2D{T}"/> to check.</param>
    /// <returns>True if the point lies on the line; otherwise, false.</returns>
    public static bool Contains<T>(Line<T> line, Vector2D<T> point) where T : struct, INumber<T>
        => Contains(point, line);
}
