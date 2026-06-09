using Core.Engine;
using Core.Resources;
using SFML.Graphics;
using SFML.System;

namespace Core.Drawing.UserInterface;

/// <summary>
/// A non-interactive text label. Measures its own size from the rendered glyphs so it can take part
/// in layout containers.
/// </summary>
public class UiText : UiElement
{
    private readonly Text _text = new();

    /// <summary>Creates a non-interactive text label using the theme defaults.</summary>
    public UiText()
    {
        Interactive = false;
        _text.CharacterSize = UiTheme.FontSize;
        _text.FillColor = UiTheme.OnSurface;
    }

    /// <summary>The size of the characters in pixels.</summary>
    public uint CharacterSize
    {
        get => _text.CharacterSize;
        set => _text.CharacterSize = value;
    }

    /// <summary>The text content to display.</summary>
    public string Content
    {
        get => _text.DisplayedString;
        set => _text.DisplayedString = value;
    }

    /// <summary>
    /// Optional source for the content, pulled every frame. Lets a label reflect live state without the
    /// scene having to hold a reference to it.
    /// </summary>
    public Func<string> ContentProvider { get; set; }

    /// <summary>The color of the text.</summary>
    public Color Color
    {
        get => _text.FillColor;
        set => _text.FillColor = value;
    }

    /// <summary>The measured size of the text in pixels.</summary>
    public Vector2f TextSize
    {
        get
        {
            var bounds = _text.GetLocalBounds();
            return new Vector2f(bounds.Width, bounds.Height);
        }
    }

    /// <summary>Resolves the font (theme override or embedded default) and measures the text.</summary>
    protected override void Start()
    {
        _text.Font = UiTheme.FontPath is null
            ? EmbeddedResources.DefaultFont
            : ResourceManager<Font>.GetResource(UiTheme.FontPath);
        Measure();
    }

    /// <summary>Sizes the element to the rendered text bounds.</summary>
    public override void Measure() => Size = TextSize;

    /// <summary>Pulls content from <see cref="ContentProvider"/> (if set) and re-measures.</summary>
    protected override void Update()
    {
        if (ContentProvider is not null) Content = ContentProvider();
        Measure();
    }

    /// <summary>Draws the text, compensating for SFML's glyph bounds offset.</summary>
    protected override void Render()
    {
        if (!Visible) return;

        // SFML glyph bounds carry an internal offset; subtract it so the visual top-left sits at Position.
        var bounds = _text.GetLocalBounds();
        _text.Position = new Vector2f(Position.X - bounds.Left, Position.Y - bounds.Top);
        GameContext.CurrentWindow.Draw(_text);
    }
}
