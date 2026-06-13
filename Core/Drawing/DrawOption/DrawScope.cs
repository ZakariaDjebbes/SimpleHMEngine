using SFML.System;

namespace Core.Drawing.DrawOption;

/// <summary>
/// A disposable transform/anchor scope returned by <see cref="Draw.Pushed"/>. It saves the drawing state
/// on creation and restores it on dispose, and offers fluent transform calls.
/// </summary>
public sealed class DrawScope : IDisposable
{
    internal DrawScope() => Draw.Push();

    /// <summary>Translates the frame and returns this scope for chaining.</summary>
    /// <param name="x">The horizontal offset.</param>
    /// <param name="y">The vertical offset.</param>
    /// <returns>This scope.</returns>
    public DrawScope Translate(float x, float y)
    {
        Draw.Translate(x, y);
        return this;
    }

    /// <summary>Translates the frame and returns this scope for chaining.</summary>
    /// <param name="offset">The offset to translate by.</param>
    /// <returns>This scope.</returns>
    public DrawScope Translate(Vector2f offset)
    {
        Draw.Translate(offset);
        return this;
    }

    /// <summary>Scales the frame uniformly and returns this scope for chaining.</summary>
    /// <param name="factor">The uniform scale factor.</param>
    /// <returns>This scope.</returns>
    public DrawScope Scale(float factor)
    {
        Draw.Scale(factor);
        return this;
    }

    /// <summary>Scales the frame per axis and returns this scope for chaining.</summary>
    /// <param name="x">The horizontal scale factor.</param>
    /// <param name="y">The vertical scale factor.</param>
    /// <returns>This scope.</returns>
    public DrawScope Scale(float x, float y)
    {
        Draw.Scale(x, y);
        return this;
    }

    /// <summary>Rotates the frame and returns this scope for chaining.</summary>
    /// <param name="degrees">The rotation angle, in degrees.</param>
    /// <returns>This scope.</returns>
    public DrawScope Rotate(float degrees)
    {
        Draw.Rotate(degrees);
        return this;
    }

    /// <summary>Sets the anchor mode and returns this scope for chaining.</summary>
    /// <param name="anchor">The anchor mode.</param>
    /// <returns>This scope.</returns>
    public DrawScope Mode(Anchor anchor)
    {
        Draw.DrawMode(anchor);
        return this;
    }

    /// <summary>Restores the drawing state captured when this scope was created.</summary>
    public void Dispose() => Draw.Pop();
}