using Core.Input;

namespace Core.Entity;

/// <summary>
/// Manages scenes in the game engine.
/// Provides functionality for switching between scenes and managing scene lifecycle.
/// </summary>
public static class SceneManager
{
    private static readonly HashSet<Scene> Scenes = [];
    internal static bool SceneSwapped { get; set; }
    
    /// <summary>
    /// Gets the current active scene.
    /// </summary>
    public static Scene CurrentScene => Scene.Current;

    /// <summary>
    /// Switches to a scene of the specified type, using the provided render window.
    /// </summary>
    /// <typeparam name="T">The type of scene to switch to.</typeparam>
    /// <exception cref="Exception">Thrown when attempting to switch to the same scene.</exception>
    public static void SwitchScene<T>() where T : Scene, new()
    {
        var sceneType = typeof(T);
        // RemoveScene<T>();

        if (Scene.Current.GetType() == sceneType)
            throw new Exception("Cannot swap to the same scene");

        RemoveScene(Scene.Current.GetType());

        Scene.Current = new PendingScene();
        var newScene = Scenes.FirstOrDefault(s => s.GetType() == sceneType);

        if (newScene == null)
        {
            newScene = new T();
            Scenes.Add(newScene);
        }

        Scene.Current = newScene;
        SceneSwapped = true;
    }

    /// <summary>
    /// Gets a scene of the specified type, throwing an exception if not found.
    /// </summary>
    /// <typeparam name="T">The type of scene to get.</typeparam>
    /// <returns>The found scene.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the scene is not found.</exception>
    public static T GetRequiredScene<T>() where T : Scene
    {
        var sceneType = typeof(T);
        var scene = Scenes.FirstOrDefault(s => s.GetType() == sceneType) ??
                    throw new InvalidOperationException($"Scene {sceneType.Name} not found.");
        return (T)scene;
    }

    /// <summary>
    /// Gets a scene of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of scene to get.</typeparam>
    /// <returns>The found scene, or null if not found.</returns>
    public static Scene GetScene<T>() where T : Scene => Scenes.FirstOrDefault(s => s.GetType() == typeof(T));

    /// <summary>
    /// Gets all scenes currently managed by the scene manager.
    /// </summary>
    /// <returns>A read-only collection of all scenes.</returns>
    public static IEnumerable<Scene> GetAllScenes() => Scenes.ToList().AsReadOnly();

    /// <summary>
    /// Removes a scene of the specified type from the scene manager.
    /// </summary>
    /// <typeparam name="T">The type of scene to remove.</typeparam>
    /// <exception cref="InvalidOperationException">Thrown when the scene is not found.</exception>
    public static void RemoveScene<T>() where T : Scene => RemoveScene(typeof(T));

    /// <summary>
    /// Removes a scene of the specified type from the scene manager.
    /// </summary>
    /// <param name="sceneType">
    ///  The type of scene to remove.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///  Thrown when the scene is not found.
    /// </exception>
    public static void RemoveScene(Type sceneType)
    {
        if(sceneType == null)
            throw new ArgumentNullException(nameof(sceneType), "Scene type cannot be null.");
        
        if (!typeof(Scene).IsAssignableFrom(sceneType))
            throw new ArgumentException($"Type {sceneType.Name} is not a valid Scene type.", nameof(sceneType));
        
        if (sceneType == typeof(PendingScene))
            return;
        
        var scene = Scenes.FirstOrDefault(s => s.GetType() == sceneType);
        if (scene == null)
            throw new InvalidOperationException($"Scene {sceneType.Name} not found.");

        scene.OnClose();
        InputManager.ClearBindings();
        Scenes.Remove(scene);
    }
}