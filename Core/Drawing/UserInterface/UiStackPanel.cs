using SFML.System;

namespace Core.Drawing.UserInterface;

/// <summary>Layout direction for a <see cref="UiStackPanel"/>.</summary>
public enum Orientation { Vertical, Horizontal }

/// <summary>Cross-axis alignment of items within a <see cref="UiStackPanel"/>.</summary>
public enum ItemAlignment { Start, Center, End }

/// <summary>
/// Arranges its children in a single row or column with configurable spacing, padding and
/// cross-axis alignment. Sizes itself to fit its content.
/// </summary>
public class UiStackPanel : UiElement
{
    private readonly List<UiElement> _items = [];

    public Orientation Orientation { get; set; } = Orientation.Vertical;
    public float Spacing { get; set; } = 10f;
    public Vector2f Padding { get; set; }
    public ItemAlignment Alignment { get; set; } = ItemAlignment.Center;

    /// <summary>
    /// Creates a new StackPanel. StackPanels are not interactive by default.
    /// </summary>
    public UiStackPanel() => Interactive = false;

    /// <summary>Adds a child and returns it for fluent wiring.</summary>
    public T Add<T>(T item) where T : UiElement
    {
        _items.Add(item);
        AddComponent(item);
        return item;
    }

    public override void Measure()
    {
        var width = 0f;
        var height = 0f;

        foreach (var item in _items)
        {
            item.Measure();
            if (Orientation == Orientation.Vertical)
            {
                width = MathF.Max(width, item.Size.X);
                height += item.Size.Y;
            }
            else
            {
                height = MathF.Max(height, item.Size.Y);
                width += item.Size.X;
            }
        }

        if (_items.Count > 1)
        {
            var gaps = Spacing * (_items.Count - 1);
            if (Orientation == Orientation.Vertical) height += gaps; else width += gaps;
        }

        Size = new Vector2f(width + Padding.X * 2f, height + Padding.Y * 2f);
    }

    protected override void Update()
    {
        Arrange();
        base.Update();
    }

    private void Arrange()
    {
        var x = Position.X + Padding.X;
        var y = Position.Y + Padding.Y;
        var innerWidth = Size.X - Padding.X * 2f;
        var innerHeight = Size.Y - Padding.Y * 2f;

        foreach (var item in _items)
        {
            if (Orientation == Orientation.Vertical)
            {
                item.Position = new Vector2f(Cross(item.Size.X, innerWidth, x), y);
                y += item.Size.Y + Spacing;
            }
            else
            {
                item.Position = new Vector2f(x, Cross(item.Size.Y, innerHeight, y));
                x += item.Size.X + Spacing;
            }
        }
    }

    private float Cross(float itemExtent, float available, float origin) => Alignment switch
    {
        ItemAlignment.Center => origin + (available - itemExtent) / 2f,
        ItemAlignment.End => origin + available - itemExtent,
        _ => origin
    };
}
