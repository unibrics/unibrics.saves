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

            services.AddSingleton<SaveScheduler>(typeof(ISaveScheduler), typeof(ITickable));
            services.AddSingleton<ISaveProcessor, SaveProcessor>();
            services.AddSingleton<ISaveInjector, SaveInjector>();
            services.AddSingleton<ISaveWriter, SaveWriter>();
            services.AddSingleton<ISaveIoWorker, LocalSaveWorker>();

            void PreparePipeline()
            {
                var settings = AppSettings.Get<SaveSettingsSection>();
                var factory = new SavePipelineFactory();
                services.AddSingleton<ISavePipelineFactory>(factory);
                
                var defaultPipeline = factory.CreatePipeline(settings.Pipeline);
                services.AddSingleton<ISavePipeline>(defaultPipeline);
            }
            
            void BindSaveables()
            {
                var saveableTuples = Types.AnnotatedWith<SaveableAttribute>().WithParent(typeof(ISaveable));
                foreach (var (attr, type) in saveableTuples)
                {
                    var typesToBind = attr.BindTo;
                    typesToBind.Add(typeof(ISaveable));

                    services.AddSingleton(typesToBind, type);
                }
            }
        }
    }
}