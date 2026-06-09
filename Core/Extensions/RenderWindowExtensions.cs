using SFML.Graphics;

namespace Core.Extensions;

/// <summary>
/// Provides extension methods for working with SFML RenderWindow objects.
/// </summary>
public static class RenderWindowExtensions
{
    /// <summary>
    /// Draws all the specified drawable objects to the render window.
    /// </summary>
    /// <param name="renderWindow">The render window to draw to.</param>
    /// <param name="drawables">The drawable objects to render.</param>
    public static void DrawAll(this RenderWindow renderWindow, params Drawable[] drawables) 
        => drawables.EnumeratedForEach(renderWindow.Draw);
}