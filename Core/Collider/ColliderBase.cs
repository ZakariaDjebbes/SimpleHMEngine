using Core.Entity;

namespace Core.Collider;

/// <summary>
/// Base class for all collider components in the game engine.
/// Colliders are used to detect and respond to collisions between game objects.
/// </summary>
public abstract class ColliderBase : Component
{
    /// <summary>
    /// Gets or sets whether this collider is a trigger.
    /// Trigger colliders do not cause physical collisions but still detect overlap.
    /// </summary>
    public bool IsTrigger { get; set; } = false;

    /// <summary>
    /// Checks if this collider is colliding with another collider.
    /// </summary>
    /// <param name="other">The other collider to check collision with.</param>
    /// <returns>True if the colliders are colliding, false otherwise.</returns>
    public abstract bool CheckCollision(ColliderBase other);

    /// <summary>
    /// Raises the collision enter event for this collider.
    /// Called when this collider first comes into contact with another collider.
    /// </summary>
    /// <param name="other">The other collider that was collided with.</param>
    public abstract void RaiseCollisionEnter(ColliderBase other);

    /// <summary>
    /// Raises the collision exit event for this collider.
    /// Called when this collider stops colliding with another collider.
    /// </summary>
    /// <param name="other">The other collider that is no longer colliding.</param>
    public abstract void RaiseCollisionExit(ColliderBase other);

    /// <summary>
    /// Raises the collision stay event for this collider.
    /// Called every frame while this collider is colliding with another collider.
    /// </summary>
    /// <param name="other">The other collider that is being collided with.</param>
    public abstract void RaiseCollisionStay(ColliderBase other);
}