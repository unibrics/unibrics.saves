namespace Unibrics.Saves.Injector
{
    using System;
    using System.Collections.Generic;
    using API;

    interface ISaveInjector
    {
        SaveInjectionResult TryInjectSaves(List<ISaveComponent> saves, ISaveable persistent, DateTime lastSaveTime);
        
        ISaveComponent GetSave(ISaveable persistent);
    }

    public enum SaveInjectionResult
    {
        Success,
        Fail,
        NoSaveFound
    }
}