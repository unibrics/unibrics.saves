namespace Unibrics.Saves
{
    using API;
    using Core.DI;

    class SaveScheduler : ISaveScheduler, ITickable
    {
        private bool saveRequested;
        
        public void RequestSave()
        {
            //we are processing request only once in frame, to avoid extra saves in one frame
            saveRequested = true;
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
            PerformSave();
        }

        private void PerformSave()
        {
            
        }
    }
}