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

    public string Texture { get; set; } = texture;

    public Vector2f Scale { get; set; } = new(1, 1);

    public Color Color { get; set; } = Color.White;

    public float Rotation { get; set; }

    public bool Visible { get; set; } = true;

    /// <summary>Optional sub-rectangle of the texture to draw. When null, the whole texture is used.</summary>
    public IntRect? TextureRect { get; set; }

    /// <summary>The on-screen size of the sprite in pixels (texture rect size times scale).</summary>
    public Vector2f Size { get; private set; }

    public Sprite2D(string texture, IntRect rect) : this(texture) => TextureRect = rect;

    protected override void Start()
    {
        _sprite = new Sprite(ResourceManager<Texture>.GetResource(Texture));
        if (TextureRect.HasValue)
            _sprite.TextureRect = TextureRect.Value;
    }

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
