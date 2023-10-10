namespace Unibrics.Saves.API
{
    /// <summary>
    /// interface for partial saves such as DailyBonus, Tutorial, Wallet etc.
    /// which must be combined into one big SaveModel and saved
    /// </summary>
    public interface ISaveComponent
    {
        /// <summary>
        /// This Name will be used as a top-level key in JSON tree of save.
        /// </summary>
        string GetName();
    }

    public abstract class SaveComponent : ISaveComponent
    {
        public string GetName() => Name;
        
        protected abstract string Name { get; }
    }
}