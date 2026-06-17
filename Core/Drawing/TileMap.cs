using Core.Drawing.DrawOption;
using Core.Entity;
using Core.Resources;
using SFML.Graphics;
using SFML.System;
using ZGeometry.Primitives.Point;

namespace Core.Drawing;

/// <summary>
/// Renders a grid of tiles cut from a single texture sheet. Tiles are grouped by
/// <see cref="Tile2D.Layer"/> and each layer is drawn in one call from a cached <see cref="VertexArray"/>,
/// lowest layer first. The map's world origin is the component's <see cref="Component.Position"/>, and
/// <see cref="TileScale"/> scales the whole map; both are applied through a render transform, so moving
/// or scaling the map is free and needs no rebuild. The vertex data is rebuilt lazily only when the
/// tiles change.
/// </summary>
public class TileMap(Vector2D<int> tileSize, string texture = null) : Component
{
    // Keyed by cell + layer so a tile placed at an occupied (x, y, layer) replaces the previous one.
    private readonly Dictionary<(int X, int Y, int Layer), Tile2D> _tiles = new();
    private readonly HashSet<int> _hiddenLayers = [];
    private readonly SortedDictionary<int, VertexArray> _layerVertices = new();
    private Vector2f _localSize;
    private Texture _sheet;
    private bool _dirty = true;

    /// <summary>The texture sheet the tiles are cut from. When null/empty, the embedded default texture is used.</summary>
    public string Texture { get; init; } = texture;

    /// <summary>The pixel size of a single source tile in the sheet (e.g. 16x16).</summary>
    public Vector2D<int> TileSize { get; } = tileSize;

    /// <summary>On-screen scale applied to the whole map. Applied via the render transform, so changing it is free.</summary>
    public Vector2f TileScale { get; set; } = new(1, 1);

    /// <summary>Whether the map is drawn.</summary>
    public bool Visible { get; set; } = true;

    /// <summary>The on-screen size of a single unscaled tile, in pixels (<see cref="TileSize"/> times <see cref="TileScale"/>).</summary>
    public Vector2f ScaledTileSize => new(TileSize.X * TileScale.X, TileSize.Y * TileScale.Y);

    /// <summary>The map's drawn size in pixels, measured from its origin to the far edge of the furthest tile.</summary>
    public Vector2f MapPixelSize
    {
        get
        {
            EnsureBuilt();
            return new Vector2f(_localSize.X * TileScale.X, _localSize.Y * TileScale.Y);
        }
    }

    /// <summary>The number of tiles currently on the map.</summary>
    public int TileCount => _tiles.Count;

    /// <summary>The tiles currently on the map, in no particular order.</summary>
    public IEnumerable<Tile2D> Tiles => _tiles.Values;

    /// <summary>Places a tile2D, replacing any existing tile2D at the same cell and layer.</summary>
    /// <param name="tile2D">The tile2D to place.</param>
    public void SetTile(Tile2D tile2D)
    {
        _tiles[(tile2D.X, tile2D.Y, tile2D.Layer)] = tile2D;
        _dirty = true;
    }

    /// <summary>Places many tiles, each replacing any existing tile at the same cell and layer.</summary>
    /// <param name="tiles">The tiles to place.</param>
    public void AddTiles(IEnumerable<Tile2D> tiles)
    {
        foreach (var tile in tiles)
            _tiles[(tile.X, tile.Y, tile.Layer)] = tile;
        _dirty = true;
    }

    /// <summary>Removes the tile at the given cell and layer.</summary>
    /// <param name="x">The column on the map grid.</param>
    /// <param name="y">The row on the map grid.</param>
    /// <param name="layer">The layer to remove from.</param>
    /// <returns><c>true</c> if a tile was found and removed; otherwise <c>false</c>.</returns>
    public bool RemoveTile(int x, int y, int layer = 0)
    {
        if (!_tiles.Remove((x, y, layer)))
            return false;

        _dirty = true;
        return true;
    }

    /// <summary>Gets the tile2D at the given cell and layer.</summary>
    /// <param name="x">The column on the map grid.</param>
    /// <param name="y">The row on the map grid.</param>
    /// <param name="layer">The layer to look in.</param>
    /// <param name="tile2D">The tile2D found, if any.</param>
    /// <returns><c>true</c> if a tile2D exists at that cell and layer; otherwise <c>false</c>.</returns>
    public bool TryGetTile(int x, int y, int layer, out Tile2D tile2D) => _tiles.TryGetValue((x, y, layer), out tile2D);

    /// <summary>Removes every tile from the map.</summary>
    public void ClearTiles()
    {
        if (_tiles.Count == 0)
            return;

        _tiles.Clear();
        _dirty = true;
    }

