using Core.Drawing;
using Core.Drawing.UserInterface;
using Core.Engine;
using Core.Entity;
using Core.Input;
using SFML.System;
using SFML.Window;
using ZGeometry.Primitives.Point;

namespace Demo.Scenes;

/// <summary>
/// Showcases the <see cref="TileMap"/> and <see cref="Camera"/>: a tiled map built from a sprite
/// sheet in a single draw call, with a camera rig the player pans around using WASD / arrow keys.
/// </summary>
public class TileMapDemo : Scene
{
    private const int Columns = 40;
    private const int Rows = 24;
    private const int TilePx = 16;
    private const int Scale = 3;

    private readonly TileMap _map = new(new Vector2D<int>(TilePx, TilePx), "Resources/Sprites/catacomb.png")
    {
        TileScale = new Vector2f(Scale, Scale),
        Tiles = BuildTiles()
    };

    private readonly CameraRig _camera = new();

    private readonly UiCanvas _canvas = new();

    public override void OnStart()
    {
        InputManager.BindAction(Keyboard.Key.Escape, ActionType.Pressed, SceneManager.SwitchScene<MenuScene>);
        InputManager.BindAction(Keyboard.Key.B, ActionType.Pressed, () => GameContext.IsDebugMode = !GameContext.IsDebugMode);

        // Start the camera near the middle of the map.
        _camera.Position = new Vector2f(Columns * TilePx * Scale / 2f, Rows * TilePx * Scale / 2f);

        var panel = new UiStackPanel { Spacing = 6, Alignment = ItemAlignment.Start };
        panel.Add(new UiText { Content = "TileMap & Camera", CharacterSize = 24, Color = UiTheme.Accent });
        panel.Add(new UiText { Content = "WASD / arrows: pan camera    B: debug grid    Esc: menu", CharacterSize = 15 });

        var card = new UiCard { Anchor = Anchor.TopLeft, Margin = new Vector2f(12, 12), Padding = new Vector2f(12, 10) };
        card.SetContent(panel);
        _canvas.Add(card);
    }

    private static List<Tile> BuildTiles()
    {
        var tiles = new List<Tile>(Columns * Rows);
        for (var x = 0; x < Columns; x++)
        for (var y = 0; y < Rows; y++)
        {
            // Simple checkerboard from two cells of the sheet so the map has visible structure.
            var index = (x + y) % 2 == 0 ? new Vector2D<int>(0, 0) : new Vector2D<int>(1, 0);
            tiles.Add(new Tile(index, x, y));
        }
        return tiles;
    }
}

/// <summary>
/// A movable component carrying a <see cref="Camera"/>. The camera follows this rig, so moving the
/// rig pans the view. Demonstrates attaching a Camera to any game object.
/// </summary>
public class CameraRig : Component
{
    private const float Speed = 400f;
    private readonly Camera _camera = new() { Smoothing = 0.0025f };

    protected override void Start() => AddComponent(_camera);

    protected override void FixedUpdate()
    {
        var dir = new Vector2f();
        if (Keyboard.IsKeyPressed(Keyboard.Key.W) || Keyboard.IsKeyPressed(Keyboard.Key.Up)) dir.Y -= 1;
        if (Keyboard.IsKeyPressed(Keyboard.Key.S) || Keyboard.IsKeyPressed(Keyboard.Key.Down)) dir.Y += 1;
        if (Keyboard.IsKeyPressed(Keyboard.Key.A) || Keyboard.IsKeyPressed(Keyboard.Key.Left)) dir.X -= 1;
        if (Keyboard.IsKeyPressed(Keyboard.Key.D) || Keyboard.IsKeyPressed(Keyboard.Key.Right)) dir.X += 1;

        Position += dir * Speed * GameContext.FixedDeltaTime;
    }
}
