namespace Featurize.SortFeatures;

internal static class HashSetExtensions
{
    internal static HashSet<T> Sort<T>(this HashSet<T> source, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle = false)
    {
        var sorted = new HashSet<T>();
        var visited = new HashSet<T>();

        foreach (var item in source)
        {
            Visit(item, visited, sorted, dependencies, throwOnCycle);
        }

        return sorted;
    }

    private static void Visit<T>(T item, HashSet<T> visited, HashSet<T> sorted, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle = false)
    {
        ArgumentNullException.ThrowIfNull(item, "item");

        if(!visited.Contains(item))
        {
            visited.Add(item);

            foreach(var dependency in dependencies(item))
            {
                Visit(dependency, visited, sorted, dependencies, throwOnCycle);
            }

            sorted.Add(item);
        } else
        {
            if (throwOnCycle && !sorted.Contains(item))
                throw new InvalidOperationException($"Feature: '{item.GetType().Name}' has cyclic dependency.");
        }
    }
}
