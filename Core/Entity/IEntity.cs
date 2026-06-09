namespace Core.Entity;

/// <summary>
/// Defines the basic interface for all entities in the game engine.
/// Entities are the fundamental objects that can exist in a scene.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets the unique identifier for this entity.
    /// </summary>
    Guid Guid { get; }

    /// <summary>
    /// Gets or sets whether this entity is currently active.
    /// Inactive entities are not updated or rendered.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the name of this entity.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Called once when the entity is first created or when the scene starts.
    /// </summary>
    protected void OnStart();

    /// <summary>
    /// Called every frame to update the entity's state.
    /// </summary>
    protected void OnUpdate();

    /// <summary>
    /// Called at fixed intervals for physics and other time-dependent updates.
    /// </summary>
    protected  void OnFixedUpdate();

    /// <summary>
    /// Called every frame to render the entity.
    /// </summary>
    protected void OnRender();

    /// <summary>
    /// Called when the entity is about to be destroyed or when the scene is closing.
    /// </summary>
    protected void OnClose();
    
    /// <summary>
    /// Called when debug render is toggeled.
    /// </summary>
    protected void OnDebugRender();
}