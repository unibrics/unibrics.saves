namespace Unibrics.Saves.Commands
{
    using API;
    using Blocker;
    using Core.DI;
    using Core.Execution;

    public class UnblockSavesCommand : ExecutableCommand
    {
        [Inject]
        public ISaveContext SaveContext { get; set; }
        
        protected override void ExecuteInternal()
        {
            SaveContext.Unblock(SaveBlockingContext.Initial);
        }
    }
}