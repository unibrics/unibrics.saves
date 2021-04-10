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
    interface ISaveProcessor
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
        
        public void LoadFromSave(ISaveObject saveObject)
        {
            throw new System.NotImplementedException();
        }

        public SaveModel GetSaveForCurrentState()
        {
            var header = new SerializationHeader(DateTime.UtcNow, 1);
            return new SaveModel(header, Saveables.Select(saveable => Injector.GetSave(saveable)).ToList());
        }
    }
}