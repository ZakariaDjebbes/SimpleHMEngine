using Core.Collider;
using Core.Drawing;
using Core.Engine;
using Core.Entity;
using SFML.System;

namespace Demo.Demo;

public class BouncingBall : Component
{
    public float Radius { get; init; } = 18.0f;
    public Vector2f Velocity { get; set; }
    public Vector2f AreaMin { get; init; }
    public Vector2f AreaMax { get; init; }

    private CircleCollider _collider;
    private int _contacts;

    protected override void Start()
    {
        _collider = new CircleCollider(Radius);
        _collider.CollisionEnter += OnCollisionEnter;
        _collider.CollisionExit += OnCollisionExit;

        AddComponent(_collider);
    }

    private void OnCollisionEnter(ColliderBase other) => _contacts++;

    private void OnCollisionExit(ColliderBase other) => _contacts = Math.Max(0, _contacts - 1);

    protected override void FixedUpdate()
    {
        var next = Position + Velocity * GameContext.FixedDeltaTime;

        var velocity = Velocity;
        if (next.X - Radius < AreaMin.X || next.X + Radius > AreaMax.X) velocity.X = -velocity.X;
        if (next.Y - Radius < AreaMin.Y || next.Y + Radius > AreaMax.Y) velocity.Y = -velocity.Y;
        Velocity = velocity;

        Position += Velocity * GameContext.FixedDeltaTime;
    }

    protected override void Render()
    {
        var fill = _contacts > 0 ? Palette.LightRed : Palette.LightAqua;
        Draw.Circle(Position, Radius, new DrawOptions
        {
            FillColor = fill,
            OutlineColor = Palette.DarkAqua,
            OutlineThickness = 2
        });
    }
}
