namespace Unibrics.Saves.Groups
{
    using System.Collections.Generic;

    interface ISaveGroupProvider
    {
         string GetGroupFor(string component);
        
         Dictionary<string, List<string>> GroupsDependencies { get; }
    }
}