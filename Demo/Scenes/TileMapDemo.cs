using Core.Drawing;
using Core.Drawing.DrawOption;
using Core.Drawing.UserInterface;
using Core.Engine;
using Core.Entity;
using Core.Input;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ZGeometry.Primitives.Point;

namespace Demo.Scenes;

public class TileMapDemo : Scene
{
    private const int Columns = 40;
    private const int Rows = 24;
    private const int TilePx = 32;
    private const int Scale = 3;
    private const int AccentLayer = 1;

    private readonly TileMap _map = new(new Vector2D<int>(TilePx, TilePx), "Resources/Sprites/tiles.png")
    {
        TileScale = new Vector2f(Scale, Scale)
    };

    private readonly CameraRig _camera = new();

    private readonly UiCanvas _canvas = new();

    private bool _accentVisible = true;

    public override void OnStart()
    {
        InputManager.BindAction(Keyboard.Key.Escape, ActionType.Pressed, SceneManager.SwitchScene<MenuScene>);
        InputManager.BindAction(Keyboard.Key.B, ActionType.Pressed, () => GameContext.IsDebugMode = !GameContext.IsDebugMode);
        InputManager.BindAction(Keyboard.Key.L, ActionType.Pressed, () =>
        {
            _accentVisible = !_accentVisible;
            _map.SetLayerVisible(AccentLayer, _accentVisible);
        });

        _map.AddTiles(BuildFloor());
        _map.AddTiles(BuildAccents());

        // Centre the camera on the map using its reported pixel size, rather than recomputing it by hand.
        _camera.Position = _map.MapPixelSize / 2f;

        var panel = new UiStackPanel { Spacing = 6, Alignment = ItemAlignment.Start };
        panel.Add(new UiText { Content = "TileMap & Camera", CharacterSize = 24, Color = UiTheme.Accent });
        panel.Add(new UiText { Content = "WASD / arrows: pan    +/-: zoom    L: toggle accents    B: debug grid    Esc: menu", CharacterSize = 15 });

        var card = new UiCard { Anchor = Anchor.TopLeft, Margin = new Vector2f(12, 12), Padding = new Vector2f(12, 10) };
        card.SetContent(panel);
        _canvas.Add(card);
    }

    // Layer 0: a checkerboard floor from two cells of the sheet so the map has visible structure.
    private static IEnumerable<Tile2D> BuildFloor()
    {
        for (var x = 0; x < Columns; x++)
        for (var y = 0; y < Rows; y++)
        {
            var index = (x + y) % 2 == 0 ? new Vector2D<int>(0, 0) : new Vector2D<int>(1, 0);
            yield return new Tile2D(index, x, y);
        }
    }

    // Layer 1: a tinted, horizontally flipped accent along the diagonal, drawn on top of the floor.
    private static IEnumerable<Tile2D> BuildAccents()
    {
        var accent = new Color(255, 220, 120, 210);
        for (var i = 0; i < Math.Min(Columns, Rows); i++)
            yield return new Tile2D(new Vector2D<int>(1, 0), i, i, AccentLayer)
            {
                Tint = accent,
                FlipX = true
            };
    }
}

public class CameraRig : Component
{
    private const float Speed = 400f;
    private const float Zoom = 0.01f;
    private readonly Camera _camera = new() { Smoothing = 0.0025f };
    private Vector2f _dir;

    protected override void Start()
    {
        InputManager.BindAction(Keyboard.Key.Z, ActionType.Held, () => _dir.Y -= 1);
        InputManager.BindAction(Keyboard.Key.S, ActionType.Held, () => _dir.Y += 1);
        InputManager.BindAction(Keyboard.Key.Q, ActionType.Held, () => _dir.X -= 1);
        InputManager.BindAction(Keyboard.Key.D, ActionType.Held, () => _dir.X += 1);
        InputManager.BindAction(Keyboard.Key.Add, ActionType.Held, () => _camera.Zoom += Zoom);
        InputManager.BindAction(Keyboard.Key.Subtract, ActionType.Held, () => _camera.Zoom -= Zoom);

        AddComponent(_camera);
    }


    protected override void FixedUpdate()
    {
        Position += _dir * Speed * GameContext.FixedDeltaTime;
        _dir = new Vector2f();
    }
}
