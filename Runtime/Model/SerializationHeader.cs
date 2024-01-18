namespace Unibrics.Saves.Model
{
    using System;
    using Newtonsoft.Json;
    using UnityEngine;

    public class SerializationHeader
    {
        public DateTime Timestamp { get; }

        public int FormatVersion { get; }
        
        public string BuildVersion { get; } 
        
        public string GroupName { get; }
        
        public string DeviceFingerprint { get; } 

        public SerializationHeader(DateTime timestamp, string groupName, int formatVersion, string fingerprint)
        {
            Timestamp = timestamp;
            BuildVersion = Application.version;
            GroupName = groupName;
            FormatVersion = formatVersion;
            DeviceFingerprint = fingerprint;
        }
        
        [JsonConstructor]
        public SerializationHeader(DateTime timestamp, string buildVersion, string groupName, int formatVersion, string fingerprint)
        {
            Timestamp = timestamp;
            BuildVersion = buildVersion;
            GroupName = groupName;
            FormatVersion = formatVersion;
            DeviceFingerprint = fingerprint;
        }
    }
}