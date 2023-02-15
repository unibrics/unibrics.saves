namespace Unibrics.Saves.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Core.DI;
    using Core.Execution;
    using Cysharp.Threading.Tasks;

    public class StartSaveablesCommand : ExecutableCommand
    {
        [Inject]
        List<ISaveable> Saveables { get; set; }
        
        protected override async void ExecuteInternal()
        {
            Retain();
            await Enumerable.Select(Saveables, saveable => saveable.Start()).ToList();
            ReleaseAndComplete();
        }
    }
}