    /// <summary>Shows or hides a single layer without removing its tiles.</summary>
    /// <param name="layer">The layer to toggle.</param>
    /// <param name="visible">Whether the layer should be drawn.</param>
    public void SetLayerVisible(int layer, bool visible)
    {
        if (visible)
            _hiddenLayers.Remove(layer);
        else
            _hiddenLayers.Add(layer);
    }

    /// <summary>Resolves the texture sheet on scene start.</summary>
    protected override void Start() => ResolveSheet();

    /// <summary>
    /// Rebuilds the per-layer vertex arrays from the current tiles. Called automatically the next time the
    /// map is drawn after a change, so it rarely needs calling by hand.
    /// </summary>
    public void Rebuild()
    {
        _layerVertices.Clear();
        var maxX = 0f;
        var maxY = 0f;

        foreach (var group in _tiles.Values.GroupBy(tile => tile.Layer))
        {
            var tiles = group.ToList();
            var vertices = new VertexArray(PrimitiveType.Triangles, (uint)(tiles.Count * 6));

            uint v = 0;
            foreach (var tile in tiles)
            {
                // Local (pre-transform) geometry: position and map scale are applied at draw time.
                var left = tile.X * (float)TileSize.X;
                var top = tile.Y * (float)TileSize.Y;
                var right = left + TileSize.X * tile.Scale.X;
                var bottom = top + TileSize.Y * tile.Scale.Y;

                var texLeft = (float)(tile.Index.X * TileSize.X);
                var texTop = (float)(tile.Index.Y * TileSize.Y);
                var texRight = texLeft + TileSize.X;
                var texBottom = texTop + TileSize.Y;
                if (tile.FlipX) (texLeft, texRight) = (texRight, texLeft);
                if (tile.FlipY) (texTop, texBottom) = (texBottom, texTop);

                var color = tile.Tint;
                var topLeft = new Vertex(new Vector2f(left, top), color, new Vector2f(texLeft, texTop));
                var topRight = new Vertex(new Vector2f(right, top), color, new Vector2f(texRight, texTop));
                var bottomRight = new Vertex(new Vector2f(right, bottom), color, new Vector2f(texRight, texBottom));
                var bottomLeft = new Vertex(new Vector2f(left, bottom), color, new Vector2f(texLeft, texBottom));

                // Two triangles per tile (SFML 3 removed PrimitiveType.Quads).
                vertices[v++] = topLeft;
                vertices[v++] = topRight;
                vertices[v++] = bottomRight;
                vertices[v++] = topLeft;
                vertices[v++] = bottomRight;
                vertices[v++] = bottomLeft;

                maxX = Math.Max(maxX, right);
                maxY = Math.Max(maxY, bottom);
            }

            _layerVertices[group.Key] = vertices;
        }

        _localSize = new Vector2f(maxX, maxY);
        _dirty = false;
    }

    /// <summary>Draws each visible layer in order, lowest first, as one draw call per layer.</summary>
    protected override void Render()
    {
        if (!Visible)
            return;

        EnsureBuilt();

        var states = new RenderStates(_sheet);
        states.Transform.Translate(Position.X, Position.Y);
        states.Transform.Scale(TileScale.X, TileScale.Y);

        foreach (var (layer, vertices) in _layerVertices)
        {
            if (_hiddenLayers.Contains(layer))
                continue;

            Draw.Vertices(vertices, states);
        }
    }

    /// <summary>Draws a green outline around each visible tile cell.</summary>
    protected override void DebugRender()
    {
        if (!Visible)
            return;

        foreach (var tile in _tiles.Values)
        {
            if (_hiddenLayers.Contains(tile.Layer))
                continue;

            var left = Position.X + tile.X * TileSize.X * TileScale.X;
            var top = Position.Y + tile.Y * TileSize.Y * TileScale.Y;
            Draw.Rectangle(left, top, TileSize.X * TileScale.X * tile.Scale.X, TileSize.Y * TileScale.Y * tile.Scale.Y, DebugOptions);
        }
    }

    private void EnsureBuilt()
    {
        if (_sheet is null)
            ResolveSheet();
        if (_dirty)
            Rebuild();
    }

    private void ResolveSheet()
        => _sheet = ResourceManager<Texture>.GetResource(Texture);

    // Reused across tiles and frames so debug rendering allocates nothing per tile.
    private static readonly DrawOptions DebugOptions = new()
    {
        FillColor = Color.Transparent,
        OutlineColor = Color.Green,
        OutlineThickness = 1
    };
}
