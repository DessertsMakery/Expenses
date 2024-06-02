using System.Reflection;

namespace DessertsMakery.Common.Utility.Helpers;

public static class AssemblyHelper
{
    public static Assembly[] LoadAssemblies<T>(string? prefix = null) => LoadAssemblies(typeof(T).Assembly, prefix);

    public static Assembly[] LoadAssemblies(Assembly mainAssembly, string? prefix = null)
    {
        var loaded = new HashSet<string>();
        var container = new HashSet<Assembly> { mainAssembly };

        return LoadAssemblies(mainAssembly, loaded, container, prefix);
    }

    private static Assembly[] LoadAssemblies(
        Assembly current,
        ISet<string> loaded,
        ICollection<Assembly> container,
        string? prefix
    )
    {
        IEnumerable<AssemblyName> referencedAssemblies = current.GetReferencedAssemblies();
        if (prefix is not null)
        {
            referencedAssemblies = referencedAssemblies.Where(x => x.Name!.StartsWith(prefix));
        }

        foreach (var referencedAssembly in referencedAssemblies)
        {
            var referencedAssemblyName = referencedAssembly.ToString();
            if (loaded.Contains(referencedAssemblyName))
            {
                continue;
            }

            var assembly = Assembly.Load(referencedAssembly);
            LoadAssemblies(assembly, loaded, container, prefix);
            loaded.Add(referencedAssemblyName);
            container.Add(assembly);
        }

        return container.ToArray();
    }
}
