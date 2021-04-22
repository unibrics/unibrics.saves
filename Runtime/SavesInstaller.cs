namespace Unibrics.Saves
{
    using System.Linq;
    using API;
    using Core;
    using Core.DI;
    using Core.Launchers;
    using Core.Services;
    using Format;
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
            var settings = AppSettings.Get<SaveSettingsSection>();

            BindSaveables();

            services.Add(typeof(ISaveScheduler), typeof(ITickable)).ImplementedBy<SaveScheduler>().AsSingleton();
            services.Add<ISaveProcessor>().ImplementedBy<SaveProcessor>().AsSingleton();
            services.Add<ISaveInjector>().ImplementedBy<SaveInjector>().AsSingleton();
            services.Add<ISaveWriter>().ImplementedBy<SaveWriter>().AsSingleton();
            services.Add<ISaveIoWorker>().ImplementedBy<LocalSaveWorker>().AsSingleton();
            services.Add<ISaveFormatVersionProvider>().ImplementedByInstance(settings);
            services.Add<ISavePipelinesProvider>().ImplementedBy<SettingsBasedSavePipelinesProvider>().AsSingleton();
            services.Add<ISavePipelineFactory>().ImplementedBy<SavePipelineFactory>().AsSingleton();
            services.Add<ISaveModelSerializer>().ImplementedBy<MultiPipelineSaveModelSerializer>().AsSingleton();
            services.Add<ISaveFormatConverter>().ImplementedBy<SaveFormatConverter>().AsSingleton();
            services.Add<IIncrementalSaveConvertersProvider>().ImplementedBy<IncrementalSaveConvertersProvider>()
                .AsSingleton();

            void BindSaveables()
            {
                var saveableTuples = Types.AnnotatedWith<SaveableAttribute>().WithParent(typeof(ISaveable));
                foreach (var (attr, type) in saveableTuples)
                {
                    var typesToBind = attr.BindTo;
                    typesToBind.Add(typeof(ISaveable));

                    services.Add(typesToBind.ToArray()).ImplementedBy(type).AsSingleton();
                }
            }
        }
    }
}