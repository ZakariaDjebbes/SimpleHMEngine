using SFML.Graphics;
using SFML.System;

namespace Core.Drawing.UserInterface;

/// <summary>
/// A clickable button with a centered text label. Inherits all hover/press/click behavior from
/// <see cref="UiElement"/>; only the visuals and label live here.
/// </summary>
public class UiButton : UiElement
{
    private readonly UiText _label = new();

    public Color NormalColor { get; set; } = UiTheme.Surface;
    public Color HoverColor { get; set; } = UiTheme.Hover;
    public Color PressedColor { get; set; } = UiTheme.Pressed;
    public Color OutlineColor { get; set; } = UiTheme.Outline;

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

    public string Label
    {
        get => _label.Content;
        set => _label.Content = value;
    }

    public UiButton() => Size = new Vector2f(200, 48);

    protected override void Start() => AddComponent(_label);

    protected override void Update()
    {
        base.Update();

        _label.Measure();
        _label.Position = new Vector2f(
            Position.X + (Size.X - _label.Size.X) / 2f,
            Position.Y + (Size.Y - _label.Size.Y) / 2f);
    }

    protected override void Render()
    {
        if (!Visible) return;

        var color = IsHeld ? PressedColor : IsHovered ? HoverColor : NormalColor;
        Draw.Rectangle(Position.X, Position.Y, Size.X, Size.Y,
            new DrawOptions { FillColor = color, OutlineColor = OutlineColor, OutlineThickness = 2 });
    }
}
