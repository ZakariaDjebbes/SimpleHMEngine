using SFML.Graphics;
using SFML.System;

namespace Core.Drawing;

public class GradiantRect : Drawable
{
    private readonly Vertex[] _vertices = new Vertex[4];
    private readonly Vector2f _size;
    private Vector2f _position;
    private readonly Color _startColor;
    private readonly Color _endColor;

    public GradiantRect(Vector2f size, Color startColor, Color endColor)
    {
        _size = size;
        _startColor = startColor;
        _endColor = endColor;
        UpdateVertices();
    }

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

    public void Draw(RenderTarget target, RenderStates states) => target.Draw(_vertices, PrimitiveType.Quads, states);
}