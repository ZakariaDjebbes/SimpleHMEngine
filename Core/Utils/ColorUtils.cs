using SFML.Graphics;

namespace Core.Utils;

/// <summary>
/// Provides utility methods for working with colors in the game engine.
/// </summary>
public static class ColorUtils
{
    /// <summary>
    /// Performs linear interpolation between two colors.
    /// </summary>
    /// <param name="colorA">The starting color.</param>
    /// <param name="colorB">The ending color.</param>
    /// <param name="t">The interpolation factor between 0.0 and 1.0.</param>
    /// <returns>A new color that is the result of interpolating between the two input colors.</returns>
    public static Color Lerp(Color colorA, Color colorB, float t)
    {
        t = Math.Clamp(t, 0f, 1f);

        var r = (byte)(colorA.R + (colorB.R - colorA.R) * t);
        var g = (byte)(colorA.G + (colorB.G - colorA.G) * t);
        var b = (byte)(colorA.B + (colorB.B - colorA.B) * t);
        var a = (byte)(colorA.A + (colorB.A - colorA.A) * t);
        
        return new Color(r, g, b, a);
    }
}