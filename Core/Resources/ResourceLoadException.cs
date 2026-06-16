namespace Core.Resources;

/// <summary>
/// Thrown when a resource fails to load and no fallback is registered for its type.
/// Wraps the underlying loader exception and records what was being loaded.
/// </summary>
/// <param name="resourceType">The resource type that failed to load.</param>
/// <param name="identifier">The path, cache key or manifest name the load was attempted from.</param>
/// <param name="inner">The underlying exception thrown by the loader.</param>
public sealed class ResourceLoadException(Type resourceType, string identifier, Exception inner)
    : Exception($"Failed to load {resourceType.Name} from '{identifier}'.", inner)
{
    /// <summary>The resource type that failed to load.</summary>
    public Type ResourceType { get; } = resourceType;

    /// <summary>The path, cache key or manifest name the load was attempted from.</summary>
    public string Identifier { get; } = identifier;
}
