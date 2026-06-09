using Core.Extensions;
using SFML.Window;

namespace Core.Input;

/// <summary>
/// Manages input handling for keyboard and mouse events in the game engine.
/// </summary>
/// TODO: Global inputs and local inputs (only for current scene).
public static class InputManager
{
    private static readonly Dictionary<Keyboard.Key, Dictionary<ActionType, List<Delegate>>> KeyBindings = [];
    private static readonly Dictionary<Mouse.Button, Dictionary<ActionType, List<Delegate>>> MouseBindings = [];

    /// <summary>
    /// Binds a keyboard key to an action with no parameters.
    /// </summary>
    /// <param name="key">The keyboard key to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the key is triggered.</param>
    public static void BindAction(Keyboard.Key key, ActionType actionType, Action action)
        => BindDelegate(KeyBindings, key, actionType, action);

    /// <summary>
    /// Binds a keyboard key to an action that receives key event arguments.
    /// </summary>
    /// <param name="key">The keyboard key to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the key is triggered.</param>
    public static void BindAction(Keyboard.Key key, ActionType actionType, Action<KeyEventArgs> action) 
        => BindDelegate(KeyBindings, key, actionType, action);

    /// <summary>
    /// Binds a mouse button to an action with no parameters.
    /// </summary>
    /// <param name="button">The mouse button to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the button is triggered.</param>
    public static void BindAction(Mouse.Button button, ActionType actionType, Action action)
        => BindDelegate(MouseBindings, button, actionType, action);

    /// <summary>
    /// Binds a mouse button to an action that receives mouse button event arguments.
    /// </summary>
    /// <param name="button">The mouse button to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The action to execute when the button is triggered.</param>
    public static void BindAction(Mouse.Button button, ActionType actionType, Action<MouseButtonEventArgs> action)
        => BindDelegate(MouseBindings, button, actionType, action);

    /// <summary>
    /// Binds a delegate to an input action.
    /// </summary>
    /// <typeparam name="T">The type of input (Keyboard.Key or Mouse.Button).</typeparam>
    /// <param name="bindings">The dictionary of bindings to add to.</param>
    /// <param name="input">The input to bind.</param>
    /// <param name="actionType">The type of action to bind.</param>
    /// <param name="action">The delegate to bind.</param>
    private static void BindDelegate<T>(IDictionary<T, Dictionary<ActionType, List<Delegate>>> bindings, T input,
        ActionType actionType, Delegate action)
    {
        if (!bindings.TryGetValue(input, out var actionBindings))
        {
            actionBindings = new Dictionary<ActionType, List<Delegate>>()
            {
                { ActionType.Pressed, [] },
                { ActionType.Released, [] },
                { ActionType.Held, [] }
            };
            bindings[input] = actionBindings;
        }

        actionBindings[actionType].Add(action);
    }

    /// <summary>
    /// Invokes all actions bound to a specific input and action type.
    /// </summary>
    /// <typeparam name="T">The type of input (Keyboard.Key or Mouse.Button).</typeparam>
    /// <param name="bindings">The dictionary of bindings to check.</param>
    /// <param name="input">The input that triggered the action.</param>
    /// <param name="actionType">The type of action to invoke.</param>
    /// <param name="parameters">Optional parameters to pass to the action.</param>
    private static void InvokeActions<T>(IReadOnlyDictionary<T, Dictionary<ActionType, List<Delegate>>> bindings, T input,
        ActionType actionType, params object[] parameters)
    {
        if (!bindings.TryGetValue(input, out var actionBindings) ||
            !actionBindings.TryGetValue(actionType, out var actions))
            return;

        actions.ForEach(a =>
        {
            if (a.Method.GetParameters().Length == parameters.Length)
                a.DynamicInvoke(parameters);
            else
                a.DynamicInvoke();
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
    /// Clears all input bindings for both keyboard and mouse.
    /// </summary>
    public static void ClearBindings()
    {
        KeyBindings.Clear();
        MouseBindings.Clear();
    }
}