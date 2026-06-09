using Core.Collider;
using Core.Drawing;
using Core.Drawing.UserInterface;
using Core.Engine;
using Core.Entity;
using Core.Input;
using NoGfx.Demo;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace NoGfx.Scenes;

/// <summary>
/// Showcases the collision system: a flock of <see cref="BouncingBall"/>s moving with fixed-update
/// physics inside a bordered area. Each ball turns red while a collider reports contact, driven by
/// the engine's CollisionEnter / CollisionExit events.
/// </summary>
public class PhysicsDemo : Scene
{
    private static readonly Vector2f AreaMin = new(60, 110);
    private static readonly Vector2f AreaMax = new(1220, 860);

    // Auto-registered: the scene reflects over its fields and wires up every Component it finds.
    private readonly List<BouncingBall> _balls =
    [
        new() { Position = new Vector2f(200, 300), Velocity = new Vector2f(150, 95),  Radius = 22, AreaMin = AreaMin, AreaMax = AreaMax },
        new() { Position = new Vector2f(450, 420), Velocity = new Vector2f(-120, 140), Radius = 18, AreaMin = AreaMin, AreaMax = AreaMax },
        new() { Position = new Vector2f(700, 260), Velocity = new Vector2f(170, -110), Radius = 26, AreaMin = AreaMin, AreaMax = AreaMax },
        new() { Position = new Vector2f(920, 500), Velocity = new Vector2f(-160, -90), Radius = 16, AreaMin = AreaMin, AreaMax = AreaMax },
        new() { Position = new Vector2f(1050, 320), Velocity = new Vector2f(-130, 150), Radius = 20, AreaMin = AreaMin, AreaMax = AreaMax },
        new() { Position = new Vector2f(320, 650), Velocity = new Vector2f(140, -130), Radius = 24, AreaMin = AreaMin, AreaMax = AreaMax }
    ];

    private readonly UiCanvas _canvas = new();

    public override void OnStart()
    {
        InputManager.BindAction(Keyboard.Key.Escape, ActionType.Pressed, SceneManager.SwitchScene<MenuScene>);
        InputManager.BindAction(Keyboard.Key.B, ActionType.Pressed, () => GameContext.IsDebugMode = !GameContext.IsDebugMode);

        var panel = new UiStackPanel { Spacing = 6, Alignment = ItemAlignment.Start };
        panel.Add(new UiText { Content = "Physics & Collisions", CharacterSize = 24, Color = UiTheme.Accent });
        panel.Add(new UiText { Content = "Balls bounce in the box and flash on contact. B = debug colliders. Esc = menu.", CharacterSize = 15 });
        panel.Add(new UiText
        {
            CharacterSize = 15,
            Color = UiTheme.AccentDark,
            ContentProvider = () => $"Colliders in scene: {GetComponents<CircleCollider>().Count()}"
        });

        var card = new UiCard { Anchor = Anchor.TopLeft, Margin = new Vector2f(12, 12), Padding = new Vector2f(12, 10) };
        card.SetContent(panel);
        _canvas.Add(card);
    }

    public override void OnRender()
    {
        Draw.Rectangle(AreaMin.X, AreaMin.Y, AreaMax.X - AreaMin.X, AreaMax.Y - AreaMin.Y,
            new DrawOptions { FillColor = Color.Transparent, OutlineColor = Palette.AquaGray, OutlineThickness = 1 });
    }
}
