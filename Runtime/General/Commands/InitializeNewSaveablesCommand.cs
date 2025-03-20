namespace Unibrics.Saves.Commands
{
    using API;
    using Core.Execution;

    /// <summary>
    /// In case one or more saveables weren't initialize with a save, they a initialized here
    /// This is separate step because configs may be needed for it
    /// </summary>
    public class InitializeNewSaveablesCommand<TGroup> : ExecutableCommand where TGroup : ISaveablesGroup
    {
        private readonly INewSaveablesInitializer initializer;

        InitializeNewSaveablesCommand(INewSaveablesInitializer initializer)
        {
            this.initializer = initializer;
        }

        protected override void ExecuteInternal()
        {
            initializer.InitializeComponentsWithoutSaves<TGroup>();
        }
    }
}