namespace Unibrics.Saves.IoWorkers
{
    using Cysharp.Threading.Tasks;
    using UnityEditor;

    public interface ISaveIoWorkersProvider
    {
        ILocalSaveIoWorker LocalWorker { get; }
        
        ISaveIoWorker RemoteWorker { get; }
    }
    
    public class ComboSaveWorker : ISaveIoWorker, ISaveWorkersConfigurator, ISaveIoWorkersProvider
    {
        public ILocalSaveIoWorker LocalWorker { get; private set; } = new LocalSaveWorker();

        public ISaveIoWorker RemoteWorker { get; private set; } = new StubSaveIoWorker();

        public UniTask<byte[]> Read()
        {
            return RemoteWorker.Read();
        }

        public async UniTask<bool> Write(byte[] data)
        {
            await LocalWorker.Write(data);

            RemoteWorker.Write(data);
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