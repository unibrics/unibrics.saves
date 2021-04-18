namespace Unibrics.Saves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Core.DI;
    using Injector;
    using Model;

    /// <summary>
    /// This entity is responsible for extracting save from saveable components
    /// and for injecting saves into them
    /// </summary>
    internal interface ISaveProcessor
    {
        void LoadFromSave(ISaveObject saveObject);
        
        SaveModel GetSaveForCurrentState();
    }

    class SaveProcessor : ISaveProcessor
    {
        [Inject]
        public List<ISaveable> Saveables { get; set; }

        [Inject]
        public ISaveInjector Injector { get; set; }
        
        private List<ISaveable> initialSaveables = new List<ISaveable>();
        
        public void LoadFromSave(ISaveObject saveObject)
        {
            if (saveObject.IsInitial)
            {

            }
            else
            {
                TryRestore(saveObject, saveObject.Result.Header.Timestamp);
            }
        }
        
        private void TryRestore(ISaveObject saveObject, DateTime lastSaveTime)
        {
            foreach (var persistent in Saveables)
            {
                var result = Injector.TryInjectSaves(saveObject.Result.Components, persistent, lastSaveTime);
                if (result == SaveInjectionResult.Fail)
                {
                    throw new Exception($"Could not insert into {persistent}");
                }

                if (result == SaveInjectionResult.NoSaveFound)
                {
                    initialSaveables.Add(persistent);
                }
            }
        }

        public SaveModel GetSaveForCurrentState()
        {
            var header = new SerializationHeader(DateTime.UtcNow, 1);
            return new SaveModel(header, Saveables.Select(saveable => Injector.GetSave(saveable)).ToList());
        }
    }
}