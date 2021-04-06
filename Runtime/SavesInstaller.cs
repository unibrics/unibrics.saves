namespace Unibrics.Saves
{
    using Core;
    using Core.Services;
    using General;
    using Tools;

    [Install]
    public class SavesInstaller : ModuleInstaller
    {
        public override void Install(IServicesRegistry services)
        {
            var saveableTuples = Types.AnnotatedWith<SaveableAttribute>().WithParent(typeof(ISaveable));
            foreach (var (type, attr) in saveableTuples)
            {
                
            }
        }
    }
}