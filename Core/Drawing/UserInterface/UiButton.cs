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

    /// <summary>Fill color when idle.</summary>
    public Color NormalColor { get; set; } = UiTheme.Surface;

    /// <summary>Fill color while hovered.</summary>
    public Color HoverColor { get; set; } = UiTheme.Hover;

    /// <summary>Fill color while pressed.</summary>
    public Color PressedColor { get; set; } = UiTheme.Pressed;

    /// <summary>Outline color.</summary>
    public Color OutlineColor { get; set; } = UiTheme.Outline;

    /// <summary>Character size, in pixels, of the button label.</summary>
    public uint CharSize
    {
        get => _label.CharacterSize;
        set => _label.CharacterSize = value;
    }

    /// <summary>Color of the button label.</summary>
    public Color LabelColor
    {
        get => _label.Color;
        set => _label.Color = value;
    }

    /// <summary>The button's text label.</summary>
    public string Label
    {
        get => _label.Content;
        set => _label.Content = value;
    }

    /// <summary>Creates a button with a default size.</summary>
    public UiButton() => Size = new Vector2f(200, 48);

    /// <summary>Attaches the label as a child element.</summary>
    protected override void Start() => AddComponent(_label);

    /// <summary>Runs interaction handling and centers the label within the button.</summary>
    protected override void Update()
    {
        base.Update();

        _label.Measure();
        _label.Position = new Vector2f(
            Position.X + (Size.X - _label.Size.X) / 2f,
            Position.Y + (Size.Y - _label.Size.Y) / 2f);
    }

    /// <summary>Draws the button background in its current interaction color.</summary>
    protected override void Render()
    {
        if (!Visible) return;

        var color = IsHeld ? PressedColor : IsHovered ? HoverColor : NormalColor;
        Draw.Rectangle(Position.X, Position.Y, Size.X, Size.Y,
            new DrawOptions { FillColor = color, OutlineColor = OutlineColor, OutlineThickness = 2 });
    }
}
