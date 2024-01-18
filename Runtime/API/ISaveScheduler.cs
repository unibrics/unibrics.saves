namespace Unibrics.Saves.API
{
    public interface ISaveScheduler
    {
        /// <summary>
        /// Request game state saving.
        /// Actual save will be performed later during current frame
        /// </summary>
        void RequestSave(string saveGroup, SaveImportance importance = SaveImportance.Simple);

        /// <summary>
        /// Request immediate state saving
        /// </summary>
        void ForceSave();
    }
    
    public enum SaveImportance
    {
        /// <summary>
        /// This is a regular save
        /// </summary>
        Simple,
        
        /// <summary>
        /// This is a important save (performed after transaction or important in-game event)
        /// </summary>
        Important
    }
}