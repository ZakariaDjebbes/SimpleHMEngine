using Core.Input;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Core.Engine;

/// <summary>
/// Provides global access to game engine context and timing information.
/// This class manages the main game window and handles time-based updates.
/// </summary>
public static class GameContext
{
    /// <summary>
    /// Gets the time elapsed since the last frame in seconds.
    /// </summary>
    public static float DeltaTime { get; private set; }

    /// <summary>
    /// Gets the fixed time step used for physics and other fixed updates.
    /// Default is 1/60 seconds (60 FPS).
    /// </summary>
    public static float FixedDeltaTime { get; private set; } = 1.0f / 60.0f;

    /// <summary>
    /// Gets the current frames per second.
    /// </summary>
    public static float Fps { get; private set; }
    
    public static bool IsDebugMode { get; set; } = false;
    public static bool IsPaused { get; set; } = false;

    /// <summary>
    /// Gets the current SFML render window.
    /// </summary>
    public static RenderWindow CurrentWindow { get; private set; }

    private static readonly Clock Clock = new();
    private static float _fixedTimeAccumulator;

    /// <summary>
    /// Initializes the game context with a new render window.
    /// </summary>
    /// <param name="width">The width of the window in pixels.</param>
    /// <param name="height">The height of the window in pixels.</param>
    /// <param name="title">The title of the window.</param>
    public static void Initialize(uint width, uint height, string title)
    {
        CurrentWindow = new RenderWindow(new VideoMode(width, height), title);
        CurrentWindow.Closed += CurrentWindowClosed;
        CurrentWindow.KeyPressed += InputManager.HandleKeyPressed;
        CurrentWindow.KeyReleased += InputManager.HandleKeyReleased;
        CurrentWindow.MouseButtonPressed += InputManager.HandleMouseButtonPressed;
        CurrentWindow.MouseButtonReleased += InputManager.HandleMouseButtonReleased;
    }

    /// <summary>
    /// Updates the timing information for the current frame.
    /// This should be called once per frame.
    /// </summary>
    public static void UpdateTime()
    {
        DeltaTime = Clock.Restart().AsSeconds();
        Fps = 1 / DeltaTime;
        _fixedTimeAccumulator += DeltaTime;
    }

    /// <summary>
    /// Determines if a fixed update should be performed based on the accumulated time.
    /// </summary>
    /// <returns>True if a fixed update should be performed, false otherwise.</returns>
    public static bool ShouldFixedUpdate()
    {
        if (!(_fixedTimeAccumulator >= FixedDeltaTime)) return false;
        
        _fixedTimeAccumulator -= FixedDeltaTime;
        return true;
    }

    private static void CurrentWindowClosed(object sender, EventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);
        var window = (Window)sender;
        window.Close();
    }
}
