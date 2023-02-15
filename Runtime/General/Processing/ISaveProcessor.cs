namespace Unibrics.Saves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Core.DI;
    using Format;
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

    class SaveProcessor : ISaveProcessor, INewSaveablesInitializer
    {
        [Inject]
        public List<ISaveable> Saveables { get; set; }

        [Inject]
        public ISaveInjector Injector { get; set; }
        
        [Inject]
        public ISaveFormatVersionProvider FormatVersionProvider { get; set; }
        
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
            var header = new SerializationHeader(DateTime.UtcNow, FormatVersionProvider.SaveFormatVersion);
            return new SaveModel(header, Saveables.Select(saveable => Injector.GetSave(saveable)).ToList());
        }

        public void InitializeComponentsWithoutSaves()
        {
            foreach (var initialSaveable in initialSaveables)
            {
                initialSaveable.PrepareInitial();
            }
        }
    }
}