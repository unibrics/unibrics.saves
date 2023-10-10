namespace Unibrics.Saves.API
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;

    internal interface ISaveable
    {
        /// <summary>
        /// Called on first load to create default save
        /// </summary>
        /// <param name="saveComponentsProcessors"></param>
        void PrepareInitial(List<ISaveComponentsProcessor> saveComponentsProcessors);

        /// <summary>
        /// Call after successful loading AND all configs are ready 
        /// </summary>
        UniTask Start();
        
        string SaveGroup { get; }
        
        internal string SaveComponentName { get; }

        internal void InitializeSaveGroup(string group);
    }
    
    internal interface ISaveable<T> : ISaveable where T : ISaveComponent
    {
        T Serialize();
        
        void Deserialize(T save, DateTime lastSaveTime);
    }
}