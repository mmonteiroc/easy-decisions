using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace EasyDecisions;

/// <summary>
/// Factory for instantiating decisions that have been registered via the <see cref="DecisionFabricatorAttribute"/>.
/// </summary>
public static class DecisionFactory
{
    private static readonly ConcurrentDictionary<string, Type> _fabricatorTypes = new();
    private static bool _initialized = false;
    private static readonly object _initLock = new();

    /// <summary>
    /// Explicitly registers all decision fabricators found in the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    public static void RegisterAssembly(Assembly assembly)
    {
        try
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.GetCustomAttribute<DecisionFabricatorAttribute>() != null);

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<DecisionFabricatorAttribute>();
                if (attr != null)
                {
                    _fabricatorTypes[attr.Name] = type;
                }
            }
        }
        catch (ReflectionTypeLoadException)
        {
            // Ignore assemblies that cannot be fully loaded
        }
    }

    private static void EnsureInitialized()
    {
        if (!_initialized)
        {
            lock (_initLock)
            {
                if (!_initialized)
                {
                    // Scan all assemblies in the current AppDomain
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        // Skip well-known system assemblies to improve startup time
                        if (!assembly.IsDynamic && 
                            assembly.FullName != null &&
                            !assembly.FullName.StartsWith("System") && 
                            !assembly.FullName.StartsWith("Microsoft") &&
                            !assembly.FullName.StartsWith("mscorlib"))
                        {
                            RegisterAssembly(assembly);
                        }
                    }
                    _initialized = true;
                }
            }
        }
    }

    /// <summary>
    /// Creates a new instance of a decision based on its registered name.
    /// </summary>
    /// <typeparam name="TInput">The input type of the decision.</typeparam>
    /// <typeparam name="TOutput">The output type of the decision.</typeparam>
    /// <param name="name">The name defined in the <see cref="DecisionFabricatorAttribute"/>.</param>
    /// <returns>The created decision instance.</returns>
    /// <exception cref="ArgumentException">Thrown if the decision name is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the fabricator does not match the expected generic types.</exception>
    public static Decision<TInput, TOutput> Create<TInput, TOutput>(string name) where TOutput : new()
    {
        EnsureInitialized();

        if (!_fabricatorTypes.TryGetValue(name, out var type))
        {
            throw new ArgumentException($"No decision fabricator found with name '{name}'. Make sure the class has a [DecisionFabricator(\"{name}\")] attribute.");
        }

        if (!typeof(IDecisionFabricator<TInput, TOutput>).IsAssignableFrom(type))
        {
            throw new InvalidOperationException($"The fabricator for '{name}' does not implement IDecisionFabricator<{typeof(TInput).Name}, {typeof(TOutput).Name}>.");
        }

        var fabricator = (IDecisionFabricator<TInput, TOutput>)Activator.CreateInstance(type)!;
        return fabricator.Create();
    }
}
