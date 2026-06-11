using Core.Extensions;
using SFML.Window;

namespace Core.Input;

/// <summary>
/// Manages input handling for keyboard and mouse events in the game engine.
/// Bindings come in two tiers: scene-scoped bindings (the <see cref="BindAction(Keyboard.Key, ActionType, Action)"/>
/// overloads) are cleared on every scene switch, while global bindings (the
/// <see cref="BindGlobalAction(Keyboard.Key, ActionType, Action)"/> overloads) survive switches.
/// </summary>
public static class InputManager
{
    private static readonly Dictionary<Keyboard.Key, Dictionary<ActionType, List<Binding>>> KeyBindings = [];
    private static readonly Dictionary<Mouse.Button, Dictionary<ActionType, List<Binding>>> MouseBindings = [];

    /// <summary>
    /// A bound delegate together with whether it is global (survives scene switches) or scene-scoped.
    /// </summary>
    private readonly struct Binding(Delegate action, bool isGlobal)
    {
        public Delegate Action { get; } = action;
        public bool IsGlobal { get; } = isGlobal;
    }

    /// <summary>
    /// Binds a keyboard key to a scene-scoped action with no parameters. The binding is cleared on the next scene switch.
    /// </summary>
    /// <param name="key">The keyboard key to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the key is triggered.</param>
    public static void BindAction(Keyboard.Key key, ActionType actionType, Action action)
        => BindDelegate(KeyBindings, key, actionType, action, isGlobal: false);

    /// <summary>
    /// Binds a keyboard key to a scene-scoped action that receives key event arguments. The binding is cleared on the next scene switch.
    /// </summary>
    /// <param name="key">The keyboard key to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the key is triggered.</param>
    public static void BindAction(Keyboard.Key key, ActionType actionType, Action<KeyEventArgs> action)
        => BindDelegate(KeyBindings, key, actionType, action, isGlobal: false);

    /// <summary>
    /// Binds a mouse button to a scene-scoped action with no parameters. The binding is cleared on the next scene switch.
    /// </summary>
    /// <param name="button">The mouse button to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the button is triggered.</param>
    public static void BindAction(Mouse.Button button, ActionType actionType, Action action)
        => BindDelegate(MouseBindings, button, actionType, action, isGlobal: false);

    /// <summary>
    /// Binds a mouse button to a scene-scoped action that receives mouse button event arguments. The binding is cleared on the next scene switch.
    /// </summary>
    /// <param name="button">The mouse button to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the button is triggered.</param>
    public static void BindAction(Mouse.Button button, ActionType actionType, Action<MouseButtonEventArgs> action)
        => BindDelegate(MouseBindings, button, actionType, action, isGlobal: false);

    /// <summary>
    /// Binds a keyboard key to a global action with no parameters. The binding survives scene switches.
    /// </summary>
    /// <param name="key">The keyboard key to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the key is triggered.</param>
    public static void BindGlobalAction(Keyboard.Key key, ActionType actionType, Action action)
        => BindDelegate(KeyBindings, key, actionType, action, isGlobal: true);

    /// <summary>
    /// Binds a keyboard key to a global action that receives key event arguments. The binding survives scene switches.
    /// </summary>
    /// <param name="key">The keyboard key to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the key is triggered.</param>
    public static void BindGlobalAction(Keyboard.Key key, ActionType actionType, Action<KeyEventArgs> action)
        => BindDelegate(KeyBindings, key, actionType, action, isGlobal: true);

    /// <summary>
    /// Binds a mouse button to a global action with no parameters. The binding survives scene switches.
    /// </summary>
    /// <param name="button">The mouse button to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the button is triggered.</param>
    public static void BindGlobalAction(Mouse.Button button, ActionType actionType, Action action)
        => BindDelegate(MouseBindings, button, actionType, action, isGlobal: true);

    /// <summary>
    /// Binds a mouse button to a global action that receives mouse button event arguments. The binding survives scene switches.
    /// </summary>
    /// <param name="button">The mouse button to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the button is triggered.</param>
    public static void BindGlobalAction(Mouse.Button button, ActionType actionType, Action<MouseButtonEventArgs> action)
        => BindDelegate(MouseBindings, button, actionType, action, isGlobal: true);

