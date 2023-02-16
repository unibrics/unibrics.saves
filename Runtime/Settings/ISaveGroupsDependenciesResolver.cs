namespace Unibrics.Saves.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Some save groups are leading to saving other groups
    /// E.g., save Wallet when Quests are saving, but not vice versa
    /// This interface provides config validation (check for circular dependencies) and resolves full list
    /// of save groups to save 
    /// </summary>
    public interface ISaveGroupsDependenciesResolver
    {
        void ValidateAndPrepare(Dictionary<string, List<string>> dependencies);
        
        List<string> ResolveGroupsToSave(List<string> groupsToSave);
    }

    class SaveGroupsDependenciesResolver : ISaveGroupsDependenciesResolver
    {
        private readonly IDictionary<string, List<string>> fullDependencies = new Dictionary<string, List<string>>();

        public void ValidateAndPrepare(Dictionary<string, List<string>> dependencies)
        {
            if (dependencies == null)
            {
                return;
            }
            
            fullDependencies.Clear();
            var permanent = new List<string>();
            var temporary = new List<string>();
            foreach (var dependency in dependencies)
            {
                var depsForKey = new List<string>();
                Visit(dependency.Key, depsForKey);
                fullDependencies[dependency.Key] = depsForKey;
            }

            void Visit(string node, List<string> depsForKey)
            {
                if (permanent.Contains(node))
                {
                    return;
                }

                if (temporary.Contains(node))
                {
                    throw new Exception(
                        $"Circular dependency found!'{node}' is already on the list");
                }
                
                temporary.Add(node);
                if (dependencies.TryGetValue(node, out var directDependencies))
                {
                    foreach (var directDependency in directDependencies)
                    {
                        depsForKey.Add(directDependency);
                        Visit(directDependency, depsForKey);
                    }
                }

                temporary.Remove(node);
                permanent.Add(node);
            }
            
        }

        public List<string> ResolveGroupsToSave(List<string> groupsToSave)
        {
            var allGroups = new List<string>();
            foreach (var group in groupsToSave)
            {
                if (fullDependencies.TryGetValue(group, out var dependencies))
                {
                    allGroups.AddRange(dependencies);
                }
            }

            return allGroups.Distinct().ToList();
        }
    }
}