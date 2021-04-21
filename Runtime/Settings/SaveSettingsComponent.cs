namespace Unibrics.Saves.Settings
{
    using System.Collections.Generic;
    using Core;
    using Core.Config;
    using Format;

    [InstallWithId("saves")]
    class SaveSettingsSection : IAppSettingsSection, ISaveFormatVersionProvider
    {
        public int MinimumSupportedSaveFormatVersion { get; set; }
        
        public int SaveFormatVersion { get; set; }
        
        public SavePipelineSettings Pipeline { get; set; }
        
        public List<SavePipelineSettings> AcceptablePipelines { get; set; }
    }

    class SavePipelineSettings
    {
        public string Id { get; set; }
        
        public string[] Stages { get; set; }
    }
}