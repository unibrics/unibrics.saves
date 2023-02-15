namespace Unibrics.Saves
{
    using System;
    using API;
    using Core.DI;
    using Cysharp.Threading.Tasks;

    public abstract class Saveable<T> : ISaveable<T> where T : SaveComponent, new()
    {
        [Inject]
        public ISaveScheduler SaveScheduler { get; set; }
        
        public void PrepareInitial()
        {
            Deserialize(PrepareInitialSave(), DateTime.UtcNow);
        }

        public virtual UniTask Start()
        {
            return UniTask.CompletedTask;
        }
        
        protected void ScheduleSave()
        {
            SaveScheduler.RequestSave();
        }

        protected virtual T PrepareInitialSave()
        {
            return new T();
        }
        
        public abstract T Serialize();

        public abstract void Deserialize(T save, DateTime lastSaveTime);
    }
}