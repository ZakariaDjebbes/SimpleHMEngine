using Core.Drawing.UserInterface;
using Core.Engine;
using Core.Entity;
using SFML.System;

namespace Demo.Scenes;

/// <summary>
/// The demo hub. Each button switches to a scene that showcases one area of the engine.
/// </summary>
public class MenuScene : Scene
{
    private readonly UiCanvas _canvas = new();

    public override void OnStart()
    {
        var menu = new UiStackPanel
        {
            Orientation = Orientation.Vertical,
            Spacing = 12,
            Alignment = ItemAlignment.Center,
            Padding = new Vector2f(28, 24)
        };

        menu.Add(new UiText { Content = "SimpleHMEngine", CharacterSize = 48, Color = UiTheme.Accent });
        menu.Add(new UiText { Content = "demo gallery", CharacterSize = 18, Color = UiTheme.AccentDark });

        menu.Add(Button("Geometry & ZGeometry", SceneManager.SwitchScene<GeometryDemo>));
        menu.Add(Button("Physics & Collisions", SceneManager.SwitchScene<PhysicsDemo>));
        menu.Add(Button("Drawing Primitives", SceneManager.SwitchScene<DrawingDemo>));
        menu.Add(Button("UI Toolkit", SceneManager.SwitchScene<UiDemo>));
        menu.Add(Button("TileMap & Camera", SceneManager.SwitchScene<TileMapDemo>));
        menu.Add(Button("Quit", () => GameContext.CurrentWindow.Close()));

        var card = new UiCard
        {
            Anchor = Anchor.Center,
            GradientEndColor = UiTheme.SurfaceVariant,
            Padding = new Vector2f(16, 16)
        };
        card.SetContent(menu);

        _canvas.Add(card);
    }

    private static UiButton Button(string label, Action onClick)
    {
        var button = new UiButton { Size = new Vector2f(300, 50), Label = label };
        button.Clicked += onClick;
        return button;
    }
}
