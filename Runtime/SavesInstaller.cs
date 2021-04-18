namespace Unibrics.Saves
{
    using API;
    using Core;
    using Core.DI;
    using Core.Services;
    using Injector;
    using IoWorkers;
    using Pipeline;
    using Settings;
    using Tools;

    [Install]
    public class SavesInstaller : ModuleInstaller
    {
        public override void Install(IServicesRegistry services)
        {
            PreparePipeline();
            BindSaveables();

            services.Add(typeof(ISaveScheduler), typeof(ITickable)).ImplementedBy<SaveScheduler>().AsSingleton();
            services.Add<ISaveProcessor>().ImplementedBy<SaveProcessor>().AsSingleton();
            services.Add<ISaveInjector>().ImplementedBy<SaveInjector>().AsSingleton();
            services.Add<ISaveWriter>().ImplementedBy<SaveWriter>().AsSingleton();
            services.Add<ISaveIoWorker>().ImplementedBy<LocalSaveWorker>().AsSingleton();

            void PreparePipeline()
            {
                var settings = AppSettings.Get<SaveSettingsSection>();
                var factory = new SavePipelineFactory();
                services.Add<ISavePipelineFactory>().ImplementedByInstance(factory);
                
                var defaultPipeline = factory.CreatePipeline(settings.Pipeline);
                services.Add<ISavePipeline>().ImplementedByInstance(defaultPipeline);
            }
            
            void BindSaveables()
            {
                var saveableTuples = Types.AnnotatedWith<SaveableAttribute>().WithParent(typeof(ISaveable));
                foreach (var (attr, type) in saveableTuples)
                {
                    var typesToBind = attr.BindTo;
                    typesToBind.Add(typeof(ISaveable));

                    services.Add(typesToBind.ToArray()).ImplementedByInstance(type);
                    
                }
            }
        }
    }
}