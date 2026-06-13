using System.Numerics;
using SFML.System;
using ZGeometry.Primitives.Point;

namespace Core.Extensions;

/// <summary>
/// Provides extension methods for converting SFML vectors to ZGeometry <see cref="Vector2D{T}"/> values.
/// </summary>
public static class VectorExtensions
{
    /// <summary>Converts an SFML <see cref="Vector2f"/> to a ZGeometry <see cref="Vector2D{T}"/>.</summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>A <see cref="Vector2D{T}"/> with the same components.</returns>
    public static Vector2D<float> ToVector2D(this Vector2f vector) => new(vector.X, vector.Y);

    /// <summary>Converts an SFML <see cref="Vector2i"/> to a ZGeometry <see cref="Vector2D{T}"/>.</summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>A <see cref="Vector2D{T}"/> with the same components.</returns>
    public static Vector2D<int> ToVector2D(this Vector2i vector) => new(vector.X, vector.Y);

    /// <summary>Converts an SFML <see cref="Vector2u"/> to a ZGeometry <see cref="Vector2D{T}"/>.</summary>
    /// <param name="vector">The vector to convert.</param>
    /// <returns>A <see cref="Vector2D{T}"/> with the same components.</returns>
    public static Vector2D<uint> ToVector2D(this Vector2u vector) => new(vector.X, vector.Y);
}