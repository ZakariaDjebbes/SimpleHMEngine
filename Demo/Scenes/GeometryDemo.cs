using Core.Drawing;
using Core.Drawing.DrawOption;
using Core.Drawing.UserInterface;
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

namespace Demo.Scenes;

/// <summary>
/// Showcases the ZGeometry library: a mouse-controlled probe tested against a rectangle, triangle,
/// circle and line via Overlaps / Contains / Intersects. The probe shape (circle, rectangle or
/// triangle) and its size are chosen from the UI. Shapes glow on overlap, turn white when fully
/// contained, and boundary intersection points are plotted live.
/// </summary>
public class GeometryDemo : Scene
{
    private enum ProbeShape { Circle, Rectangle, Triangle }

    private static readonly Color IdleOutline = Palette.LightAqua;
    private static readonly Color OverlapFill = new(220, 90, 90, 110);
    private static readonly Color OverlapOutline = Palette.LightRed;
    private static readonly Color ContainFill = new(120, 220, 120, 140);
    private static readonly Color HitColor = Palette.DeepRed;
    private static readonly Color ContainOutline = Palette.DarkRed;

    private static readonly DrawOptions ProbeStyle = new()
    {
        FillColor = new Color(255, 255, 255, 25), OutlineColor = Color.White, OutlineThickness = 2
    };

    private readonly Rectangle<float> _rectangle = Rectangle<float>.Create((140, 150), (190, 130));
    private readonly Triangle<float> _triangle = Triangle<float>.Create((470, 150), (390, 300), (590, 300));
    private readonly Circle<float> _circle = Circle<float>.Create((760, 230), 70);
    private readonly Line<float> _line = Line<float>.Create((900, 140), (900, 360));

    private ProbeShape _shape = ProbeShape.Circle;
    private float _probeSize = 45.0f;

    private readonly List<Vector2D<float>> _hits = [];
    private bool[] _overlap = new bool[4];
    private bool[] _contain = new bool[4];
    private int _overlaps;
    private int _contained;
    private Action _drawProbe;

    private readonly UiCanvas _canvas = new();

    public override void OnStart()
    {
        InputManager.BindAction(Keyboard.Key.Escape, ActionType.Pressed, SceneManager.SwitchScene<MenuScene>);

        var panel = new UiStackPanel { Spacing = 6, Alignment = ItemAlignment.Start };
        panel.Add(new UiText { Content = "Geometry & ZGeometry", CharacterSize = 24, Color = UiTheme.Accent });
        panel.Add(new UiText { Content = "Move the mouse to drag the probe. Esc = menu.", CharacterSize = 15 });
        panel.Add(new UiText
        {
            CharacterSize = 15,
            Color = UiTheme.AccentDark,
            ContentProvider = () => $"Overlapping {_overlaps}/4    Contained {_contained}/4    Intersections {_hits.Count}"
        });

        panel.Add(new UiText { CharacterSize = 15, ContentProvider = () => $"Probe shape: {_shape}" });
        var shapeButton = new UiButton { Size = new Vector2f(240, 40), Label = "Cycle shape" };
        shapeButton.Clicked += () => _shape = Next(_shape);
        panel.Add(shapeButton);

        panel.Add(new UiText { CharacterSize = 15, ContentProvider = () => $"Probe size: {_probeSize:0}" });
        var sizeSlider = new UiSlider { Min = 10, Max = 260, Size = new Vector2f(240, 24) };
        sizeSlider.ValueChanged += v => _probeSize = v;
        sizeSlider.SetValue(_probeSize);
        panel.Add(sizeSlider);
        
        var card = new UiCard { Anchor = Anchor.TopLeft, Margin = new Vector2f(200, 500), Padding = new Vector2f(12, 10) };
        card.SetContent(panel);
        _canvas.Add(card);
    }

    public override void OnUpdate()
    {
        var m = MouseUtils.GetCurrentViewMousePosition();

        switch (_shape)
        {
            case ProbeShape.Rectangle:
                Evaluate(Rectangle<float>.Create((m.X - _probeSize, m.Y - _probeSize), (2 * _probeSize, 2 * _probeSize)));
                break;
            case ProbeShape.Triangle:
                Evaluate(Triangle<float>.Create( (m.X, m.Y - _probeSize), (m.X - _probeSize, m.Y + _probeSize), (m.X + _probeSize, m.Y + _probeSize)));
                break;
            default:
                Evaluate(Circle<float>.Create((m.X, m.Y), _probeSize));
                break;
        }
    }

