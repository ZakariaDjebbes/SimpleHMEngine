using Core.Engine;
using SFML.Graphics;
using SFML.System;

namespace Core.Drawing.UserInterface;

/// <summary>
/// A panel that draws a (optionally gradient) background with an outline and hosts a single content
/// element, sizing itself to the content plus padding. Useful as a backdrop for menus and dialogs.
/// </summary>
public class UiCard : UiElement
{
    private UiElement _content;

    public Color BackgroundColor { get; set; } = UiTheme.Surface;

    /// <summary>When set, the background is drawn as a vertical gradient from <see cref="BackgroundColor"/> to this color.</summary>
    public Color? GradientEndColor { get; set; }

    public Color OutlineColor { get; set; } = UiTheme.Outline;
    public float OutlineThickness { get; set; } = 2f;
    public Vector2f Padding { get; set; } = new(16, 16);

    public UiCard() => Interactive = false;

    /// <summary>Sets the card's single content element and returns it for fluent wiring.</summary>
    public T SetContent<T>(T content) where T : UiElement
    {
        _content = content;
        AddComponent(content);
        return content;
    }

    public override void Measure()
    {
        if (_content is null) return;

        _content.Measure();
        Size = new Vector2f(_content.Size.X + Padding.X * 2f, _content.Size.Y + Padding.Y * 2f);
    }

    protected override void Update()
    {
        if (_content is not null)
            _content.Position = new Vector2f(Position.X + Padding.X, Position.Y + Padding.Y);

        base.Update();
    }

    protected override void Render()
    {
        if (!Visible) return;

        if (GradientEndColor is { } end)
        {
            var gradient = new GradiantRect(Size, BackgroundColor, end) { Position = Position };
            GameContext.CurrentWindow.Draw(gradient);

            if (OutlineThickness > 0)
                Draw.Rectangle(Position.X, Position.Y, Size.X, Size.Y,
                    new DrawOptions { FillColor = Color.Transparent, OutlineColor = OutlineColor, OutlineThickness = OutlineThickness });
        }
        else
        {
            Draw.Rectangle(Position.X, Position.Y, Size.X, Size.Y,
                new DrawOptions { FillColor = BackgroundColor, OutlineColor = OutlineColor, OutlineThickness = OutlineThickness });
        }
    }
}
