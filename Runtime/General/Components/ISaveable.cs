namespace Unibrics.Saves.General
{
    using System;

    interface ISaveable
    {
        /// <summary>
        /// Called on first load to create default save
        /// </summary>
        void PrepareInitial();

        /// <summary>
        /// Call after successful loading AND all configs are ready 
        /// </summary>
        void Start();
    }
    
    interface ISaveable<T> : ISaveable where T : SaveComponent
    {
        T Serialize();

        void Deserialize(T save, DateTime lastSaveTime);
    }
}