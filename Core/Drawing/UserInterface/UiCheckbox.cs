using SFML.Graphics;
using SFML.System;

namespace Core.Drawing.UserInterface;

/// <summary>
/// A labeled checkbox. Clicking anywhere on the box or its label toggles <see cref="Checked"/>.
/// </summary>
public class UiCheckbox : UiElement
{
    private readonly UiText _label = new();
    private readonly Vector2f _boxSize = new(24, 24);
    private const float LabelGap = 10f;
    private bool _checked;

    public event Action<bool> CheckedChanged;

    public Color BoxColor { get; set; } = UiTheme.Surface;
    public Color HoverColor { get; set; } = UiTheme.Hover;
    public Color PressedColor { get; set; } = UiTheme.Pressed;
    public Color OutlineColor { get; set; } = UiTheme.Outline;
    public Color CheckColor { get; set; } = UiTheme.Accent;

    public uint CharSize
    {
        get => _label.CharacterSize;
        set => _label.CharacterSize = value;
    }

    public Color LabelColor
    {
        get => _label.Color;
        set => _label.Color = value;
    }

    public bool Checked
    {
        get => _checked;
        set
        {
            if (_checked == value) return;
            _checked = value;
            CheckedChanged?.Invoke(_checked);
        }
    }

    public string Label
    {
        get => _label.Content;
        set => _label.Content = value;
    }

    protected override void Start()
    {
        AddComponent(_label);
        Measure();
    }

    public override void Measure()
    {
        _label.Measure();
        Size = new Vector2f(_boxSize.X + LabelGap + _label.Size.X, MathF.Max(_boxSize.Y, _label.Size.Y));
    }

    protected override void Update()
    {
        base.Update();
        _label.Position = new Vector2f(
            Position.X + _boxSize.X + LabelGap,
            Position.Y + (_boxSize.Y - _label.Size.Y) / 2f);
    }

    protected override void OnClick()
    {
        Checked = !Checked;
        base.OnClick();
    }

    protected override void Render()
    {
        if (!Visible) return;

        var color = IsHeld ? PressedColor : IsHovered ? HoverColor : BoxColor;
        Draw.Rectangle(Position.X, Position.Y, _boxSize.X, _boxSize.Y,
            new DrawOptions { FillColor = color, OutlineColor = OutlineColor, OutlineThickness = 2 });

        if (!Checked) return;

        const float pad = 5f;
        Draw.Line(new Vector2f(Position.X + pad, Position.Y + pad),
            new Vector2f(Position.X + _boxSize.X - pad, Position.Y + _boxSize.Y - pad),
            new DrawOptions { FillColor = CheckColor });
        Draw.Line(new Vector2f(Position.X + _boxSize.X - pad, Position.Y + pad),
            new Vector2f(Position.X + pad, Position.Y + _boxSize.Y - pad),
            new DrawOptions { FillColor = CheckColor });
    }
}
