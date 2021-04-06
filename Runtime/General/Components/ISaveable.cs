namespace Unibrics.Saves.General
{
    using System;

    public interface ISaveable
    {
        
    }
    
    public interface ISaveable<T> : ISaveable where T : SaveComponent
    {
        T Serialize();

        void Deserialize(T save, DateTime lastSaveTime);
    }
}