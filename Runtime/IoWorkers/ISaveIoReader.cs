namespace Unibrics.Saves.IoWorkers
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;

    public interface ISaveIoReader
    {
        UniTask<IEnumerable<byte[]>> Read();
    }
}