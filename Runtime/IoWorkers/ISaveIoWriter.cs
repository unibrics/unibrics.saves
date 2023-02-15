namespace Unibrics.Saves.IoWorkers
{
    using Cysharp.Threading.Tasks;

    public interface ISaveIoWriter
    {
        UniTask<bool> Write(byte[] data);
    }
}