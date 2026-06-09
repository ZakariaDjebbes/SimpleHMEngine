using SFML.Graphics;
using SFML.System;

namespace Core.Drawing;

/// <summary>
/// A drawable rectangle filled with a vertical gradient from a start color (top) to an end color
/// (bottom). Used as a background for UI cards and panels.
/// </summary>
public class GradiantRect : Drawable
{
    private readonly Vertex[] _vertices = new Vertex[4];
    private readonly Vector2f _size;
    private Vector2f _position;
    private readonly Color _startColor;
    private readonly Color _endColor;

    /// <summary>Creates a gradient rectangle of the given size, from <paramref name="startColor"/> (top) to <paramref name="endColor"/> (bottom).</summary>
    /// <param name="size">The rectangle size in pixels.</param>
    /// <param name="startColor">The color at the top edge.</param>
    /// <param name="endColor">The color at the bottom edge.</param>
    public GradiantRect(Vector2f size, Color startColor, Color endColor)
    {
        _size = size;
        _startColor = startColor;
        _endColor = endColor;
        UpdateVertices();
    }

    /// <summary>The top-left position of the rectangle, in pixels.</summary>
    public Vector2f Position
    {
        get => _position;
        set
        {
            _position = value;
            UpdateVertices();
        }
    }

    private void UpdateVertices()
    {
        _vertices[0] = new Vertex(new Vector2f(_position.X, _position.Y), _startColor);
        _vertices[1] = new Vertex(new Vector2f(_position.X + _size.X, _position.Y), _startColor);
        _vertices[2] = new Vertex(new Vector2f(_position.X + _size.X, _position.Y + _size.Y), _endColor);
        _vertices[3] = new Vertex(new Vector2f(_position.X, _position.Y + _size.Y), _endColor);
    }

    /// <summary>Draws the gradient quad to the given render target.</summary>
    public void Draw(RenderTarget target, RenderStates states) => target.Draw(_vertices, PrimitiveType.Quads, states);
}