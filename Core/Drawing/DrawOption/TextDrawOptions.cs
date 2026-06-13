using SFML.Graphics;

namespace Core.Drawing.DrawOption;

/// <summary>
/// Drawing options for text, adding font and character-size choices on top of <see cref="DrawOptions"/>.
/// </summary>
public class TextDrawOptions : DrawOption.DrawOptions
{
    /// <summary>
    /// Gets or sets the path to the font file used for rendering text.
    /// </summary>
    public string FontPath { get; set; }
    
    /// <summary>
    /// Gets or sets the default character size for rendering text, in pixels.
    /// </summary>
    public uint? CharacterSize { get; set; }

    /// <summary>
    /// Creates text options with only a fill color set, letting a <see cref="Color"/> be passed directly
    /// to <c>Draw.Text</c>.
    /// </summary>
    /// <param name="color">The fill (text) color.</param>
    public static implicit operator TextDrawOptions(Color color) => new() { FillColor = color };

    
    /// <summary>
    /// Creates text options with the given font and character size, as the start of a fluent chain.
    /// </summary>
    /// <param name="characterSize">
    /// The character size for rendering text, in pixels.
    /// </param>
    /// <returns>
    ///  The instance of <see cref="TextDrawOptions"/>
    /// </returns>
    public TextDrawOptions WithCharacterSize(uint characterSize)
    {
        CharacterSize = characterSize;
        return this;
    }
}