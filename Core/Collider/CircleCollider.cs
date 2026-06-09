using Core.Drawing;
using SFML.Graphics;
using SFML.System;
using ZGeometry.Logic;
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Point;

namespace Core.Collider;

/// <summary>
/// A circular collider. Its <see cref="Bounds"/> follow the owning component's position and are
/// tested for overlap against other colliders by the <see cref="CollisionManager"/>.
/// </summary>
public class CircleCollider : ColliderBase
{
    /// <summary>The collider's circle in world space.</summary>
    public Circle<float> Bounds { get; set; }

    /// <summary>Raised when this collider first begins overlapping another.</summary>
    public event Action<ColliderBase> CollisionEnter;

    /// <summary>Raised when this collider stops overlapping another.</summary>
    public event Action<ColliderBase> CollisionExit;

    /// <summary>Raised every fixed update while this collider overlaps another.</summary>
    public event Action<ColliderBase> CollisionStay;

    /// <summary>Creates a circle collider with the given radius.</summary>
    /// <param name="radius">The collider radius.</param>
    public CircleCollider(float radius)
    {
        Bounds = Circle<float>.Create(Point.Create(Position.X, Position.Y), radius);
    }

    /// <summary>Returns whether this collider overlaps <paramref name="other"/>.</summary>
    public override bool CheckCollision(ColliderBase other)
        => other switch
        {
            CircleCollider circleCollider => Bounds.Overlaps(circleCollider.Bounds),
            BoxCollider boxCollider => Bounds.Overlaps(boxCollider.Bounds),
            _ => false
        };

    /// <summary>Raises <see cref="CollisionEnter"/> for the given other collider.</summary>
    public override void RaiseCollisionEnter(ColliderBase other) => CollisionEnter?.Invoke(other);

    /// <summary>Raises <see cref="CollisionExit"/> for the given other collider.</summary>
    public override void RaiseCollisionExit(ColliderBase other) => CollisionExit?.Invoke(other);

    /// <summary>Raises <see cref="CollisionStay"/> for the given other collider.</summary>
    public override void RaiseCollisionStay(ColliderBase other) => CollisionStay?.Invoke(other);

    /// <summary>Keeps the circle centered on the owning component each fixed update.</summary>
    protected override void FixedUpdate()
    {
        var center = Parent?.Position ?? Position;
        Bounds = Circle<float>.Create(Point.Create(center.X, center.Y), Bounds.Radius);
    }

    /// <summary>Draws the collider outline and center point for debugging.</summary>
    protected override void DebugRender()
    {
        Bounds.DrawSelf(new DrawOptions{FillColor = Color.Transparent, OutlineColor = Color.Green, OutlineThickness = 2});
        Draw.Circle(new Vector2f(Bounds.Center.X, Bounds.Center.Y), 1, new DrawOptions{FillColor = Color.Green});
    }
}