using SFML.Graphics;
using SFML.System;
using ZGeometry.Primitives.Point;

namespace Core.Drawing;

/// <summary>
/// A single tile placed on a <see cref="TileMap"/>. A tile is identified on the map by its cell and
/// layer (<see cref="X"/>, <see cref="Y"/>, <see cref="Layer"/>); placing another tile at the same
/// cell and layer replaces it.
/// </summary>
/// <param name="index">The (column, row) of the source cell in the texture sheet.</param>
/// <param name="x">The tile's column on the map grid.</param>
/// <param name="y">The tile's row on the map grid.</param>
/// <param name="layer">The draw/sort layer; lower layers are drawn first (underneath).</param>
public struct Tile2D(Vector2D<int> index, int x, int y, int layer = 0)
{
    /// <summary>The (column, row) of the source cell in the texture sheet.</summary>
    public Vector2D<int> Index { get; set; } = index;

    /// <summary>The tile's column on the map grid.</summary>
    public int X { get; set; } = x;

    /// <summary>The tile's row on the map grid.</summary>
    public int Y { get; set; } = y;

    /// <summary>The draw/sort layer. Lower layers are drawn first, so higher layers appear on top.</summary>
    public int Layer { get; set; } = layer;

    /// <summary>The colour the tile is tinted with (multiplied with the texture). Defaults to white (no tint).</summary>
    public Color Tint { get; set; } = Color.White;

    /// <summary>Per-tile scale, relative to the map's tile size. Defaults to (1, 1). The tile is anchored at the top-left of its cell.</summary>
    public Vector2f Scale { get; set; } = new(1, 1);

    /// <summary>Whether the tile's source image is flipped horizontally.</summary>
    public bool FlipX { get; set; }

    /// <summary>Whether the tile's source image is flipped vertically.</summary>
    public bool FlipY { get; set; }
}
