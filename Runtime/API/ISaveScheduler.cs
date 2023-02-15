namespace Unibrics.Saves.API
{
    using UnityEngine;

    public interface ISaveScheduler
    {
        /// <summary>
        /// Request game state saving.
        /// Actual save will be performed later during current frame
        /// </summary>
        /// <param name="saveGroup"></param>
        void RequestSave(string saveGroup);

        /// <summary>
        /// Request immediate state saving
        /// </summary>
        void ForceSave();
    }
}