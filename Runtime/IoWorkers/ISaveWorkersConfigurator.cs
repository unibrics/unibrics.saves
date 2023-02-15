namespace Unibrics.Saves.IoWorkers
{
    public interface ISaveWorkersConfigurator
    {
        void RegisterRemoteWriter(ISaveIoWorker worker);
        void RegisterLocalWriter(ILocalSaveIoWorker worker);
    }
}