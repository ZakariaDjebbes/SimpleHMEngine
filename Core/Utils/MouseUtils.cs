using Core.Engine;
using SFML.System;
using SFML.Window;

namespace Core.Utils;

/// <summary>
/// Provides utility methods for working with mouse input in the game engine.
/// </summary>
public static class MouseUtils
{
    /// <summary>
    /// Gets the current mouse position in world coordinates, taking into account the current view.
    /// </summary>
    /// <returns>The mouse position in world coordinates.</returns>
    /// <exception cref="NullReferenceException">Thrown when there is no current scene or view set in the current window.</exception>
    public static Vector2f GetCurrentViewMousePosition()
    {
        var currentWindow = GameContext.CurrentWindow;
        var mousePosition = Mouse.GetPosition(currentWindow);
        return currentWindow.MapPixelToCoords(mousePosition, currentWindow.GetView() ?? throw new NullReferenceException("There is no view set in current window"));
    }

    /// <summary>
    /// Gets the current mouse position in screen-space (default view) coordinates, independent of the
    /// game camera. Use this for UI hit-testing.
    /// </summary>
    /// <returns>The mouse position in screen-space coordinates.</returns>
    public static Vector2f GetUiMousePosition()
    {
        var currentWindow = GameContext.CurrentWindow;
        return currentWindow.MapPixelToCoords(Mouse.GetPosition(currentWindow), currentWindow.DefaultView);
    }
}