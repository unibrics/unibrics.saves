namespace Unibrics.Saves.Settings
{
    using Core;
    using Core.Config;

    [InstallWithId("saves")]
    public class SaveSettingsComponent : IAppSettingsComponent
    {
        public string[] Pipeline { get; set; }
    }
}