namespace Unibrics.Saves
{
    using Core.DI;
    using IoWorkers;
    using Model;
    using Pipeline;

    interface ISaveWriter
    {
        void Write(SaveModel saveData);
    }

    internal class SaveWriter : ISaveWriter
    {
        [Inject]
        public ISaveModelSerializer Serializer { get; set; }

        [Inject]
        public ISaveIoWorker IoWorker { get; set; }
        
        public async void Write(SaveModel saveData)
        {
            await IoWorker.Write(Serializer.ConvertToBytes(saveData));
        }
    }
}