namespace Unibrics.Saves.IoWorkers
{
    using Cysharp.Threading.Tasks;

    public interface ISaveIoReader
    {
        UniTask<byte[]> Read();
    }
}