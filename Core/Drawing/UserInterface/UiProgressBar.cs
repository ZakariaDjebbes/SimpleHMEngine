using Core.Engine;
using SFML.Graphics;
using SFML.System;

namespace Core.Drawing.UserInterface;

/// <summary>
/// A horizontal bar that fills proportionally to <see cref="Value"/> / <see cref="Max"/>. Values can be
/// pushed directly or pulled each frame from <see cref="ValueProvider"/> / <see cref="MaxProvider"/>,
/// which makes it trivial to bind to live game state (e.g. a health bar) without holding a reference.
/// </summary>
public class UiProgressBar : UiElement
{
    private readonly RectangleShape _background = new();
    private readonly RectangleShape _fill = new();
    private readonly UiText _label = new();

    public float Value { get; set; }
    public float Max { get; set; } = 100f;

    public Func<float> ValueProvider { get; set; }
    public Func<float> MaxProvider { get; set; }

    public Color BackgroundColor { get; set; } = UiTheme.Pressed;
    public Color FillColor { get; set; } = UiTheme.Accent;
    public Color OutlineColor { get; set; } = UiTheme.Outline;
    public bool ShowLabel { get; set; }

    public float Fraction => Max <= 0 ? 0f : Math.Clamp(Value / Max, 0f, 1f);

    public UiProgressBar()
    {
        Interactive = false;
        Size = new Vector2f(200, 22);
        _label.CharacterSize = 14;
    }

    protected override void Start()
    {
        if (ShowLabel) AddComponent(_label);
    }

    protected override void Update()
    {
        if (ValueProvider is not null) Value = ValueProvider();
        if (MaxProvider is not null) Max = MaxProvider();

        if (!ShowLabel) return;

        _label.Content = $"{Value:0} / {Max:0}";
        _label.Measure();
        _label.Position = new Vector2f(
            Position.X + (Size.X - _label.Size.X) / 2f,
            Position.Y + (Size.Y - _label.Size.Y) / 2f);
    }

    protected override void Render()
    {
        if (!Visible) return;

        var window = GameContext.CurrentWindow;

        _background.Position = Position;
        _background.Size = Size;
        _background.FillColor = BackgroundColor;
        _background.OutlineColor = OutlineColor;
        _background.OutlineThickness = 2;
        window.Draw(_background);

        var fraction = Fraction;
        if (fraction <= 0) return;

        _fill.Position = Position;
        _fill.Size = new Vector2f(Size.X * fraction, Size.Y);
        _fill.FillColor = FillColor;
        window.Draw(_fill);
    }
}
