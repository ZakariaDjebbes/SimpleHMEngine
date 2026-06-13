using SFML.Graphics;
using SFML.System;

namespace Core.Drawing.DrawOption;

/// <summary>
/// Drawing options for sprites, adding a texture source, sub-rectangle, scale and origin on top of
/// <see cref="DrawOptions"/>. <see cref="DrawOptions.FillColor"/> acts as the tint (default white),
/// <see cref="DrawOptions.Opacity"/> modulates its alpha and <see cref="DrawOptions.Rotation"/> rotates
/// the sprite. The outline options are not used for sprites.
/// </summary>
public class SpriteDrawOptions : DrawOptions
{
    /// <summary>
    /// The texture to draw. When set, it takes precedence over <see cref="TexturePath"/>.
    /// </summary>
    public Texture Texture { get; set; }

    /// <summary>
    /// Path the texture is loaded from (through the shared resource cache) when <see cref="Texture"/>
    /// is null. When both are null, the engine's default texture is used.
    /// </summary>
    public string TexturePath { get; set; }

    /// <summary>
    /// Sub-rectangle of the texture to draw. When null, the whole texture is used.
    /// </summary>
    public IntRect? SourceRect { get; set; }

    /// <summary>
    /// Scale applied on each axis. When null, the sprite is drawn at its native size (1, 1).
    /// </summary>
    public Vector2f? Scale { get; set; }

    /// <summary>
    /// Local origin used as the pivot for position, rotation and scale. When null, the top-left (0, 0).
    /// </summary>
    public Vector2f? Origin { get; set; }

    /// <summary>
    /// Creates sprite options drawing the given texture, letting a <see cref="Texture"/> be passed
    /// directly to <c>Draw.Sprite</c>.
    /// </summary>
    /// <param name="texture">The texture to draw.</param>
    public static implicit operator SpriteDrawOptions(Texture texture) => new() { Texture = texture };

    /// <summary>
    /// Creates sprite options drawing the texture at the given path, letting a path be passed directly
    /// to <c>Draw.Sprite</c>.
    /// </summary>
    /// <param name="texturePath">The texture resource path.</param>
    public static implicit operator SpriteDrawOptions(string texturePath) => new() { TexturePath = texturePath };

    /// <summary>Creates options drawing the given texture, as the start of a fluent chain.</summary>
    /// <param name="texture">The texture to draw.</param>
    /// <returns>A new <see cref="SpriteDrawOptions"/> instance.</returns>
    public static SpriteDrawOptions FromTexture(Texture texture) => new() { Texture = texture };

    /// <summary>Creates options drawing the texture at the given path, as the start of a fluent chain.</summary>
    /// <param name="texturePath">The texture resource path.</param>
    /// <returns>A new <see cref="SpriteDrawOptions"/> instance.</returns>
    public static SpriteDrawOptions FromPath(string texturePath) => new() { TexturePath = texturePath };

    /// <summary>Sets the explicit texture and returns this instance for chaining.</summary>
    /// <param name="texture">The texture to draw.</param>
    /// <returns>This instance.</returns>
    public SpriteDrawOptions WithTexture(Texture texture)
    {
        Texture = texture;
        return this;
    }

    /// <summary>Sets the sub-rectangle of the texture to draw and returns this instance for chaining.</summary>
    /// <param name="rect">The source sub-rectangle.</param>
    /// <returns>This instance.</returns>
    public SpriteDrawOptions WithSource(IntRect rect)
    {
        SourceRect = rect;
        return this;
    }

    /// <summary>Sets a per-axis scale and returns this instance for chaining.</summary>
    /// <param name="scale">The scale on each axis.</param>
    /// <returns>This instance.</returns>
    public SpriteDrawOptions WithScale(Vector2f scale)
    {
        Scale = scale;
        return this;
    }

    /// <summary>Sets a uniform scale and returns this instance for chaining.</summary>
    /// <param name="uniform">The scale applied to both axes.</param>
    /// <returns>This instance.</returns>
    public SpriteDrawOptions WithScale(float uniform)
    {
        Scale = new Vector2f(uniform, uniform);
        return this;
    }

    /// <summary>Sets the local origin (pivot) and returns this instance for chaining.</summary>
    /// <param name="origin">The local origin, in texture pixels.</param>
    /// <returns>This instance.</returns>
    public SpriteDrawOptions WithOrigin(Vector2f origin)
    {
        Origin = origin;
        return this;
    }

    /// <summary>Sets the tint color (stored as <see cref="DrawOptions.FillColor"/>) and returns this instance for chaining.</summary>
    /// <param name="color">The tint color.</param>
    /// <returns>This instance.</returns>
    public SpriteDrawOptions WithTint(Color color)
    {
        FillColor = color;
        return this;
    }

    /// <summary>Sets the rotation and returns this typed instance, keeping a sprite fluent chain intact.</summary>
    /// <param name="degrees">The rotation angle, in degrees.</param>
    /// <returns>This instance.</returns>
    public new SpriteDrawOptions WithRotation(float degrees)
    {
        Rotation = degrees;
        return this;
    }

    /// <summary>Sets the opacity and returns this typed instance, keeping a sprite fluent chain intact.</summary>
    /// <param name="opacity">The opacity, from 0.0 (transparent) to 1.0 (opaque).</param>
    /// <returns>This instance.</returns>
    public new SpriteDrawOptions WithOpacity(float opacity)
    {
        Opacity = opacity;
        return this;
    }
}
