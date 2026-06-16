using Core.Entity;
using SFML.Graphics;
using SFML.System;

namespace Core.Engine;

/// <summary>
/// A camera component that drives the render window's <see cref="View"/>. It follows a target <see cref="Component"/>
/// (its parent by default) with an optional offset, zoom and smoothing.
/// </summary>
public class Camera : Component
{
    private View _view;

    /// <summary>Offset added to the focus position, in world units.</summary>
    public Vector2f Offset { get; set; }

    /// <summary>
    /// View zoom factor. 1 = no zoom; values below 1 zoom in (the world looks bigger),
    /// values above 1 zoom out (more of the world is visible).
    /// </summary>
    public float Zoom { get; set; } = 1.0f;

    /// <summary>Optional explicit follow target. When null, the camera follows its parent component.</summary>
    public Component Target { get; set; }

    /// <summary>
    /// Follow smoothing, expressed as the fraction of the remaining distance left after one second.
    /// 0 snaps instantly to the target; small positive values (e.g. 0.001) ease toward it in a
    /// frame-rate independent way.
    /// </summary>
    public float Smoothing { get; set; }

    /// <summary>Creates the view centered on the focus point and applies it to the window.</summary>
    protected override void Start()
    {
        var window = GameContext.CurrentWindow;
        _view = new View(FocusPoint, new Vector2f(window.Size.X * Zoom, window.Size.Y * Zoom));
        window.SetView(_view);
    }

    /// <summary>Tracks the target each frame, applying zoom and smoothing, and updates the window view.</summary>
    protected override void Update()
    {
        var window = GameContext.CurrentWindow;
        _view.Size = new Vector2f(window.Size.X * Zoom, window.Size.Y * Zoom);

        var focus = FocusPoint;
        _view.Center = Smoothing <= 0.0f
            ? focus
            : Lerp(_view.Center, focus, 1.0f - MathF.Pow(Smoothing, GameContext.DeltaTime));

        window.SetView(_view);
    }

    private Vector2f FocusPoint
    {
        get
        {
            var anchor = (Target ?? Parent)?.Position ?? Position;
            return new Vector2f(anchor.X + Offset.X, anchor.Y + Offset.Y);
        }
    }

    private static Vector2f Lerp(Vector2f from, Vector2f to, float t)
        => new(from.X + (to.X - from.X) * t, from.Y + (to.Y - from.Y) * t);
}