    public override void OnRender()
    {
        _rectangle.DrawSelf(StyleFor(_overlap[0], _contain[0]));
        _triangle.DrawSelf(StyleFor(_overlap[1], _contain[1]));
        _circle.DrawSelf(StyleFor(_overlap[2], _contain[2]));
        _line.DrawSelf(new DrawOptions { FillColor = LineColor(_overlap[3], _contain[3]) });

        _drawProbe?.Invoke();

        _hits.ForEach(p => Draw.Circle(new Vector2f(p.X, p.Y), 3,
            new DrawOptions { FillColor = HitColor, OutlineColor = HitColor, OutlineThickness = 1 }));
    }

    private void Evaluate(Circle<float> p) => Evaluate(
        p.Overlaps(_rectangle), p.Overlaps(_triangle), p.Overlaps(_circle), p.Overlaps(_line),
        p.Contains(_rectangle), p.Contains(_triangle), p.Contains(_circle), p.Contains(_line),
        p.Intersects(_rectangle), p.Intersects(_triangle), p.Intersects(_circle), p.Intersects(_line),
        () => p.DrawSelf(ProbeStyle));

    private void Evaluate(Rectangle<float> p) => Evaluate(
        p.Overlaps(_rectangle), p.Overlaps(_triangle), p.Overlaps(_circle), p.Overlaps(_line),
        p.Contains(_rectangle), p.Contains(_triangle), p.Contains(_circle), p.Contains(_line),
        p.Intersects(_rectangle), p.Intersects(_triangle), p.Intersects(_circle), p.Intersects(_line),
        () => p.DrawSelf(ProbeStyle));

    private void Evaluate(Triangle<float> p) => Evaluate(
        p.Overlaps(_rectangle), p.Overlaps(_triangle), p.Overlaps(_circle), p.Overlaps(_line),
        p.Contains(_rectangle), p.Contains(_triangle), p.Contains(_circle), p.Contains(_line),
        p.Intersects(_rectangle), p.Intersects(_triangle), p.Intersects(_circle), p.Intersects(_line),
        () => p.DrawSelf(ProbeStyle));

    private void Evaluate(
        bool oRect, bool oTri, bool oCir, bool oLine,
        bool cRect, bool cTri, bool cCir, bool cLine,
        IEnumerable<Vector2D<float>> hRect, IEnumerable<Vector2D<float>> hTri,
        IEnumerable<Vector2D<float>> hCir, IEnumerable<Vector2D<float>> hLine,
        Action drawProbe)
    {
        _overlap = [oRect, oTri, oCir, oLine];
        _contain = [cRect, cTri, cCir, cLine];
        _overlaps = _overlap.Count(b => b);
        _contained = _contain.Count(b => b);

        _hits.Clear();
        _hits.AddRange(hRect);
        _hits.AddRange(hTri);
        _hits.AddRange(hCir);
        _hits.AddRange(hLine);

        _drawProbe = drawProbe;
    }

    private static ProbeShape Next(ProbeShape shape) => shape switch
    {
        ProbeShape.Circle => ProbeShape.Rectangle,
        ProbeShape.Rectangle => ProbeShape.Triangle,
        _ => ProbeShape.Circle
    };

    private static DrawOptions StyleFor(bool overlap, bool contained)
    {
        if (contained)
            return new DrawOptions { FillColor = ContainFill, OutlineColor = ContainOutline, OutlineThickness = 2 };
        if (overlap)
            return new DrawOptions { FillColor = OverlapFill, OutlineColor = OverlapOutline, OutlineThickness = 2 };

        return new DrawOptions { FillColor = Color.Transparent, OutlineColor = IdleOutline, OutlineThickness = 2 };
    }

    private static Color LineColor(bool overlap, bool contained)
        => contained ? ContainOutline : overlap ? OverlapOutline : IdleOutline;
}
