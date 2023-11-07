namespace Unibrics.Saves.Quick
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;

    [Saveable(typeof(IQuickSaveables))]
    class QuickSaveables : Saveable<QuickSaveablesSaveData>, IQuickSaveables
    {
        private readonly List<IQuickSaveable> saveables = new();

        private Dictionary<string, object> restoredSaveables = new();

        IQuickSaveable<T> IQuickSaveables.Get<T>(string key)
        {
            var saveable = saveables.FirstOrDefault(quickSaveable => quickSaveable.Key == key);
            if (saveable == null)
            {
                var newSaveable = new QuickSaveable<T>(key);
                saveables.Add(newSaveable);
                if (restoredSaveables.TryGetValue(key, out var val))
                {
                    newSaveable.Value = (T)val;
                    restoredSaveables.Remove(key);
                }

                newSaveable.ValueUpdated += ScheduleSave;
                return newSaveable;
            }
            if (saveable is not IQuickSaveable<T> qs)
            {
                throw new Exception($"There is already another quick saveable with key '{key}' but other type");
            }

            return qs;
        }

        public override QuickSaveablesSaveData Serialize()
        {
            var dictionary = restoredSaveables.ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var saveable in saveables)
            {
                dictionary[saveable.Key] = saveable.GenericValue;
            }
            return new QuickSaveablesSaveData()
            {
                Values = dictionary
            };
        }

        public override void Deserialize(QuickSaveablesSaveData save, DateTime lastSaveTime)
        {
            if (save.Values != null)
            {
                restoredSaveables = save.Values;    
            }
        }
    }

    class QuickSaveablesSaveData : SaveComponent
    {
        protected override string Name => "Quick";
        
        public Dictionary<string, object> Values { get; set; }
    }
}