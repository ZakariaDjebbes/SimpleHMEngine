using Core.Engine;
using Core.Entity;
using SFML.Graphics;

namespace Core.Drawing.UserInterface;

/// <summary>
/// A screen-space UI layer. It owns and drives a set of root <see cref="UiElement"/>s, rendering and
/// hit-testing them against the window's default view so the UI never scrolls with the game camera.
/// Add a <see cref="UiCanvas"/> as a scene field, then build widgets into it via <see cref="Add{T}"/>.
/// </summary>
public class UiCanvas : Component
{
    private readonly HashSet<UiElement> _roots = [];

    /// <summary>Adds a root element to the canvas and returns it for fluent wiring.</summary>
    public T Add<T>(T element) where T : UiElement
    {
        _roots.Add(element);
        return element;
    }

    /// <summary>Removes a root element from the canvas.</summary>
    public void Remove(UiElement element) => _roots.Remove(element);

    private static FloatRect ScreenRect
    {
        get
        {
            var size = GameContext.CurrentWindow.Size;
            return new FloatRect(0, 0, size.X, size.Y);
        }
    }

    /// <summary>
    /// Finds the first root or nested element of type <typeparamref name="T"/> in the canvas.
    /// </summary>
    /// <typeparam name="T">
    ///    The type of the <see cref="UiElement"/> to find.
    /// </typeparam>
    /// <returns>
    ///  The found element.
    /// </returns>
    public T Find<T>() where T : UiElement
    {
        foreach (var root in _roots)
        {
            if (root is T match) return match;
            if (root.FindComponentInChildren<T>() is { } nested) return nested;
        }
        return null;
    }
    
    /// <summary>Measures, anchors and starts every root element.</summary>
    protected override void Start()
    {
        var area = ScreenRect;
        foreach (var root in _roots)
        {
            root.Measure();
            root.ApplyAnchor(area);
            root.OnStart();
        }
    }

    /// <summary>Re-measures and re-anchors each root against the current window, then updates it.</summary>
    protected override void Update()
    {
        var area = ScreenRect;
        foreach (var root in _roots)
        {
            root.Measure();
            root.ApplyAnchor(area);
            root.OnUpdate();
        }
    }

    /// <summary>Renders the roots in screen space, temporarily switching to the window's default view.</summary>
    protected override void Render()
    {
        var window = GameContext.CurrentWindow;
        var previous = new View(window.GetView());
        window.SetView(window.DefaultView);

        foreach (var root in _roots)
            if (root.Visible)
                root.OnRender();

        window.SetView(previous);
    }

    /// <summary>Debug-renders the roots in screen space.</summary>
    protected override void DebugRender()
    {
        var window = GameContext.CurrentWindow;
        var previous = new View(window.GetView());
        window.SetView(window.DefaultView);

        foreach (var root in _roots)
            root.OnDebugRender();

        window.SetView(previous);
    }

    /// <summary>Closes every root element.</summary>
    protected override void Close()
    {
        foreach (var root in _roots)
            root.OnClose();
    }
}
