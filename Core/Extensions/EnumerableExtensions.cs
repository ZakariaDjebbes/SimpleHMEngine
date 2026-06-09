namespace Core.Extensions;

/// <summary>
/// Provides extension methods for working with enumerable collections.
/// </summary>
internal static class EnumerableExtensions
{
    /// <summary>
    /// Executes an action for each element in the source collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source collection.</typeparam>
    /// <param name="source">The source collection to iterate over.</param>
    /// <param name="action">The action to execute for each element.</param>
    /// <exception cref="ArgumentNullException">Thrown when source or action is null.</exception>
    internal static void EnumeratedForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        var enumerable = source.ToList();
        
        enumerable.ForEach(action);
    }
    
    /// <summary>
    /// Removes elements from the source collection that match the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source collection.</typeparam>
    /// <param name="source">The source collection to filter.</param>
    /// <param name="predicate">The function to test each element for a condition.</param>
    /// <returns>An enumerable containing elements that do not match the predicate.</returns>
    /// <exception cref="ArgumentNullException">Thrown when source or predicate is null.</exception>
    internal static IEnumerable<T> RemoveWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        return source.Where(item => !predicate(item));
    }
}