    /// <summary>
    /// Binds a delegate to an input action.
    /// </summary>
    /// <typeparam name="T">The type of input (Keyboard.Key or Mouse.Button).</typeparam>
    /// <param name="bindings">The dictionary of bindings to add to.</param>
    /// <param name="input">The input to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The delegate to bind.</param>
    /// <param name="isGlobal">When <c>true</c> the binding survives scene switches; otherwise it is scene-scoped.</param>
    private static void BindDelegate<T>(IDictionary<T, Dictionary<ActionType, List<Binding>>> bindings, T input,
        ActionType actionType, Delegate action, bool isGlobal)
    {
        if (!bindings.TryGetValue(input, out var actionBindings))
        {
            actionBindings = new Dictionary<ActionType, List<Binding>>()
            {
                { ActionType.Pressed, [] },
                { ActionType.Released, [] },
                { ActionType.Held, [] }
            };
            bindings[input] = actionBindings;
        }

        actionBindings[actionType].Add(new Binding(action, isGlobal));
    }

    /// <summary>
    /// Invokes all actions bound to a specific input and action type.
    /// </summary>
    /// <typeparam name="T">The type of input (Keyboard.Key or Mouse.Button).</typeparam>
    /// <param name="bindings">The dictionary of bindings to check.</param>
    /// <param name="input">The input that triggered the action.</param>
    /// <param name="actionType">The type of action to invoke.</param>
    /// <param name="parameters">Optional parameters to pass to the action.</param>
    private static void InvokeActions<T>(IReadOnlyDictionary<T, Dictionary<ActionType, List<Binding>>> bindings, T input,
        ActionType actionType, params object[] parameters)
    {
        if (!bindings.TryGetValue(input, out var actionBindings) ||
            !actionBindings.TryGetValue(actionType, out var actions))
            return;

        // Snapshot first: a bound action may switch scenes, which clears scene-scoped bindings and would
        // otherwise mutate this very list mid-enumeration.
        actions.EnumeratedForEach(binding =>
        {
            if (binding.Action.Method.GetParameters().Length == parameters.Length)
                binding.Action.DynamicInvoke(parameters);
            else
                binding.Action.DynamicInvoke();
        });
    }

    /// <summary>
    /// Handles a key press event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The key event arguments.</param>
    public static void HandleKeyPressed(object sender, KeyEventArgs e)
        => InvokeActions(KeyBindings, e.Code, ActionType.Pressed, e);

    /// <summary>
    /// Handles a key release event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The key event arguments.</param>
    public static void HandleKeyReleased(object sender, KeyEventArgs e)
        => InvokeActions(KeyBindings, e.Code, ActionType.Released, e);

    /// <summary>
    /// Handles a mouse button press event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The mouse button event arguments.</param>
    public static void HandleMouseButtonPressed(object sender, MouseButtonEventArgs e)
        => InvokeActions(MouseBindings, e.Button, ActionType.Pressed, e);

    /// <summary>
    /// Handles a mouse button release event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The mouse button event arguments.</param>
    public static void HandleMouseButtonReleased(object sender, MouseButtonEventArgs e)
        => InvokeActions(MouseBindings, e.Button, ActionType.Released, e);

    /// <summary>
    /// Checks for and invokes actions for held keys and mouse buttons.
    /// </summary>
    public static void CheckHeldKeys()
    {
        KeyBindings
            .Keys
            .Where(Keyboard.IsKeyPressed)
            .EnumeratedForEach(key => InvokeActions(KeyBindings, key, ActionType.Held));

        MouseBindings
            .Keys
            .Where(Mouse.IsButtonPressed)
            .EnumeratedForEach(button => InvokeActions(MouseBindings, button, ActionType.Held));
    }
    
    /// <summary>
    /// Clears all input bindings for both keyboard and mouse, including global ones.
    /// </summary>
    public static void ClearBindings()
    {
        KeyBindings.Clear();
        MouseBindings.Clear();
    }

    /// <summary>
    /// Clears all scene-scoped bindings, leaving global bindings intact
    /// </summary>
    public static void ClearSceneBindings()
    {
        RemoveSceneScopedBindings(KeyBindings);
        RemoveSceneScopedBindings(MouseBindings);
    }

    private static void RemoveSceneScopedBindings<T>(Dictionary<T, Dictionary<ActionType, List<Binding>>> bindings)
    {
        foreach (var actionBindings in bindings.Values)
            foreach (var actions in actionBindings.Values)
                actions.RemoveAll(binding => !binding.IsGlobal);
    }
}