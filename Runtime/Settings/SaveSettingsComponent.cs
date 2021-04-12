namespace Unibrics.Saves.Settings
{
    using Core;
    using Core.Config;

    [InstallWithId("saves")]
    class SaveSettingsSection : IAppSettingsSection
    {
        public SavePipelineSettings Pipeline { get; set; }
    }

    class SavePipelineSettings
    {
        public string Id { get; set; }
        
        public string[] Stages { get; set; }
    } 
}