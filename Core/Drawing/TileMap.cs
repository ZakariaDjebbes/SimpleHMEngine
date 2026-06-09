using Core.Engine;
using Core.Entity;
using Core.Resources;
using SFML.Graphics;
using SFML.System;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;

namespace Core.Drawing;

/// <summary>
/// Renders a grid of tiles cut from a single texture sheet in one draw call using a
/// <see cref="VertexArray"/>. Each <see cref="Tile"/> maps a sheet cell (<see cref="Tile.Index"/>)
/// to a grid position (<see cref="Tile.X"/>, <see cref="Tile.Y"/>) on the map. The map's world
/// origin is the component's <see cref="Component.Position"/>.
/// </summary>
public class TileMap(Vector2D<int> tileSize, string texture = TileMap.DefaultTexture) : Component
{
    private const string DefaultTexture = "Resources/Sprites/default.png";

    /// <summary>The texture sheet the tiles are cut from.</summary>
    public string Texture { get; init; } = texture;

    /// <summary>The pixel size of a single source tile in the sheet (e.g. 16x16).</summary>
    public Vector2D<int> TileSize { get; } = tileSize;

    /// <summary>On-screen scale applied to each tile.</summary>
    public Vector2f TileScale { get; set; } = new(1, 1);

    /// <summary>The tiles that make up the map.</summary>
    public List<Tile> Tiles { get; set; } = [];

    /// <summary>Whether the map is drawn.</summary>
    public bool Visible { get; set; } = true;

    /// <summary>The on-screen size of a single tile, in pixels.</summary>
    public Vector2f ScaledTileSize => new(TileSize.X * TileScale.X, TileSize.Y * TileScale.Y);

    private VertexArray _vertices;
    private Texture _sheet;

    protected override void Start() => Rebuild();

    /// <summary>
    /// (Re)builds the vertex array from the current <see cref="Tiles"/>.
    /// Call this after mutating <see cref="Tiles"/>, <see cref="Position"/> or <see cref="TileScale"/>.
    /// </summary>
    public void Rebuild()
    {
        _sheet = ResourceManager<Texture>.GetResource(Texture);
        _vertices = new VertexArray(PrimitiveType.Quads, (uint)(Tiles.Count * 4));

        var tileWidth = ScaledTileSize.X;
        var tileHeight = ScaledTileSize.Y;

        for (var i = 0; i < Tiles.Count; i++)
        {
            var tile = Tiles[i];

            var left = Position.X + tile.X * tileWidth;
            var top = Position.Y + tile.Y * tileHeight;
            var right = left + tileWidth;
            var bottom = top + tileHeight;

            var texLeft = tile.Index.X * TileSize.X;
            var texTop = tile.Index.Y * TileSize.Y;
            var texRight = texLeft + TileSize.X;
            var texBottom = texTop + TileSize.Y;

            var v = (uint)(i * 4);
            _vertices[v + 0] = new Vertex(new Vector2f(left, top), new Vector2f(texLeft, texTop));
            _vertices[v + 1] = new Vertex(new Vector2f(right, top), new Vector2f(texRight, texTop));
            _vertices[v + 2] = new Vertex(new Vector2f(right, bottom), new Vector2f(texRight, texBottom));
            _vertices[v + 3] = new Vertex(new Vector2f(left, bottom), new Vector2f(texLeft, texBottom));
        }
    }

    protected override void Render()
    {
        if (!Visible || _vertices is null) return;

        GameContext.CurrentWindow.Draw(_vertices, new RenderStates(_sheet));
    }

    protected override void DebugRender()
    {
        if (!Visible) return;

        var size = ScaledTileSize;
        foreach (var tile in Tiles)
        {
            var rect = new Rectangle<float>(
                (Position.X + tile.X * size.X, Position.Y + tile.Y * size.Y),
                (size.X, size.Y));
            rect.DrawSelf(new DrawOptions { FillColor = Color.Transparent, OutlineColor = Color.Green, OutlineThickness = 1 });
        }
    }
}
