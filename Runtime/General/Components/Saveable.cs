namespace Unibrics.Saves
{
    using System;
    using System.Collections.Generic;
    using API;
    using Core.DI;
    using Cysharp.Threading.Tasks;
    

    public abstract class Saveable<TComponent, TGroup> : ISaveable<TComponent>,  ISaveableWithinGroup<TGroup>
        where TComponent : SaveComponent, new()
        where TGroup : ISaveablesGroup
    {
        [Inject]
        public ISaveScheduler SaveScheduler { get; set; }

        string ISaveable.SaveComponentName => new TComponent().GetName();

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

        protected virtual TComponent PrepareInitialSave()
        {
            return new TComponent();
        }
        
        public abstract TComponent Serialize();
        

        public abstract void Deserialize(TComponent save, DateTime lastSaveTime);
    }
    
    public abstract class Saveable<TComponent> : Saveable<TComponent, DefaultSaveablesGroup>
        where TComponent : SaveComponent, new()
    {
        
    }
}