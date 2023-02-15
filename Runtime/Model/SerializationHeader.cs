namespace Unibrics.Saves.Model
{
    using System;
    using UnityEngine;

    public class SerializationHeader
    {
        public DateTime Timestamp { get; }

        public int FormatVersion { get; }
        
        public string BuildVersion { get; } 
        
        public string GroupName { get; }

        public SerializationHeader(DateTime timestamp, string groupName, int formatVersion)
        {
            Timestamp = timestamp;
            BuildVersion = Application.version;
            GroupName = groupName;
            FormatVersion = formatVersion;
        }
    }
}