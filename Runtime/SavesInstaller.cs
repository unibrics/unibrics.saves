namespace Unibrics.Saves
{
    using API;
    using Core;
    using Core.DI;
    using Core.Services;
    using Injector;
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

            void PreparePipeline()
            {
                var settings = AppSettings.Get<SaveSettingsComponent>();
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