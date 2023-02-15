namespace Unibrics.Saves.API
{
    using System;
    using Cysharp.Threading.Tasks;

    interface ISaveable
    {
        /// <summary>
        /// Called on first load to create default save
        /// </summary>
        void PrepareInitial();

        /// <summary>
        /// Call after successful loading AND all configs are ready 
        /// </summary>
        UniTask Start();
    }
    
    interface ISaveable<T> : ISaveable where T : ISaveComponent
    {
        T Serialize();

        void Deserialize(T save, DateTime lastSaveTime);
    }
}