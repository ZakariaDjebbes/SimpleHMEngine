namespace Core.Input;

/// <summary>
/// Represents the type of input action that can be performed.
/// </summary>
public enum ActionType
{
    /// <summary>
    /// The input is pressed down.
    /// </summary>
    Pressed,

    /// <summary>
    /// The input is released.
    /// </summary>
    Released,

    /// <summary>
    /// The input is being held down.
    /// </summary>
    Held
}