using Core.Engine;
using Core.Entity;
using Core.Resources;
using SFML.Graphics;
using SFML.System;

namespace Core.Drawing;

/// <summary>
/// Draws a single textured sprite. Each instance owns its own <see cref="SFML.Graphics.Sprite"/> so multiple
/// sprites can share a texture while having independent positions, colors and sub-rectangles.
/// </summary>
public class Sprite2D : Component
{
    /// <summary>
    /// Creates a <see cref="Sprite2D"/> using the incoming texture.
    /// </summary>
    /// <param name="texture">
    /// The texture to use for the sprite. The caller is responsible for ensuring this texture remains valid for the lifetime of the sprite.
    /// </param>
    public Sprite2D(Texture texture)
    {
        Sprite = new Sprite(texture);
    }

    /// <summary>
    /// Creates a <see cref="Sprite2D"/> by loading the texture from the given path.
    /// </summary>
    /// <param name="texturePath">
    ///  The path to the texture resource. The texture is loaded and cached by the
    ///  <see cref="ResourceManager{T}"/>; the cached instance is shared, not copied, so many sprites
    ///  drawing the same path reference one GPU texture.
    /// </param>
    public Sprite2D(string texturePath)
    {
        TexturePath = texturePath;
        Sprite = new Sprite(ResourceManager<Texture>.GetResource(texturePath));
    }

    /// <summary>
    /// Creates a <see cref="Sprite2D"/> with a default texture. This can be used as a placeholder when the actual texture is not yet known or available.
    /// </summary>
    public Sprite2D()
    {
        var texture = EmbeddedResources.DefaultTexture;
        Sprite = new Sprite(texture);
    }

    /// <summary>The underlying SFML sprite this component draws. Fixed at construction.</summary>
    public Sprite Sprite { get; private init; }

    /// <summary>
    /// Path the texture was loaded from, or <c>null</c> when the sprite was constructed from a
    /// <see cref="SFML.Graphics.Texture"/> directly or uses the default texture. The texture is fixed
    /// at construction, so this is informational only.
    /// </summary>
    public string TexturePath { get; private init; }

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
        if (TextureRect.HasValue)
            Sprite.TextureRect = TextureRect.Value;
    }

    /// <summary>Draws the sprite with the current transform and color.</summary>
    protected override void Render()
    {
        if (!Visible || Sprite is null) return;

        Sprite.Position = Position;
        Sprite.Color = Color;
        Sprite.Rotation = Rotation;
        Sprite.Scale = Scale;

        var rect = Sprite.TextureRect;
        Size = new Vector2f(rect.Width * Scale.X, rect.Height * Scale.Y);

        GameContext.CurrentWindow.Draw(Sprite);
    }
}