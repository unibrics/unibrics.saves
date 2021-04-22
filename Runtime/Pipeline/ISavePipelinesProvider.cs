namespace Unibrics.Saves.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Config;
    using Settings;

    interface ISavePipelinesProvider
    {
        ISavePipeline GetDefaultPipeline();

        List<ISavePipeline> GetAcceptablePipelines();
    }

    class SettingsBasedSavePipelinesProvider : ISavePipelinesProvider
    {
        private readonly ISavePipelineFactory factory;
        
        private readonly SaveSettingsSection settings;
        
        public SettingsBasedSavePipelinesProvider(IAppSettings appSettings, ISavePipelineFactory factory)
        {
            this.factory = factory;
            settings = appSettings.Get<SaveSettingsSection>();
        }

        public ISavePipeline GetDefaultPipeline()
        {
            return factory.CreatePipeline(settings.Pipeline);
        }

        public List<ISavePipeline> GetAcceptablePipelines()
        {
            return settings.AcceptablePipelines.Select(ps => factory.CreatePipeline(ps)).ToList();
        }
    }
}