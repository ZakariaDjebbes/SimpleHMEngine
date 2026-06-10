namespace Core.Entity.Attributes;

/// <summary>
/// Marks a component field that the scene's automatic component discovery should ignore. The reference itself is untouched; only the automatic registration is skipped.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class DetachedAttribute : Attribute;
