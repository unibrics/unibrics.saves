namespace Unibrics.Saves
{
    using API;

    internal interface INewSaveablesInitializer
    {
        void InitializeComponentsWithoutSaves<T>() where T : ISaveablesGroup;
    }
}