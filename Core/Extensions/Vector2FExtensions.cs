using SFML.System;

namespace Core.Extensions;

/// <summary>
/// Provides extension methods for working with Vector2f objects.
/// </summary>
public static class Vector2FExtensions
{
    /// <summary>
    /// Returns a normalized version of the vector (unit vector with length 1).
    /// </summary>
    /// <param name="vector">The vector to normalize.</param>
    /// <returns>A new Vector2f that is the normalized version of the input vector.</returns>
    /// <remarks>If the input vector has zero length, it is returned unchanged.</remarks>
    public static Vector2f Normalized(this Vector2f vector)
    {
        var length = MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        return length > 0 ? new Vector2f(vector.X / length, vector.Y / length) : vector;
    }
    
    /// <summary>
    /// Calculates the Euclidean distance between two points represented as Vector2f.
    /// </summary>
    /// <param name="point1">The first point.</param>
    /// <param name="point2">The second point.</param>
    /// <returns>The distance between the two points.</returns>
    public static float Distance(this Vector2f point1, Vector2f point2)
    {
        var dx = point1.X - point2.X;
        var dy = point1.Y - point2.Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }
}