namespace Unibrics.Saves.Injector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Core.DI;
    using UnityEngine;

    abstract class SaveComponentInjector
    {
        public abstract bool CanWorkWith(ISaveable persistent);

        public abstract bool TryInject(ISaveComponent save, ISaveable persistent, DateTime lastSaveTime);

        public abstract ISaveComponent GetSaveToInject(List<ISaveComponent> saves);

        public abstract ISaveComponent ExtractSave(ISaveable persistent);
    }
    
    class SaveComponentInjector<T> : SaveComponentInjector where T : class, ISaveComponent
    {
        [Inject]
        public List<ISaveComponentsProcessor> Processors { get; set; }
        
        public override bool CanWorkWith(ISaveable persistent)
        {
            return persistent is ISaveable<T>;
        }

        public override bool TryInject(ISaveComponent save, ISaveable persistent, DateTime lastSaveTime)
        {
            var typedSave = save as T;
            var typedPersistent = persistent as ISaveable<T>;
            if (typedSave == null || typedPersistent == null)
            {
                return false;
            }

            var orderedProcessors = Processors.OrderByDescending(processor => processor.Priority).ToList();
            try
            {
                orderedProcessors.ForEach(processor => processor.ProcessExistingComponent(typedSave));
                typedPersistent.Deserialize(typedSave, lastSaveTime);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError("Error during deserializing: " + exception);
                return false;
            }
        }

        public override ISaveComponent GetSaveToInject(List<ISaveComponent> saves)
        {
            return saves.OfType<T>().FirstOrDefault();
        }

        public override ISaveComponent ExtractSave(ISaveable persistent)
        {
            var typedPersistent = persistent as ISaveable<T>;
            if (typedPersistent == null)
            {
                throw new Exception($"{persistent} is not IPersistent<{typeof(T)}>");
            }
            return typedPersistent.Serialize();
        }
    }
}