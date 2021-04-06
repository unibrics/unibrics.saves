namespace Unibrics.Saves.General
{
    using System;

    public abstract class Saveable<T> : ISaveable<T> where T : SaveComponent, new()
    {
        public void PrepareInitial()
        {
            Deserialize(PrepareInitialSave(), DateTime.UtcNow);
        }

        public virtual void Start()
        {
        }
        
        protected void ScheduleSave()
        {
            //SaveScheduler.RequestSave();
        }

        protected virtual T PrepareInitialSave()
        {
            return new T();
        }
        
        public abstract T Serialize();

        public abstract void Deserialize(T save, DateTime lastSaveTime);
    }
}