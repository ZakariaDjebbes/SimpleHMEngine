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

    /// <summary>Raised when the checked state changes, with the new value.</summary>
    public event Action<bool> CheckedChanged;

    /// <summary>Box fill color when idle.</summary>
    public Color BoxColor { get; set; } = UiTheme.Surface;

    /// <summary>Box fill color while hovered.</summary>
    public Color HoverColor { get; set; } = UiTheme.Hover;

    /// <summary>Box fill color while pressed.</summary>
    public Color PressedColor { get; set; } = UiTheme.Pressed;

    /// <summary>Box outline color.</summary>
    public Color OutlineColor { get; set; } = UiTheme.Outline;

    /// <summary>Color of the check mark.</summary>
    public Color CheckColor { get; set; } = UiTheme.Accent;

    /// <summary>Character size, in pixels, of the label.</summary>
    public uint CharSize
    {
        get => _label.CharacterSize;
        set => _label.CharacterSize = value;
    }

    /// <summary>Color of the label text.</summary>
    public Color LabelColor
    {
        get => _label.Color;
        set => _label.Color = value;
    }

    /// <summary>Whether the checkbox is checked. Setting it raises <see cref="CheckedChanged"/>.</summary>
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

    /// <summary>The checkbox's text label.</summary>
    public string Label
    {
        get => _label.Content;
        set => _label.Content = value;
    }

    /// <summary>Attaches the label and measures the element.</summary>
    protected override void Start()
    {
        AddComponent(_label);
        Measure();
    }

    /// <summary>Sizes the element to fit the box plus the label.</summary>
    public override void Measure()
    {
        _label.Measure();
        Size = new Vector2f(_boxSize.X + LabelGap + _label.Size.X, MathF.Max(_boxSize.Y, _label.Size.Y));
    }

    /// <summary>Runs interaction handling and positions the label beside the box.</summary>
    protected override void Update()
    {
        base.Update();
        _label.Position = new Vector2f(
            Position.X + _boxSize.X + LabelGap,
            Position.Y + (_boxSize.Y - _label.Size.Y) / 2f);
    }

    /// <summary>Toggles <see cref="Checked"/> and raises the click event.</summary>
    protected override void OnClick()
    {
        Checked = !Checked;
        base.OnClick();
    }

    /// <summary>Draws the box and, when checked, the check mark.</summary>
    protected override void Render()
    {
        if (!Visible) return;

        var color = IsHeld ? PressedColor : IsHovered ? HoverColor : BoxColor;
        Draw.Rectangle(Position.X, Position.Y, _boxSize.X, _boxSize.Y,
            new DrawOption.DrawOptions { FillColor = color, OutlineColor = OutlineColor, OutlineThickness = 2 });

        if (!Checked) return;

        const float pad = 5f;
        Draw.Line(new Vector2f(Position.X + pad, Position.Y + pad),
            new Vector2f(Position.X + _boxSize.X - pad, Position.Y + _boxSize.Y - pad),
            new DrawOption.DrawOptions { FillColor = CheckColor });
        Draw.Line(new Vector2f(Position.X + _boxSize.X - pad, Position.Y + pad),
            new Vector2f(Position.X + pad, Position.Y + _boxSize.Y - pad),
            new DrawOption.DrawOptions { FillColor = CheckColor });
    }
}
