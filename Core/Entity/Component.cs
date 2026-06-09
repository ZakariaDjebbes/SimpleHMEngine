using Core.Engine;
using SFML.System;

namespace Core.Entity;

/// <summary>
/// Base class for all components in the game engine.
/// Components are the building blocks of entities and can be attached to other components to form a hierarchy.
/// </summary>
public abstract class Component : IEntity
{
    private static int _unnamedCounter;
    private readonly List<Component> _components = [];
    
    /// <summary>
    /// Gets the unique identifier for this component.
    /// </summary>
    public Guid Guid { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets the parent component of this component.
    /// </summary>
    public Component Parent { get; private set; }

    /// <summary>
    /// Gets or sets whether this component is currently active.
    /// Inactive components are not updated or rendered.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the name of this component.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets this component's position relative to its parent.
    /// For a root component (no parent) this is the same as the world position.
    /// </summary>
    public Vector2f LocalPosition { get; set; }

    /// <summary>
    /// Gets or sets the position of this component in world space.
    /// World position is derived by walking the parent chain, so an attached child with a zero
    /// <see cref="LocalPosition"/> always tracks its parent automatically — at any nesting depth,
    /// and with no need to copy <c>Parent.Position</c> by hand. Setting it back-solves the local offset.
    /// </summary>
    public Vector2f Position
    {
        get => Parent is null ? LocalPosition : Parent.Position + LocalPosition;
        set => LocalPosition = Parent is null ? value : value - Parent.Position;
    }

    /// <summary>
    /// Initializes a new instance of the Component class.
    /// </summary>
    protected Component()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Component class with the specified name.
    /// </summary>
    /// <param name="name">The name of the component.</param>
    protected Component(string name) => Name = name;

    /// <summary>
    /// Initializes a new instance of the Component class with the specified position.
    /// </summary>
    /// <param name="position">The initial position of the component.</param>
    protected Component(Vector2f position) => Position = position;

    /// <summary>
    /// Initializes a new instance of the Component class with the specified coordinates.
    /// </summary>
    /// <param name="x">The X-coordinate of the component's position.</param>
    /// <param name="y">The Y-coordinate of the component's position.</param>
    protected Component(float x, float y) => Position = new Vector2f(x, y);

    /// <summary>
    /// Called when this component is attached to a parent component.
    /// Override this method to perform initialization specific to the attachment.
    /// </summary>
    /// <param name="parent">The parent component this component is being attached to.</param>
    protected virtual void Attach(Component parent)
    {
    }

    /// <summary>
    /// Called once when the component is first created or when the scene starts.
    /// Override this method to perform initialization.
    /// </summary>
    protected virtual void Start()
    {
    }

    /// <summary>
    /// Called every frame to render the component.
    /// Override this method to implement rendering logic.
    /// </summary>
    protected virtual void Render()
    {
    }
    
    /// <summary>
    /// Called every frame to render debug information for the component.
    /// /// Override this method to implement debug rendering logic.
    /// </summary>
    protected virtual void DebugRender()
    {
    }

    /// <summary>
    /// Called every frame to update the component's state.
    /// Override this method to implement update logic.
    /// </summary>
    protected virtual void Update()
    {
    }

    /// <summary>
    /// Called at fixed intervals for physics and other time-dependent updates.
    /// Override this method to implement fixed update logic.
    /// </summary>
    protected virtual void FixedUpdate()
    {
    }

    /// <summary>
    /// Called when the component is about to be destroyed.
    /// Override this method to perform cleanup.
    /// </summary>
    protected virtual void Close()
    {
    }

    /// <summary>
    /// Adds a component as a child of this component.
    /// </summary>
    /// <param name="component">The component to add.</param>
    protected void AddComponent(Component component)
    {
        component.OnAttach(this);
        _components.Add(component);
    }
    
    /// <summary>
    /// Adds a component as a child of this component, ensuring no duplicate types exist.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <exception cref="ArgumentException">Thrown when a component of the same type already exists.</exception>
    protected void AddComponentUnique(Component component)
    {
        if (component.GetType() == GetType() || _components.Select(c => c.GetType()).Contains(component.GetType()))
            throw new ArgumentException(
                $"Trying to add component {component.GetType().Name} with guid {component.Guid} when it already exists or is current parent",
                nameof(component));

        AddComponent(component);
    }
    
    /// <summary>
    /// Finds a component of the specified type in the parent hierarchy.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <returns>The found component, or null if not found.</returns>
    public T FindComponentInParent<T>() where T : Component
    {
        var parent = Parent;
        while (parent != null)
        {
            if (parent is T component)
                return component;

            foreach (var c in parent._components)
            {
                if (c is T component2)
                    return component2;
            }
            
            parent = parent.Parent;
        }
        return null;
    }

    /// <summary>
    /// Finds a component of the specified type in the parent hierarchy, throwing an exception if not found.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <returns>The found component.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the component is not found.</exception>
    public T FindRequiredComponentInParent<T>() where T : Component => FindComponentInParent<T>() ?? throw new InvalidOperationException($"Component of type {typeof(T)} not found in parent.");

    /// <summary>
    /// Finds a specific component instance in the parent hierarchy.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <param name="component">The component instance to find.</param>
    /// <returns>The found component, or null if not found.</returns>
    public T FindComponentInParent<T>(T component) where T : Component
    {
        var res = FindComponentInParent<T>();
        return res is null || res.Guid != component.Guid ? null : res;
    }
    
    /// <summary>
    /// Finds a component of the specified type in the children hierarchy.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <returns>The found component, or null if not found.</returns>
    public T FindComponentInChildren<T>() where T : Component
    {
        foreach (var child in _components)
        {
            if (child is T component)
                return component;

            var found = child.FindComponentInChildren<T>();
            if (found != null)
                return found;
        }
        return null;
    }

    /// <summary>
    /// Finds a component of the specified type in the children hierarchy, throwing an exception if not found.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <returns>The found component.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the component is not found.</exception>
    public T FindRequiredComponentInChildren<T>() where T : Component
    {
        var component = FindComponentInChildren<T>();
        if (component == null)
            throw new InvalidOperationException($"Component of type {typeof(T)} not found in children.");
        return component;
    }
    
    /// <summary>
    /// Gets a component of the specified type from this component or its direct children.
    /// </summary>
    /// <typeparam name="T">The type of component to get.</typeparam>
    /// <returns>The found component, or null if not found.</returns>
    public T GetComponent<T>() where T : Component
    {
        if (_components.Find(c => c is T) is T component)
            return component;

        if (this is T thisAsT)
            return thisAsT;

        return null;
    }

    /// <summary>
    /// Gets a component of the specified type from this component or its direct children, throwing an exception if not found.
    /// </summary>
    /// <typeparam name="T">The type of component to get.</typeparam>
    /// <returns>The found component.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the component is not found.</exception>
    public T GetRequiredComponent<T>() where T : Component
    {
        if (_components.Find(c => c is T) is T component)
            return component;

        if (this is T thisAsT)
            return thisAsT;

        throw new InvalidOperationException($"Component of type {typeof(T)} not found.");
    }
    
    /// <summary>
    /// Gets all components of the specified type from the children hierarchy.
    /// </summary>
    /// <typeparam name="T">The type of components to get.</typeparam>
    /// <returns>Enumerable of found components.</returns>
    public IEnumerable<T> GetComponentsInChildren<T>() where T : Component
    {
        var components = new List<T>();

        foreach (var child in _components)
        {
            if (child is T component)
            {
                components.Add(component);
            }
            components.AddRange(child.GetComponentsInChildren<T>());
        }

        return components;
    }

    /// <summary>
    /// Gets all components of the specified type from the parent hierarchy.
    /// </summary>
    /// <typeparam name="T">The type of components to get.</typeparam>
    /// <returns>Enumerable of found components.</returns>
    public IEnumerable<T> GetComponentsInParent<T>() where T : Component
    {
        var components = new List<T>();
        var parent = Parent;

        while (parent != null)
        {
            if (parent is T component)
            {
                components.Add(component);
            }

            foreach (var c in parent._components)
            {
                if(c is T component2)
                    components.Add(component2);
            }
            
            parent = parent.Parent;
        }

        return components;
    }
    
    /// <summary>
    /// Gets the number of child components.
    /// </summary>
    public int ComponentsCount => _components.Count;

    /// <summary>
    /// Called when this component is attached to another component.
    /// </summary>
    /// <param name="component">The component this component is being attached to.</param>
    public void OnAttach(Component component)
    {
        Parent = component;
        Attach(component);
    }

    /// <summary>
    /// Called once when the component is first created or when the scene starts.
    /// </summary>
    public void OnStart()
    {
        Name = GenerateUniqueName();
        Start();
        _components.ForEach(c => c.OnStart());
    }

    /// <summary>
    /// Called every frame to render the component.
    /// </summary>
    public void OnRender()
    {
        Render();
        _components.ForEach(c => c.OnRender());
    }
    
    /// <summary>Recursively debug-renders this component and its children when debug mode is on.</summary>
    public void OnDebugRender()
    {
        if (!GameContext.IsDebugMode) return;

        _components.ForEach(c => c.OnDebugRender());
        DebugRender();
    }

    /// <summary>
    /// Called every frame to update the component's state.
    /// </summary>
    public void OnUpdate()
    {
        Update();
        
        _components.ToList().ForEach(c => c.OnUpdate());
    }
    
    /// <summary>
    /// Called at fixed intervals for physics and other time-dependent updates.
    /// </summary>
    public void OnFixedUpdate()
    {
        FixedUpdate();
        
        _components.ToList().ForEach(c => c.OnFixedUpdate());
    }

    /// <summary>
    /// Called when the component is about to be destroyed.
    /// </summary>
    public void OnClose()
    {
        _components.ForEach(c => c.OnClose());
        Close();
    }

    /// <summary>
    /// Instantiates a new component and optionally attaches it to this component.
    /// </summary>
    /// <param name="component">The component to instantiate.</param>
    /// <param name="attach">Whether to attach the component to this component.</param>
    protected void Instantiate(Component component, bool attach = true)
    {
        if (attach)
        {
            component.OnAttach(this);
            _components.Add(component);
        }

        if (Scene.Current != null && component.Parent is null)
            Scene.Current.AddComponent(component);

        component.OnStart();
    }

    /// <summary>
    /// Destroys a component and all its children.
    /// </summary>
    /// <param name="component">The component to destroy, or null to destroy this component.</param>
    protected void Destroy(Component component = null)
    {
        component ??= this;
        
        component.OnClose();
        component._components.ToList().ForEach(c =>
        {
            c.OnClose();
            Destroy(c);
        });
        component.ClearComponents();

        if (component.Parent is null)
            Scene.Current.RemoveComponent(component);
        else
            component.Parent._components.Remove(component);
    }

    private void ClearComponents() => _components.Clear();

    private string GenerateUniqueName() => $"{GetType().Name}_{_unnamedCounter++}";

    /// <summary>
    /// Returns a string representation of this component.
    /// </summary>
    /// <returns>The name of this component.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Returns the hash code for this component.
    /// </summary>
    /// <returns>The hash code of this component's GUID.</returns>
    public override int GetHashCode() => Guid.GetHashCode();
}