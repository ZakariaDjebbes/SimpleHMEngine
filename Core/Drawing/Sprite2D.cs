using Core.Engine;
using Core.Entity;
using Core.Resources;
using SFML.Graphics;
using SFML.System;

namespace Core.Drawing;

/// <summary>
/// Draws a single textured sprite. Each instance owns its own <see cref="Sprite"/> so multiple
/// sprites can share a texture while having independent positions, colors and sub-rectangles.
/// </summary>
public class Sprite2D(string texture = Sprite2D.DefaultTexture) : Component
{
    private const string DefaultTexture = "Resources/Sprites/default.png";

    private Sprite _sprite;

    /// <summary>Path to the texture this sprite draws.</summary>
    public string Texture { get; set; } = texture;

    /// <summary>Scale applied to the sprite on each axis.</summary>
    public Vector2f Scale { get; set; } = new(1, 1);

    /// <summary>Tint color multiplied into the sprite.</summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>Rotation, in degrees.</summary>
    public float Rotation { get; set; }

    /// <summary>Whether the sprite is drawn.</summary>
    public bool Visible { get; set; } = true;

    /// <summary>Optional sub-rectangle of the texture to draw. When null, the whole texture is used.</summary>
    public IntRect? TextureRect { get; set; }

    /// <summary>The on-screen size of the sprite in pixels (texture rect size times scale).</summary>
    public Vector2f Size { get; private set; }

    /// <summary>Creates a sprite drawing a sub-rectangle of the given texture.</summary>
    /// <param name="texture">Path to the texture.</param>
    /// <param name="rect">The sub-rectangle of the texture to draw.</param>
    public Sprite2D(string texture, IntRect rect) : this(texture) => TextureRect = rect;

    /// <summary>Loads the texture and applies the optional sub-rectangle.</summary>
    protected override void Start()
    {
        _sprite = new Sprite(ResourceManager<Texture>.GetResource(Texture));
        if (TextureRect.HasValue)
            _sprite.TextureRect = TextureRect.Value;
    }

    /// <summary>Draws the sprite with the current transform and color.</summary>
    protected override void Render()
    {
        if (!Visible || _sprite is null) return;

        _sprite.Position = Position;
        _sprite.Color = Color;
        _sprite.Rotation = Rotation;
        _sprite.Scale = Scale;

        var rect = _sprite.TextureRect;
        Size = new Vector2f(rect.Width * Scale.X, rect.Height * Scale.Y);

        GameContext.CurrentWindow.Draw(_sprite);
    }
}
