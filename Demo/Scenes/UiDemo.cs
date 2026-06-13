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
/// Showcases the UI toolkit: cards, stack panels, text, a slider, a progress bar, a checkbox and
/// buttons — all wired together. The slider drives a live preview circle and the progress bar.
/// </summary>
public class UiDemo : Scene
{
    private readonly UiCanvas _canvas = new();

    private float _radius = 60f;
    private bool _filled = true;

    public override void OnStart()
    {
        InputManager.BindAction(Keyboard.Key.Escape, ActionType.Pressed, SceneManager.SwitchScene<MenuScene>);

        var panel = new UiStackPanel { Spacing = 12, Alignment = ItemAlignment.Start };
        panel.Add(new UiText { Content = "UI Toolkit", CharacterSize = 24, Color = UiTheme.Accent });

        panel.Add(new UiText { CharacterSize = 15, ContentProvider = () => $"Preview radius: {_radius:0}" });
        var slider = new UiSlider { Min = 20, Max = 160, Size = new Vector2f(240, 24) };
        slider.ValueChanged += v => _radius = v;
        slider.SetValue(_radius);
        panel.Add(slider);

        panel.Add(new UiText { Content = "Radius as a progress bar", CharacterSize = 15 });
        panel.Add(new UiProgressBar
        {
            Size = new Vector2f(240, 22),
            Max = 160,
            ShowLabel = true,
            ValueProvider = () => _radius
        });

        var filled = new UiCheckbox { Label = "Filled preview", Checked = _filled };
        filled.CheckedChanged += c => _filled = c;
        panel.Add(filled);

        var debug = new UiCheckbox { Label = "Debug render", Checked = GameContext.IsDebugMode };
        debug.CheckedChanged += c => GameContext.IsDebugMode = c;
        panel.Add(debug);

        var reset = new UiButton { Size = new Vector2f(240, 44), Label = "Reset radius" };
        reset.Clicked += () => slider.SetValue(60);
        panel.Add(reset);

        var menu = new UiButton { Size = new Vector2f(240, 44), Label = "Back to menu" };
        menu.Clicked += SceneManager.SwitchScene<MenuScene>;
        panel.Add(menu);

        var card = new UiCard
        {
            Anchor = Anchor.TopLeft,
            Margin = new Vector2f(20, 20),
            Padding = new Vector2f(16, 14),
            GradientEndColor = UiTheme.SurfaceVariant
        };
        card.SetContent(panel);
        _canvas.Add(card);
    }

    public override void OnRender()
    {
        var center = new Vector2f(GameContext.CurrentWindow.Size.X * 0.66f, GameContext.CurrentWindow.Size.Y * 0.5f);
        Draw.Circle(center, _radius, new DrawOptions
        {
            FillColor = _filled ? Palette.LightAqua : Color.Transparent,
            OutlineColor = Palette.DarkAqua,
            OutlineThickness = 3
        });
    }
}
