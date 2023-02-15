namespace Unibrics.Saves.Commands
{
    using Core.Execution;

    public class InitializeNewSaveablesCommand : ExecutableCommand
    {
        private INewSaveablesInitializer initializer;

        InitializeNewSaveablesCommand(INewSaveablesInitializer initializer)
        {
            this.initializer = initializer;
        }

        protected override void ExecuteInternal()
        {
            initializer.InitializeComponentsWithoutSaves();
        }
    }
}