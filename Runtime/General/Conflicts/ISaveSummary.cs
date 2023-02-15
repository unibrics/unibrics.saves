namespace Unibrics.Saves.Conflicts
{
    using System;

    public interface ISaveSummary
    {
        DateTime CreationTime { get;  }
        
        string DeviceFingerprint { get; }
        
        SaveCompareResult CompareTo(ISaveSummary summary);
    }
    
    public enum SaveCompareResult
    {
        Unknown,
        Greater,
        Less,
        Equal
    }

}