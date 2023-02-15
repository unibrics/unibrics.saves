namespace Unibrics.Saves.IoWorkers
{
    using System;
    using Cysharp.Threading.Tasks;

    public class StubSaveIoWorker : ILocalSaveIoWorker
    {
        public UniTask<byte[]> Read()
        {
            return UniTask.FromResult(Array.Empty<byte>());
        }

        public UniTask<bool> Write(string saveDataGroup, byte[] data)
        {
            return UniTask.FromResult(true);
        }

        public byte[] ReadSync()
        {
            return Array.Empty<byte>();
        }

        public bool WriteSync(string saveGroup, byte[] data)
        {
            return true;
        }
    }
}