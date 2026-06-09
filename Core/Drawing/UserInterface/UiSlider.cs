using Core.Engine;
using Core.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Core.Drawing.UserInterface;

/// <summary>
/// A draggable horizontal slider that maps the mouse position to a value in [<see cref="Min"/>, <see cref="Max"/>]
/// and raises <see cref="ValueChanged"/>.
/// </summary>
public class UiSlider : UiElement
{
    private readonly RectangleShape _track = new();
    private readonly RectangleShape _fill = new();
    private readonly RectangleShape _thumb = new();
    private bool _dragging;

    private const float ThumbWidth = 12f;
    private const float TrackHeight = 6f;

    /// <summary>Minimum value, mapped to the left end.</summary>
    public float Min { get; set; } = 0f;

    /// <summary>Maximum value, mapped to the right end.</summary>
    public float Max { get; set; } = 1f;

    /// <summary>The current value. Set via <see cref="SetValue"/> or by dragging.</summary>
    public float Value { get; private set; }

    /// <summary>Raised when the value changes, with the new value.</summary>
    public event Action<float> ValueChanged;

    /// <summary>Color of the slider track.</summary>
    public Color TrackColor { get; set; } = UiTheme.Pressed;

    /// <summary>Color of the filled portion of the track.</summary>
    public Color FillColor { get; set; } = UiTheme.AccentDark;

    /// <summary>Color of the draggable thumb.</summary>
    public Color ThumbColor { get; set; } = UiTheme.Accent;

    /// <summary>Outline color of the track and thumb.</summary>
    public Color OutlineColor { get; set; } = UiTheme.Outline;

    /// <summary>The value as a 0..1 fraction of the <see cref="Min"/>..<see cref="Max"/> range.</summary>
    public float Fraction => Max <= Min ? 0f : Math.Clamp((Value - Min) / (Max - Min), 0f, 1f);

    /// <summary>Creates a slider with a default size.</summary>
    public UiSlider() => Size = new Vector2f(200, 24);

    /// <summary>Sets the value (clamped) and raises <see cref="ValueChanged"/> if it changed.</summary>
    public void SetValue(float value)
    {
        var clamped = Math.Clamp(value, Min, Max);
        if (Math.Abs(clamped - Value) < float.Epsilon) return;
        Value = clamped;
        ValueChanged?.Invoke(Value);
    }

    /// <summary>Begins/continues dragging and maps the pointer position to the value.</summary>
    protected override void Update()
    {
        var mouse = MouseUtils.GetUiMousePosition();
        var down = Mouse.IsButtonPressed(Mouse.Button.Left);

        if (down && !_dragging && Contains(mouse)) _dragging = true;
        if (!down) _dragging = false;

        if (_dragging)
        {
            var t = Math.Clamp((mouse.X - Position.X) / Size.X, 0f, 1f);
            SetValue(Min + t * (Max - Min));
        }
    }

    /// <summary>Draws the track, the filled portion and the thumb.</summary>
    protected override void Render()
    {
        if (!Visible) return;

        var window = GameContext.CurrentWindow;
        var trackY = Position.Y + (Size.Y - TrackHeight) / 2f;
        var fillWidth = Size.X * Fraction;

        _track.Position = new Vector2f(Position.X, trackY);
        _track.Size = new Vector2f(Size.X, TrackHeight);
        _track.FillColor = TrackColor;
        _track.OutlineColor = OutlineColor;
        _track.OutlineThickness = 1;
        window.Draw(_track);

        _fill.Position = new Vector2f(Position.X, trackY);
        _fill.Size = new Vector2f(fillWidth, TrackHeight);
        _fill.FillColor = FillColor;
        window.Draw(_fill);

        _thumb.Position = new Vector2f(Position.X + fillWidth - ThumbWidth / 2f, Position.Y);
        _thumb.Size = new Vector2f(ThumbWidth, Size.Y);
        _thumb.FillColor = ThumbColor;
        _thumb.OutlineColor = OutlineColor;
        _thumb.OutlineThickness = 1;
        window.Draw(_thumb);
    }
}
