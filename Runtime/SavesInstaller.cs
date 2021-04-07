namespace Unibrics.Saves
{
    using API;
    using Core;
    using Core.DI;
    using Core.Services;
    using Tools;

    [Install]
    public class SavesInstaller : ModuleInstaller
    {
        public override void Install(IServicesRegistry services)
        {
            var saveableTuples = Types.AnnotatedWith<SaveableAttribute>().WithParent(typeof(ISaveable));
            foreach (var (attr, type) in saveableTuples)
            {
                var typesToBind = attr.BindTo;
                typesToBind.Add(typeof(ISaveable));
                
                services.AddSingleton(typesToBind, type);
            }
            
            services.AddSingleton<SaveScheduler>(typeof(ISaveScheduler), typeof(ITickable));
        }
    }
}