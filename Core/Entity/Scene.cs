using Core.Extensions;

namespace Core.Entity;

/// <summary>
/// Internal class used to represent a pending scene during scene transitions.
/// </summary>
internal class PendingScene : Scene;

/// <summary>
/// Base class for all scenes in the game engine.
/// A scene represents a collection of components that make up a game level or menu.
/// </summary>
public abstract class Scene : IEntity
{
    private readonly List<Component> _sceneComponents = [];
    private static Scene _currentScene;

    /// <summary>
    /// Gets the unique identifier for this scene.
    /// </summary>
    public Guid Guid { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets whether this scene is currently active.
    /// Inactive scenes are not updated or rendered.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the name of this scene.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initializes a new instance of the Scene class.
    /// </summary>
    protected Scene() => Name = GetType().Name;

    /// <summary>
    /// Gets the current active scene. The engine guarantees this is never <c>null</c>: before the first
    /// real scene is loaded it returns a transient placeholder, so components can be buffered safely.
    /// Change scenes through <see cref="SceneManager.SwitchScene{T}"/>; assignment is engine-internal,
    /// because loading a scene performs teardown, component discovery and start-up.
    /// </summary>
    public static Scene Current
    {
        get => _currentScene ??= new PendingScene();
        internal set => _currentScene = value;
    }

    /// <summary>
    /// Removes every component from this scene and returns them without closing them. Used by the scene
    /// manager to carry components buffered on the bootstrap placeholder into the first real scene.
    /// </summary>
    /// <returns>The components that were attached to this scene.</returns>
    internal Component[] DetachComponents()
    {
        var detached = _sceneComponents.ToArray();
        _sceneComponents.Clear();
        return detached;
    }

    /// <summary>
    /// Releases a component from the scene by its GUID.
    /// </summary>
    /// <param name="componentId">The GUID of the component to release.</param>
    /// <returns>True if the component was found and released, false otherwise.</returns>
    public bool Release(Guid componentId)
    {
        _sceneComponents
            .FirstOrDefault(component => component.Guid == componentId)?
            .OnClose();
        return _sceneComponents.RemoveWhere(component => component.Guid == componentId).Count() == 1;
    }

    /// <summary>
    /// Releases a specific component from the scene.
    /// </summary>
    /// <param name="component">The component to release.</param>
    /// <returns>True if the component was found and released, false otherwise.</returns>
    public bool Release(Component component)
    {
        _sceneComponents
            .FirstOrDefault(innerObject => innerObject.Guid == component.Guid)?
            .OnClose();
        return _sceneComponents.Remove(component);
    }

    /// <summary>
    /// Releases all components from the scene.
    /// </summary>
    public void ReleaseAll()
    {
        _sceneComponents.ForEach(component => component.OnClose());
        _sceneComponents.Clear();
    }

    /// <summary>
    /// Starts all active components in the scene.
    /// </summary>
    public void StartAll()
        => _sceneComponents
            .Where(component => component.IsActive)
            .EnumeratedForEach(component => component.OnStart());

    /// <summary>
    /// Updates all active components in the scene.
    /// </summary>
    public void UpdateAll()
        => _sceneComponents
            .Where(component => component.IsActive)
            .EnumeratedForEach(component => component.OnUpdate());

    /// <summary>
    /// Performs fixed updates on all active components in the scene.
    /// </summary>
    public void FixedUpdateAll()
        => _sceneComponents
            .Where(component => component.IsActive)
            .EnumeratedForEach(component => component.OnFixedUpdate());

    /// <summary>
    /// Renders all active components in the scene.
    /// </summary>
    public void RenderAll()
        => _sceneComponents
            .Where(component => component.IsActive)
            .EnumeratedForEach(component => component.OnRender());

    /// <summary>
    /// Renders all active components in the scene for debugging purposes.
    /// </summary>
    public void DebugRenderAll()
        => _sceneComponents
            .Where(component => component.IsActive)
            .EnumeratedForEach(component => component.OnDebugRender());

    /// <summary>
    /// Called when the scene starts. Override this method to perform scene-specific initialization.
    /// </summary>
    public virtual void OnStart()
    {
    }

    /// <summary>
    /// Called every frame to update the scene. Override this method to implement scene-specific update logic.
    /// </summary>
    public virtual void OnUpdate()
    {
    }

    /// <summary>
    /// Called at fixed intervals for physics and other time-dependent updates.
    /// Override this method to implement scene-specific fixed update logic.
    /// </summary>
    public virtual void OnFixedUpdate()
    {
    }

    /// <summary>
    /// Called every frame to render the scene. Override this method to implement scene-specific rendering logic.
    /// </summary>
    public virtual void OnRender()
    {
    }

    /// <summary>Called every frame for scene-level debug rendering. Override to add debug visuals.</summary>
    public void OnDebugRender()
    {
    }

    /// <summary>
    /// Called when the scene is about to close. Releases all components and calls the Close method.
    /// </summary>
    public void OnClose()
    {
        ReleaseAll();
    }

    /// <summary>
    /// Adds a component to the scene.
    /// </summary>
    /// <param name="component">The component to add.</param>
    public void AddComponent(Component component) => _sceneComponents.Add(component);

    /// <summary>Adds several components to the scene, ignoring any null entries.</summary>
    /// <param name="components">The components to add.</param>
    public void AddComponents(params Component[] components)
    {
        foreach (var component in components)
        {
            if (component != null)
                _sceneComponents.Add(component);
        }
    }

    /// <summary>
    /// Gets a component of the specified type from the scene.
    /// </summary>
    /// <typeparam name="T">The type of component to get.</typeparam>
    /// <returns>The found component, or null if not found.</returns>
    public T GetComponent<T>() where T : Component
    {
        if (_sceneComponents.Find(c => c is T) is T component)
            return component;

        foreach (var child in _sceneComponents)
        {
            component = child.GetComponent<T>();
            if (component != null)
                return component;
        }

        return null;
    }

    /// <summary>
    /// Gets all components of the specified type from the scene.
    /// </summary>
    /// <typeparam name="T">The type of components to get.</typeparam>
    /// <returns>An enumerable of found components.</returns>
    public IEnumerable<T> GetComponents<T>() where T : Component
    {
        var components = _sceneComponents.OfType<T>().ToList();
        _sceneComponents.ForEach(c => components.AddRange(c.GetComponentsInChildren<T>()));
        return components;
    }

    /// <summary>
    /// Gets a component of the specified type from the scene, throwing an exception if not found.
    /// </summary>
    /// <typeparam name="T">The type of component to get.</typeparam>
    /// <returns>The found component.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the component is not found.</exception>
    public T GetRequiredComponent<T>() where T : Component
    {
        var component = GetComponent<T>();

        if (component == null)
            throw new InvalidOperationException($"Component of type {typeof(T)} not found.");

        return component;
    }

    /// <summary>
    /// Removes a component from the scene.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    public void RemoveComponent(Component component) => _sceneComponents.Remove(component);

    /// <summary>
    /// Gets the total number of components in the scene, including child components.
    /// </summary>
    /// <returns>The total number of components.</returns>
    public int ComponentsCount() => _sceneComponents.Count + _sceneComponents.Select(c => c.ComponentsCount).Sum();
}