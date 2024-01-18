namespace Unibrics.Saves
{
    using System.Collections.Generic;
    using API;
    using Core.DI;
    using Core.Features;
    using IoWorkers;

    class SaveScheduler : ISaveScheduler, ITickable
    {
        [Inject]
        public ISaveProcessor SaveProcessor { get; set; }

        [Inject]
        public ISaveWriter Writer { get; set; }

        [Inject]
        public IFeatureSet FeatureSet { get; set; }

        private readonly ISaveBlocker saveBlocker;
        private readonly List<string> requestedGroups = new();

        private bool saveRequested;

        private SaveImportance requestedSaveImportance;

        public SaveScheduler(ISaveBlocker saveBlocker)
        {
            this.saveBlocker = saveBlocker;
            saveBlocker.SaveUnblocked += ForceSave;
        }

        public void RequestSave(string saveGroup, SaveImportance importance)
        {
            //we are processing request only once in frame, to avoid extra saves in one frame
            saveRequested = true;
            requestedGroups.Add(saveGroup);
            if (requestedSaveImportance == SaveImportance.Simple && importance == SaveImportance.Important)
            {
                requestedSaveImportance = SaveImportance.Important;
            }
        }

        public void ForceSave()
        {
            PerformSave();
        }

        public void Tick()
        {
            if (!saveRequested)
            {
                return;
            }

            saveRequested = false;
            PerformSave(requestedGroups);
            
            requestedGroups.Clear();
            requestedSaveImportance = SaveImportance.Simple;
        }

        private void PerformSave(List<string> groups = null)
        {
            if (FeatureSet.GetFeature<SavesFeature>().IsSuspended)
            {
                return;
            }

            if (saveBlocker.IsBlocked)
            {
                return;
            }

            foreach (var saveModel in SaveProcessor.GetSavesForCurrentState(groups))
            {
                Writer.Write(saveModel, requestedSaveImportance);
            }
        }
    }
}