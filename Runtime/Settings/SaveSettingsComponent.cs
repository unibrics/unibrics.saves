namespace Unibrics.Saves.Settings
{
    using System.Collections.Generic;
    using Core;
    using Core.Config;
    using Format;
    using Groups;

    [InstallWithId("saves")]
    class SaveSettingsSection : IAppSettingsSection, ISaveFormatVersionProvider, ISaveGroupProvider
    {
        public int MinimumSupportedSaveFormatVersion { get; set; }
        
        public int SaveFormatVersion { get; set; }
        
        public SavePipelineSettings Pipeline { get; set; }
        
        public List<SavePipelineSettings> AcceptablePipelines { get; set; }
        
        public Dictionary<string, string> SaveComponentGroups { get; set; }
        
        public Dictionary<string, List<string>> GroupsDependencies { get; set; }

        private const string DefaultGroup = "main";
        
        public string GetGroupFor(string component)
        {
            if (SaveComponentGroups != null && SaveComponentGroups.TryGetValue(component, out var group))
            {
                return group;
            }

            return DefaultGroup;
        }
    }

    class SavePipelineSettings
    {
        public string Id { get; set; }
        
        public string[] Stages { get; set; }
    }
}