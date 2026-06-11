using System.Collections;
using System.Reflection;
using Core.Entity.Attributes;
using Core.Input;

namespace Core.Entity;

/// <summary>
/// Manages scenes in the game engine: switching between them and carrying persistent components across.
/// </summary>
public static class SceneManager
{
    private const BindingFlags FieldFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    private static readonly Dictionary<Type, Component> PersistentComponents = [];

    internal static bool SceneSwapped { get; set; }

    /// <summary>
    /// Gets the current active scene.
    /// </summary>
    public static Scene CurrentScene => Scene.Current;

    /// <summary>
    /// Switches to a fresh instance of the specified scene type, tearing down the current scene.
    /// </summary>
    /// <typeparam name="T">The type of scene to switch to.</typeparam>
    /// <exception cref="InvalidOperationException">Thrown when attempting to switch to the active scene's type.</exception>
    public static void SwitchScene<T>() where T : Scene, new()
    {
        if (Scene.Current.GetType() == typeof(T))
            throw new InvalidOperationException("Cannot swap to the same scene.");

        Load(new T());
        SceneSwapped = true;
    }

    /// <summary>
    /// Gets the shared persistent component of the specified type, or <c>null</c> if none has been
    /// registered yet. A component becomes persistent once a loaded scene exposes it through a field
    /// marked with <see cref="PersistentAttribute"/>.
    /// </summary>
    /// <typeparam name="T">The type of persistent component to get.</typeparam>
    /// <returns>The shared instance, or <c>null</c> if it does not exist.</returns>
    public static T GetPersistentComponent<T>() where T : Component
        => PersistentComponents.TryGetValue(typeof(T), out var component) ? (T)component : null;

    /// <summary>
    /// Loads a scene: carries over any bootstrap components, tears down the previous scene (sparing
    /// persistent components), clears scene-scoped input, then discovers, attaches and starts the new
    /// scene's components.
    /// </summary>
    private static void Load(Scene scene)
    {
        var previous = Scene.Current;

        // The bootstrap placeholder may hold components buffered before the first real scene; carry them over.
        var carried = previous is PendingScene ? previous.DetachComponents() : Array.Empty<Component>();

        if (previous is not PendingScene)
        {
            // Spare persistent components from teardown so their state outlives the switch.
            foreach (var persistent in PersistentComponents.Values)
                previous.RemoveComponent(persistent);
            previous.OnClose();
        }

        InputManager.ClearSceneBindings();
        Scene.Current = scene;

        // Components are started only after the scene's own OnStart has populated them, matching the
        // previous load order. Persistent components that already existed are attached but not restarted.
        var toStart = new List<Component>();

        foreach (var component in carried)
        {
            scene.AddComponent(component);
            toStart.Add(component);
        }

        foreach (var (component, isNew) in Discover(scene, new HashSet<object>(ReferenceEqualityComparer.Instance)))
        {
            scene.AddComponent(component);
            if (isNew)
                toStart.Add(component);
        }

        scene.OnStart();
        toStart.ForEach(component => component.OnStart());
    }

    /// <summary>
    /// Walks an object's fields to find the components a scene should manage. Fields marked
    /// <see cref="DetachedAttribute"/> are skipped; fields marked <see cref="PersistentAttribute"/>
    /// resolve through the shared registry. The <paramref name="visited"/> set guards against cycles in
    /// the object graph. Each result reports whether the component is new and therefore needs starting.
    /// </summary>
    private static IEnumerable<(Component component, bool isNew)> Discover(object owner, HashSet<object> visited)
    {
        if (owner is null || !visited.Add(owner))
            yield break;

        foreach (var field in owner.GetType().GetFields(FieldFlags))
        {
            if (field.IsDefined(typeof(DetachedAttribute), inherit: true))
                continue;

            var value = field.GetValue(owner);
            switch (value)
            {
                case null:
                    continue;
                case Component component:
                    yield return field.IsDefined(typeof(PersistentAttribute), inherit: true)
                        ? ResolvePersistent(owner, field, component)
                        : (component, true);
                    break;
                case IEnumerable enumerable and not string:
                    foreach (var item in enumerable)
                    {
                        if (item is Component inner)
                            yield return (inner, true);
                        else if (item is not null && IsExplorable(item.GetType()))
                            foreach (var deeper in Discover(item, visited))
                                yield return deeper;
                    }
                    break;
                default:
                    if (IsExplorable(field.FieldType))
                        foreach (var inner in Discover(value, visited))
                            yield return inner;
                    break;
            }
        }
    }

    // Looks up (or registers) the single shared instance for the field's component type. When one already
    // exists, the field is repointed at it and the freshly constructed candidate is discarded; the shared
    // instance is reported as not-new so it is neither started nor torn down more than once.
    private static (Component component, bool isNew) ResolvePersistent(object owner, FieldInfo field, Component candidate)
    {
        var key = candidate.GetType();

        if (PersistentComponents.TryGetValue(key, out var existing))
        {
            if (!ReferenceEquals(existing, candidate))
                field.SetValue(owner, existing);
            return (existing, false);
        }

        PersistentComponents[key] = candidate;
        return (candidate, true);
    }

    private static bool IsExplorable(Type type)
        => type is { IsPrimitive: false, IsEnum: false } && type != typeof(string);
}
