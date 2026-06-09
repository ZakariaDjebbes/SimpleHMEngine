using Core.Entity;
using Core.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Core.Drawing.UserInterface;

/// <summary>
/// Defines where an element is placed within its parent area.
/// </summary>
public enum Anchor
{
    TopLeft, TopCenter, TopRight,
    MiddleLeft, Center, MiddleRight,
    BottomLeft, BottomCenter, BottomRight
}

/// <summary>
/// Base class for all UI widgets. Provides bounds, anchoring and a shared hover/press/click
/// state machine so concrete widgets only need to implement their visuals.
/// </summary>
public abstract class UiElement : Component
{
    /// <summary>The element's size in screen pixels.</summary>
    public Vector2f Size { get; set; }

    /// <summary>Pixel offset applied after anchoring.</summary>
    public Vector2f Margin { get; set; }

    /// <summary>How the element is anchored within its container (used by <see cref="UiCanvas"/> for roots).</summary>
    public Anchor Anchor { get; set; } = Anchor.TopLeft;

    /// <summary>Whether the element is drawn.</summary>
    public bool Visible { get; set; } = true;

    /// <summary>Whether the element responds to the mouse (hover/click). Containers turn this off.</summary>
    public bool Interactive { get; set; } = true;

    public event Action Clicked;
    public event Action HoverEnter;
    public event Action HoverStay;
    public event Action HoverExit;

    protected bool IsHovered { get; private set; }
    protected bool IsHeld { get; private set; }

    private bool _wasHovered;
    private bool _wasDown;
    private bool _pressStartedInside;

    /// <summary>The element's screen-space rectangle.</summary>
    public FloatRect Bounds => new(Position.X, Position.Y, Size.X, Size.Y);

    /// <summary>Whether the given point lies within the element's bounds.</summary>
    public bool Contains(Vector2f point) => Bounds.Contains(point.X, point.Y);

    /// <summary>
    /// Computes <see cref="Size"/> from the element's content. Leaf widgets keep their assigned size;
    /// containers override this to size to their children.
    /// </summary>
    public virtual void Measure() { }

    /// <summary>Positions the element within <paramref name="area"/> according to its anchor and margin.</summary>
    public void ApplyAnchor(FloatRect area)
    {
        var x = Anchor switch
        {
            Anchor.TopLeft or Anchor.MiddleLeft or Anchor.BottomLeft => area.Left,
            Anchor.TopCenter or Anchor.Center or Anchor.BottomCenter => area.Left + (area.Width - Size.X) / 2f,
            _ => area.Left + area.Width - Size.X
        };

        var y = Anchor switch
        {
            Anchor.TopLeft or Anchor.TopCenter or Anchor.TopRight => area.Top,
            Anchor.MiddleLeft or Anchor.Center or Anchor.MiddleRight => area.Top + (area.Height - Size.Y) / 2f,
            _ => area.Top + area.Height - Size.Y
        };

        Position = new Vector2f(x + Margin.X, y + Margin.Y);
    }

    protected override void Update()
    {
        if (!Interactive || !Visible)
        {
            IsHovered = IsHeld = false;
            return;
        }

        var mouse = MouseUtils.GetUiMousePosition();
        _wasHovered = IsHovered;
        IsHovered = Contains(mouse);

        var down = Mouse.IsButtonPressed(Mouse.Button.Left);
        IsHeld = IsHovered && down;

        // A click is a press that begins and ends inside the element.
        if (IsHovered && down && !_wasDown) _pressStartedInside = true;
        if (_pressStartedInside && !down && _wasDown)
        {
            if (IsHovered) OnClick();
            _pressStartedInside = false;
        }
        if (!down && !IsHovered) _pressStartedInside = false;
        _wasDown = down;

        if (IsHovered)
        {
            if (!_wasHovered) HoverEnter?.Invoke();
            HoverStay?.Invoke();
        }
        else if (_wasHovered)
        {
            HoverExit?.Invoke();
        }
    }

    /// <summary>Called when the element is clicked. Override to add behavior; base raises <see cref="Clicked"/>.</summary>
    protected virtual void OnClick() => Clicked?.Invoke();
}
