namespace Unibrics.Saves.Model
{
    using System;

    public class SerializationHeader
    {
        public DateTime Timestamp { get; }

        public int FormatVersion { get; }

        public SerializationHeader(DateTime timestamp, int formatVersion)
        {
            Timestamp = timestamp;
            FormatVersion = formatVersion;
        }
    }
}