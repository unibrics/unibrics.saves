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

        string ISaveable.SaveComponentName => new T().GetName();

        public string SaveGroup { get; private set; }

        public void PrepareInitial()
        {
            Deserialize(PrepareInitialSave(), DateTime.UtcNow);
        }

        void ISaveable.InitializeSaveGroup(string group)
        {
            SaveGroup = group;
        }

        public virtual UniTask Start()
        {
            return UniTask.CompletedTask;
        }
        
        protected void ScheduleSave()
        {
            SaveScheduler.RequestSave(SaveGroup);
        }

        protected virtual T PrepareInitialSave()
        {
            return new T();
        }
        
        public abstract T Serialize();
        

        public abstract void Deserialize(T save, DateTime lastSaveTime);
    }
}