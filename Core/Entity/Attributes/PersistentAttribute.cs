namespace Core.Entity.Attributes;

/// <summary>
/// Marks a component field whose value should survive scene switches.
/// The first time a loaded scene exposes the field, its component is registered as the single shared instance for its runtime type. Every later scene that declares a
/// <see cref="PersistentAttribute"/> field of that same type receives the shared instance instead,
/// the field's freshly constructed value is discarded and the field is repointed at the shared one.
/// Persistent components keep their state across switches and are started only once.
/// </summary>
/// <remarks>
/// A persistent component is spared when its scene is torn down, so any input it binds in
/// <c>Start</c> must use the global binding overloads (see <c>InputManager.BindGlobalAction</c>);
/// scene-scoped bindings are cleared on every switch and would not survive.
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public sealed class PersistentAttribute : Attribute;
