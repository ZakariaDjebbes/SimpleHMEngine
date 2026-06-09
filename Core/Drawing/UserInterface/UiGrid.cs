using SFML.System;

namespace Core.Drawing.UserInterface;

/// <summary>
/// Arranges its children in a uniform grid of a fixed number of columns. Cell size is the largest
/// child; the grid sizes itself to fit all rows and columns.
/// </summary>
public class UiGrid : UiElement
{
    private readonly List<UiElement> _items = [];

    public int Columns { get; set; } = 2;
    public Vector2f CellSpacing { get; set; } = new(10, 10);

    private float _cellWidth;
    private float _cellHeight;

    public UiGrid() => Interactive = false;

    /// <summary>Adds a child and returns it for fluent wiring.</summary>
    public T Add<T>(T item) where T : UiElement
    {
        _items.Add(item);
        AddComponent(item);
        return item;
    }

    public override void Measure()
    {
        _cellWidth = 0f;
        _cellHeight = 0f;

        foreach (var item in _items)
        {
            item.Measure();
            _cellWidth = MathF.Max(_cellWidth, item.Size.X);
            _cellHeight = MathF.Max(_cellHeight, item.Size.Y);
        }

        var columns = Math.Max(1, Math.Min(Columns, _items.Count == 0 ? 1 : _items.Count));
        var rows = (int)MathF.Ceiling(_items.Count / (float)Math.Max(1, Columns));

        Size = new Vector2f(
            columns * _cellWidth + (columns - 1) * CellSpacing.X,
            rows * _cellHeight + Math.Max(0, rows - 1) * CellSpacing.Y);
    }

    protected override void Update()
    {
        for (var i = 0; i < _items.Count; i++)
        {
            var column = i % Columns;
            var row = i / Columns;
            _items[i].Position = new Vector2f(
                Position.X + column * (_cellWidth + CellSpacing.X),
                Position.Y + row * (_cellHeight + CellSpacing.Y));
        }

        base.Update();
    }
}
