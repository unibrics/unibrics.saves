namespace Unibrics.Saves.IoWorkers
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;

    public class StubSaveIoWorker : ILocalSaveIoWorker
    {
        public UniTask<IEnumerable<byte[]>> Read()
        {
            IEnumerable<byte[]> bytesList = new List<byte[]>(){Array.Empty<byte>()};
            return UniTask.FromResult(bytesList);
        }

        public UniTask<bool> Write(string saveDataGroup, byte[] data)
        {
            return UniTask.FromResult(true);
        }

        public IEnumerable<byte[]> ReadSync()
        {
            IEnumerable<byte[]> bytesList = new List<byte[]>(){Array.Empty<byte>()};
            return bytesList;
        }

        public bool WriteSync(string saveGroup, byte[] data)
        {
            return true;
        }
    }
}