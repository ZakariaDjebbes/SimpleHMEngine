using ZGeometry.Primitives.Point;

namespace Core.Drawing;

/// <summary>
/// A single tile placed on a <see cref="TileMap"/>.
/// </summary>
/// <param name="index">The (column, row) of the source cell in the texture sheet.</param>
/// <param name="x">The tile's column on the map grid.</param>
/// <param name="y">The tile's row on the map grid.</param>
/// <param name="layer">Optional draw/sort layer.</param>
public struct Tile(Vector2D<int> index, int x, int y, int layer = 0)
{
    /// <summary>The (column, row) of the source cell in the texture sheet.</summary>
    public Vector2D<int> Index { get; set; } = index;

    /// <summary>The tile's column on the map grid.</summary>
    public int X { get; set; } = x;

    /// <summary>The tile's row on the map grid.</summary>
    public int Y { get; set; } = y;

    /// <summary>Optional draw/sort layer.</summary>
    public int Layer { get; set; } = layer;
}
