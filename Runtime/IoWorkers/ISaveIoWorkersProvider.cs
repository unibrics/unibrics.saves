namespace Unibrics.Saves.IoWorkers
{
    using System.Collections.Generic;
    using API;
    using Cysharp.Threading.Tasks;
    using UnityEditor;
    using UnityEngine;

    public interface ISaveIoWorkersProvider
    {
        ILocalSaveIoWorker LocalWorker { get; }
        
        ISaveIoWorker RemoteWorker { get; }
    }
    
    public class ComboSaveWorker : ISaveIoWorker, ISaveWorkersConfigurator, ISaveIoWorkersProvider
    {
        public ILocalSaveIoWorker LocalWorker { get; private set; } = new LocalSaveWorker();

        public ISaveIoWorker RemoteWorker { get; private set; } = new StubSaveIoWorker();

        public UniTask<IEnumerable<byte[]>> Read()
        {
            return RemoteWorker.Read();
        }

        public async UniTask<bool> Write(string saveDataGroup, byte[] data, SaveImportance importance)
        {
            await LocalWorker.Write(saveDataGroup, data, importance);

            RemoteWorker.Write(saveDataGroup, data, importance);
            return true;
        }

        public void RegisterRemoteWriter(ISaveIoWorker worker)
        {
            RemoteWorker = worker;
        }

        public void RegisterLocalWriter(ILocalSaveIoWorker worker)
        {
            LocalWorker = worker;
        }
    }
}