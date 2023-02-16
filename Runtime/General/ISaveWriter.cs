namespace Unibrics.Saves
{
    using Core.DI;
    using IoWorkers;
    using Model;
    using Pipeline;
    using Utils;

    interface ISaveWriter
    {
        void Write(SaveModel saveData);
    }

    internal class SaveWriter : ISaveWriter
    {
        [Inject]
        public ISaveModelSerializer Serializer { get; set; }

        [Inject]
        public ISaveIoWriter Writer { get; set; }
        
        [Inject]
        public IDebugSaveWriter DebugSaveWriter { get; set; }
        
        public async void Write(SaveModel saveData)
        {
            DebugSaveWriter.WriteSave(saveData, $"save.{saveData.Group}.unpacked.json");
            
            await Writer.Write(saveData.Group, Serializer.ConvertToBytes(saveData));
        }
    }
}