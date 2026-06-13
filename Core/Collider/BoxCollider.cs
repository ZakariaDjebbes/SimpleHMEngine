using Core.Drawing;
using Core.Drawing.DrawOption;
using SFML.System;
using ZGeometry.Logic;
using ZGeometry.Primitives.Point;
using ZGeometry.Primitives.Rectangle;
using Color = SFML.Graphics.Color;

namespace Core.Collider;

/// <summary>
/// A rectangular collider component that can detect collisions with other colliders.
/// The collider's bounds are automatically updated based on the parent component's position.
/// </summary>
public class BoxCollider : ColliderBase
{
    /// <summary>
    /// Gets or sets the bounds of the box collider.
    /// The bounds are represented as a rectangle with position and size.
    /// </summary>
    public Rectangle<float> Bounds { get; set; }

    /// <summary>
    /// Event that is raised when this collider first comes into contact with another collider.
    /// </summary>
    public event Action<ColliderBase> CollisionEnter;

    /// <summary>
    /// Event that is raised when this collider stops colliding with another collider.
    /// </summary>
    public event Action<ColliderBase> CollisionExit;

    /// <summary>
    /// Event that is raised every frame while this collider is colliding with another collider.
    /// </summary>
    public event Action<ColliderBase> CollisionStay;

    /// <summary>
    /// Initializes a new instance of the BoxCollider class with the specified size.
    /// </summary>
    /// <param name="size">The size of the box collider as a Vector2f.</param>
    public BoxCollider(Vector2f size) : this(Vector2D<float>.Create(size.X, size.Y))
    {
    }

    /// <summary>
    /// Initializes a new instance of the BoxCollider class with the specified width and height.
    /// </summary>
    /// <param name="width">The width of the box collider.</param>
    /// <param name="height">The height of the box collider.</param>
    public BoxCollider(float width, float height) : this(Vector2D<float>.Create(width, height))
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the BoxCollider class with the specified size.
    /// </summary>
    /// <param name="size">The size of the box collider as a Vector2D.</param>
    public BoxCollider(Vector2D<float> size) => Bounds = Rectangle<float>.Create((Position.X, Position.Y), size);

    /// <summary>
    /// Checks if this box collider is colliding with another collider.
    /// </summary>
    /// <param name="other">The other collider to check collision with.</param>
    /// <returns>True if the colliders are colliding, false otherwise.</returns>
    public override bool CheckCollision(ColliderBase other)
        => other switch
        {
            BoxCollider boxCollider => Bounds.Overlaps(boxCollider.Bounds),
            CircleCollider circleCollider => Bounds.Overlaps(circleCollider.Bounds),
            _ => false
        };

    /// <summary>
    /// Updates the collider's bounds based on the parent component's position.
    /// Called during the fixed update phase of the game loop.
    /// </summary>
    protected override void FixedUpdate()
        => Bounds = Rectangle<float>.Create((Parent.Position.X, Parent.Position.Y), (Bounds.Size.X, Bounds.Size.Y));

    /// <summary>
    /// Raises the collision enter event for this collider.
    /// </summary>
    /// <param name="other">The other collider that was collided with.</param>
    public override void RaiseCollisionEnter(ColliderBase other) => CollisionEnter?.Invoke(other);

    /// <summary>
    /// Raises the collision exit event for this collider.
    /// </summary>
    /// <param name="other">The other collider that is no longer colliding.</param>
    public override void RaiseCollisionExit(ColliderBase other) => CollisionExit?.Invoke(other);

    /// <summary>
    /// Raises the collision stay event for this collider.
    /// </summary>
    /// <param name="other">The other collider that is being collided with.</param>
    public override void RaiseCollisionStay(ColliderBase other) => CollisionStay?.Invoke(other);

    /// <summary>
    /// Renders the collider's bounds in debug mode.
    /// The bounds are drawn as a green outline when in debug mode.
    /// </summary>
    protected override void DebugRender()
    {
        Bounds.DrawSelf(new DrawOptions { FillColor = Color.Transparent, OutlineColor = Color.Green, OutlineThickness = 2 });
    }
}