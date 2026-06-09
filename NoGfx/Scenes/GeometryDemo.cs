using Core.Drawing;
using Core.Drawing.UserInterface;
using Core.Engine;
using Core.Entity;
using Core.Input;
using Core.Utils;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ZGeometry.Logic;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Triangle;

namespace NoGfx.Scenes;

/// <summary>
/// Showcases the ZGeometry library: a mouse-controlled probe circle tested against a rectangle,
/// triangle, circle and line via Overlaps / Contains / Intersects. Shapes glow on overlap, turn
/// white when fully contained, and boundary intersection points are plotted live.
/// </summary>
public class GeometryDemo : Scene
{
    private static readonly Color IdleOutline = Palette.LightAqua;
    private static readonly Color OverlapFill = new(220, 90, 90, 110);
    private static readonly Color OverlapOutline = Palette.LightRed;
    private static readonly Color ContainFill = new(120, 220, 120, 140);
    private static readonly Color HitColor = Palette.DeepRed;

    private readonly Rectangle<float> _rectangle = Rectangle<float>.Create((140, 150), (190, 130));
    private readonly Triangle<float> _triangle = Triangle<float>.Create((470, 150), (390, 300), (590, 300));
    private readonly Circle<float> _circle = Circle<float>.Create((760, 230), 70);
    private readonly Line<float> _line = Line<float>.Create((900, 140), (900, 360));

    private float _probeRadius = 45.0f;
    private Circle<float> _probe;
    private readonly List<Vector2D<float>> _hits = [];
    private int _overlaps;
    private int _contained;

    private readonly UiCanvas _canvas = new();

    public override void OnStart()
    {
        InputManager.BindAction(Keyboard.Key.Escape, ActionType.Pressed, SceneManager.SwitchScene<MenuScene>);

        var panel = new UiStackPanel { Spacing = 6, Alignment = ItemAlignment.Start };
        panel.Add(new UiText { Content = "Geometry & ZGeometry", CharacterSize = 24, Color = UiTheme.Accent });
        panel.Add(new UiText { Content = "Move the mouse to drag the probe. Scroll to resize. Esc = menu.", CharacterSize = 15 });
        panel.Add(new UiText
        {
            CharacterSize = 15,
            Color = UiTheme.AccentDark,
            ContentProvider = () => $"Overlapping {_overlaps}/4    Contained {_contained}/4    Intersections {_hits.Count}"
        });

        var card = new UiCard { Anchor = Anchor.TopLeft, Margin = new Vector2f(12, 12), Padding = new Vector2f(12, 10) };
        card.SetContent(panel);
        _canvas.Add(card);

        InputManager.BindAction(Keyboard.Key.Up, ActionType.Held, () => _probeRadius = MathF.Min(_probeRadius + 1.5f, 200));
        InputManager.BindAction(Keyboard.Key.Down, ActionType.Held, () => _probeRadius = MathF.Max(_probeRadius - 1.5f, 10));
    }

    public override void OnUpdate()
    {
        var mouse = MouseUtils.GetCurrentViewMousePosition();
        _probe = Circle<float>.Create((mouse.X, mouse.Y), _probeRadius);

        _hits.Clear();
        _hits.AddRange(_probe.Intersects(_rectangle));
        _hits.AddRange(_probe.Intersects(_triangle));
        _hits.AddRange(_probe.Intersects(_circle));
        _hits.AddRange(_probe.Intersects(_line));

        _overlaps = Count(_probe.Overlaps(_rectangle), _probe.Overlaps(_triangle), _probe.Overlaps(_circle), _probe.Overlaps(_line));
        _contained = Count(_probe.Contains(_rectangle), _probe.Contains(_triangle), _probe.Contains(_circle), _probe.Contains(_line));
    }

    public override void OnRender()
    {
        _rectangle.DrawSelf(StyleFor(_probe.Overlaps(_rectangle), _probe.Contains(_rectangle)));
        _triangle.DrawSelf(StyleFor(_probe.Overlaps(_triangle), _probe.Contains(_triangle)));
        _circle.DrawSelf(StyleFor(_probe.Overlaps(_circle), _probe.Contains(_circle)));
        _line.DrawSelf(new DrawOptions { FillColor = LineColor(_probe.Overlaps(_line), _probe.Contains(_line)) });

        Draw.Circle(new Vector2f(_probe.Center.X, _probe.Center.Y), _probeRadius,
            new DrawOptions { FillColor = new Color(255, 255, 255, 25), OutlineColor = Color.White, OutlineThickness = 2 });

        _hits.ForEach(p => Draw.Circle(new Vector2f(p.X, p.Y), 3,
            new DrawOptions { FillColor = HitColor, OutlineColor = HitColor, OutlineThickness = 1 }));
    }

    private static DrawOptions StyleFor(bool overlap, bool contained)
    {
        if (contained)
            return new DrawOptions { FillColor = ContainFill, OutlineColor = Color.White, OutlineThickness = 2 };
        if (overlap)
            return new DrawOptions { FillColor = OverlapFill, OutlineColor = OverlapOutline, OutlineThickness = 2 };

        return new DrawOptions { FillColor = Color.Transparent, OutlineColor = IdleOutline, OutlineThickness = 2 };
    }

    private static Color LineColor(bool overlap, bool contained)
        => contained ? Color.White : overlap ? OverlapOutline : IdleOutline;

    private static int Count(params bool[] flags) => flags.Count(flag => flag);
}
