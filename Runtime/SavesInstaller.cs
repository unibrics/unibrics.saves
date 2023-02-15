namespace Unibrics.Saves
{
    using System.Linq;
    using API;
    using Conflicts;
    using Conflicts.Auto;
    using Conflicts.Manual;
    using Core;
    using Core.DI;
    using Core.Launchers;
    using Core.Services;
    using Format;
    using Groups;
    using Injector;
    using IoWorkers;
    using Pipeline;
    using Settings;
    using Tools;
    using Utils;

    [Install]
    public class SavesInstaller : ModuleInstaller
    {
        public override void Install(IServicesRegistry services)
        {
            var settings = AppSettings.Get<SaveSettingsSection>();
            services.Add<ISaveFormatVersionProvider, ISaveGroupProvider>().ImplementedByInstance(settings);


            BindSaveables();

            services.Add(typeof(ISaveScheduler), typeof(ITickable)).ImplementedBy<SaveScheduler>().AsSingleton();
            services.Add<ISaveProcessor, INewSaveablesInitializer>().ImplementedBy<SaveProcessor>().AsSingleton();
            services.Add<ISaveInjector>().ImplementedBy<SaveInjector>().AsSingleton();
            services.Add<IDebugSaveWriter>().ImplementedBy<DebugSaveWriter>().AsSingleton();
            services.Add<IAutoConflictSolver>().ImplementedBy<AutoConflictSolver>().AsSingleton();
            services.Add<ISaveSummaryExtractor>().ImplementedBy<SimpleSaveSummaryExtractor>().AsSingleton();
            services.Add<ICombinedSaveConflictSolver>().ImplementedBy<CombinedSaveConflictSolver>().AsSingleton();
            services.Add<ISaveIoWorker, ISaveIoWorkersProvider, ISaveWorkersConfigurator>().ImplementedBy<ComboSaveWorker>().AsSingleton();
            services.Add<IFirstSessionChecker>().ImplementedBy<SimpleFirstSessionChecker>().AsSingleton();
            services.Add<ISaveWriter>().ImplementedBy<SaveWriter>().AsSingleton();
            services.Add<ISavePipelinesProvider>().ImplementedBy<SettingsBasedSavePipelinesProvider>().AsSingleton();
            services.Add<ISavePipelineFactory>().ImplementedBy<SavePipelineFactory>().AsSingleton();
            services.Add<ISaveModelSerializer>().ImplementedBy<MultiPipelineSaveModelSerializer>().AsSingleton();
            services.Add<ISaveFormatConverter>().ImplementedBy<SaveFormatConverter>().AsSingleton();
            services.Add<IIncrementalSaveConvertersProvider>().ImplementedBy<IncrementalSaveConvertersProvider>()
                .AsSingleton();
            
            // things that can be rebound
            services.Add<IManualConflictSolver>().ImplementedBy<LocalChoosingSolver>().AsSingleton();
            
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