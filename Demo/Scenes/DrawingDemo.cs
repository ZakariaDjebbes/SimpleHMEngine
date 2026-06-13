using Core.Drawing;
using Core.Drawing.DrawOption;
using Core.Drawing.UserInterface;
using Core.Engine;
using Core.Entity;
using Core.Input;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Demo.Scenes;

/// <summary>
/// Showcases the immediate-mode <see cref="Draw"/> helper: circles, rectangles, triangles, lines,
/// a gradient panel, and text rendered with the engine's font — including fill, outline, rotation
/// and opacity options.
/// </summary>
public class DrawingDemo : Scene
{
    private readonly UiCanvas _canvas = new();

    public override void OnStart()
    {
        InputManager.BindAction(Keyboard.Key.Escape, ActionType.Pressed, SceneManager.SwitchScene<MenuScene>);

        var panel = new UiStackPanel { Spacing = 6, Alignment = ItemAlignment.Start };
        panel.Add(new UiText { Content = "Drawing Primitives", CharacterSize = 24, Color = UiTheme.Accent });
        panel.Add(new UiText { Content = "Everything below is drawn each frame via Draw.*  Esc = menu.", CharacterSize = 15 });

        var card = new UiCard { Anchor = Anchor.TopLeft, Margin = new Vector2f(12, 12), Padding = new Vector2f(12, 10) };
        card.SetContent(panel);
        _canvas.Add(card);
    }

    public override void OnRender()
    {
        const float top = 160f;

        // Circles: fill, outline, semi-transparent.
        Draw.Circle(new Vector2f(140, top + 40), 45, new DrawOptions { FillColor = Palette.LightAqua });
        Draw.Circle(new Vector2f(260, top + 40), 45, new DrawOptions { FillColor = Color.Transparent, OutlineColor = Palette.LightAqua, OutlineThickness = 4 });
        Draw.Circle(new Vector2f(380, top + 40), 45, new DrawOptions { FillColor = Palette.LightRed, Opacity = 0.4f });
        Label("Circles", 110, top + 100);

        // Rectangles: plain, outlined, rotated.
        Draw.Rectangle(520, top, 90, 80, new DrawOptions { FillColor = Palette.Beige });
        Draw.Rectangle(640, top, 90, 80, new DrawOptions { FillColor = Color.Transparent, OutlineColor = Palette.Brown, OutlineThickness = 4 });
        Draw.Rectangle(760, top + 10, 80, 60, new DrawOptions { FillColor = Palette.DarkAqua, Rotation = 20f });
        Label("Rectangles", 540, top + 100);

        // Triangle and lines.
        Draw.Triangle(new Vector2f(960, top + 80), new Vector2f(1060, top + 80), new Vector2f(1010, top),
            new DrawOptions { FillColor = Palette.AquaGray, OutlineColor = Color.White, OutlineThickness = 2 });
        Label("Triangle", 970, top + 100);

        for (var i = 0; i < 6; i++)
            Draw.Line(new Vector2f(140, top + 220 + i * 14), new Vector2f(440, top + 180 + i * 22),
                new DrawOptions { FillColor = Palette.LightAqua });
        Label("Lines", 250, top + 320);

        // Gradient panel (used by UI cards under the hood).
        var gradient = new GradiantRect(new Vector2f(300, 130), Palette.DarkAqua, Palette.LightRed)
        {
            Position = new Vector2f(560, top + 180)
        };
        GameContext.CurrentWindow.Draw(gradient);
        Label("Gradient", 660, top + 320);

        // Text samples.
        Draw.Text("Embedded font", new Vector2f(960, top + 200), new TextDrawOptions { FillColor = Color.White, CharacterSize = 28 });
        Draw.Text("with outline", new Vector2f(960, top + 245),
            new TextDrawOptions { FillColor = Palette.LightAqua, OutlineColor = Color.Black, OutlineThickness = 2, CharacterSize = 28 });

        // Sprite: no texture set, so Draw.Sprite falls back to the engine's default texture. Tinted,
        // scaled and rotated through the fluent SpriteDrawOptions — no Scene/Component needed.
        Draw.Sprite(new Vector2f(200, top + 360),
            new SpriteDrawOptions().WithScale(1.2f).WithRotation(12f).WithTint(Palette.Beige));
        Label("Sprite (default texture)", 170, top + 460);

        // Transform stack + anchor: the scope translates/rotates a local frame and centers the squares
        // on their positions. Pop is automatic at the end of the using block, so nothing leaks.
        using (Draw.Pushed().Translate(560, top + 400).Rotate(20f).Mode(Anchor.Center))
        {
            for (var i = 0; i < 5; i++)
                Draw.Rectangle(i * 36, 0, 28, 28,
                    new DrawOptions { FillColor = i % 2 == 0 ? Palette.LightAqua : Palette.LightRed });
        }
        Label("Transform + anchor", 560, top + 460);
    }

    private static void Label(string text, float x, float y)
        => Draw.Text(text, new Vector2f(x, y), new TextDrawOptions { FillColor = Palette.AquaGray, CharacterSize = 16 });
}
