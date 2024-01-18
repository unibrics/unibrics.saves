namespace Unibrics.Saves.IoWorkers
{
    using System.Collections.Generic;
    using API;

    /// <summary>
    /// This interface implementations should not know anything about
    /// cyphers, data models, jsons, serialization etc. - only read\write, input\output
    /// </summary>
    public interface ISaveIoWorker : ISaveIoReader, ISaveIoWriter
    {
    }
    
    public interface ILocalSaveIoWorker : ISaveIoWorker
    {
        IEnumerable<byte[]> ReadSync();

        bool WriteSync(string saveGroup, byte[] data);
    }
}