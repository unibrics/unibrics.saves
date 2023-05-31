namespace Unibrics.Saves
{
    using System.Collections.Generic;
    using API;
    using Core.DI;
    using Core.Features;

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

        public SaveScheduler(ISaveBlocker saveBlocker)
        {
            this.saveBlocker = saveBlocker;
            saveBlocker.SaveUnblocked += ForceSave;
        }

        public void RequestSave(string saveGroup)
        {
            //we are processing request only once in frame, to avoid extra saves in one frame
            saveRequested = true;
            requestedGroups.Add(saveGroup);
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
                Writer.Write(saveModel);
            }
        }
    }
}