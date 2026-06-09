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

    public float Min { get; set; } = 0f;
    public float Max { get; set; } = 1f;
    public float Value { get; private set; }

    public event Action<float> ValueChanged;

    public Color TrackColor { get; set; } = UiTheme.Pressed;
    public Color FillColor { get; set; } = UiTheme.AccentDark;
    public Color ThumbColor { get; set; } = UiTheme.Accent;
    public Color OutlineColor { get; set; } = UiTheme.Outline;

    public float Fraction => Max <= Min ? 0f : Math.Clamp((Value - Min) / (Max - Min), 0f, 1f);

    public UiSlider() => Size = new Vector2f(200, 24);

    /// <summary>Sets the value (clamped) and raises <see cref="ValueChanged"/> if it changed.</summary>
    public void SetValue(float value)
    {
        var clamped = Math.Clamp(value, Min, Max);
        if (Math.Abs(clamped - Value) < float.Epsilon) return;
        Value = clamped;
        ValueChanged?.Invoke(Value);
    }

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
