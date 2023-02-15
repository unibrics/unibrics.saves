namespace Unibrics.Saves.Model
{
    using System;
    using UnityEngine;

    public class SerializationHeader
    {
        public DateTime Timestamp { get; }

        public int FormatVersion { get; }
        
        public string BuildVersion { get; } 

        public SerializationHeader(DateTime timestamp, int formatVersion)
        {
            Timestamp = timestamp;
            BuildVersion = Application.version;
            FormatVersion = formatVersion;
        }
    }
}