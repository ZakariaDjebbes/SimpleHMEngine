using Core.Drawing;
using SFML.Graphics;
using SFML.System;
using ZGeometry.Logic;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Point;

namespace Core.Collider;

public class CircleCollider : ColliderBase
{
    public Circle<float> Bounds { get; set; }
    public event Action<ColliderBase> CollisionEnter;
    public event Action<ColliderBase> CollisionExit;
    public event Action<ColliderBase> CollisionStay;

    public CircleCollider(float radius)
    {
        Bounds = Circle<float>.Create(Point.Create(Position.X, Position.Y), radius);
    }

    public override bool CheckCollision(ColliderBase other)
        => other switch
        {
            CircleCollider circleCollider => Bounds.Overlaps(circleCollider.Bounds),
            BoxCollider boxCollider => Bounds.Overlaps(boxCollider.Bounds),
            _ => false
        };

    public override void RaiseCollisionEnter(ColliderBase other) => CollisionEnter?.Invoke(other);

    public override void RaiseCollisionExit(ColliderBase other) => CollisionExit?.Invoke(other);

    public override void RaiseCollisionStay(ColliderBase other) => CollisionStay?.Invoke(other);

    protected override void FixedUpdate()
    {
        var center = Parent?.Position ?? Position;
        Bounds = Circle<float>.Create(Point.Create(center.X, center.Y), Bounds.Radius);
    }

    protected override void DebugRender()
    {
        Bounds.DrawSelf(new DrawOptions{FillColor = Color.Transparent, OutlineColor = Color.Green, OutlineThickness = 2});
        Draw.Circle(new Vector2f(Bounds.Center.X, Bounds.Center.Y), 1, new DrawOptions{FillColor = Color.Green});
    }
}