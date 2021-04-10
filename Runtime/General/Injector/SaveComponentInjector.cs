namespace Unibrics.Saves.Injector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
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

            try
            {
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