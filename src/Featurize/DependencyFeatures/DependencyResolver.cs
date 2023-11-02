namespace Featurize.SortFeatures;

internal static class DependencyResolver
{
    internal static IEnumerable<T> GetDependencies<T>(T item, HashSet<T> source)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        ArgumentNullException.ThrowIfNull(source, nameof(source));  

        if (item.GetType().IsAssignableTo(typeof(IDependsOn<>)))
        {
            return Array.Empty<T>();
        }

        var dependencies = item.GetType()
            .GetInterfaces()
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDependsOn<>))
            .ToList();

        var deps = new List<T>();
        foreach (var dep in dependencies)
        {
            var depType = dep.GetGenericArguments()[0];
            var d = source.FirstOrDefault(x => x?.GetType() == depType);

            if (d is null)
                throw new InvalidOperationException($"Dependenvy '{dep.Name}' required for '{item.GetType().Name}' has not been registered.");

            if(!d.Equals(item))
                deps.Add(d);
        }

        return deps;
    }


}