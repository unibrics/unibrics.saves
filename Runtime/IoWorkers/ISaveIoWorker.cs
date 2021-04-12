namespace Unibrics.Saves.IoWorkers
{
    using Cysharp.Threading.Tasks;

    /// <summary>
    /// This interface implementations should not know anything about
    /// cyphers, data models, jsons, serialization etc. - only read\write, input\output
    /// </summary>
    public interface ISaveIoWorker : ISaveIoReader, ISaveIoWriter
    {
    }

    public interface ISaveIoReader
    {
        UniTask<byte[]> Read();
    }

    public interface ISaveIoWriter
    {
        UniTask<bool> Write(byte[] data);
    }
}