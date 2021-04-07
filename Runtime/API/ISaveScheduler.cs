namespace Unibrics.Saves.API
{
    using UnityEngine;

    public interface ISaveScheduler
    {
        /// <summary>
        /// Request game state saving.
        /// Actual save will be performed later during current frame
        /// </summary>
        void RequestSave();

        /// <summary>
        /// Request immediate state saving
        /// </summary>
        void ForceSave();
    }
}