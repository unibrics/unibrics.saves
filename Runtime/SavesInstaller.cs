﻿namespace Unibrics.Saves
{
    using System.Linq;
    using API;
    using Core;
    using Core.DI;
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
                services.Add<ISaveFormatVersionProvider>().ImplementedByInstance(settings);
                
                var factory = new SavePipelineFactory();
                services.Add<ISavePipelineFactory>().ImplementedByInstance(factory);
                
                var defaultPipeline = factory.CreatePipeline(settings.Pipeline);
                var acceptablePipelines = settings.AcceptablePipelines.Select(factory.CreatePipeline).ToList();
                var serializer = new MultiPipelineSaveModelSerializer(defaultPipeline, acceptablePipelines);
                services.Add<ISaveModelSerializer>().ImplementedByInstance(serializer);
            }
            
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