namespace Unibrics.Saves
{
    using System;
    using System.Collections.Generic;
    using API;
    using Core.DI;
    using Cysharp.Threading.Tasks;

    public abstract class Saveable<T> : ISaveable<T> where T : SaveComponent, new()
    {
        [Inject]
        public ISaveScheduler SaveScheduler { get; set; }

        string ISaveable.SaveComponentName => new T().GetName();

        public string SaveGroup { get; private set; }

        public void PrepareInitial(List<ISaveComponentsProcessor> saveComponentsProcessors)
        {
            var save = PrepareInitialSave();
            saveComponentsProcessors.ForEach(processor => processor.ProcessNewComponent(save));
            Deserialize(save, DateTime.UtcNow);
        }

        void ISaveable.InitializeSaveGroup(string group)
        {
            SaveGroup = group;
        }

        public virtual UniTask Start()
        {
            return UniTask.CompletedTask;
        }
        
        protected void ScheduleSave(SaveImportance importance = SaveImportance.Simple)
        {
            SaveScheduler.RequestSave(SaveGroup, importance);
        }

        protected virtual T PrepareInitialSave()
        {
            return new T();
        }
        
        public abstract T Serialize();
        

        public abstract void Deserialize(T save, DateTime lastSaveTime);
    }
}