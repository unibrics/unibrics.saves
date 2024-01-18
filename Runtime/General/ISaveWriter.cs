namespace Unibrics.Saves
{
    using API;
    using Core.DI;
    using IoWorkers;
    using Model;
    using Pipeline;
    using Utils;

    interface ISaveWriter
    {
        void Write(SaveModel saveData, SaveImportance importance);
    }
    

    internal class SaveWriter : ISaveWriter
    {
        [Inject]
        public ISaveModelSerializer Serializer { get; set; }

        [Inject]
        public ISaveIoWriter Writer { get; set; }
        
        [Inject]
        public IDebugSaveWriter DebugSaveWriter { get; set; }
        
        public async void Write(SaveModel saveData, SaveImportance importance)
        {
            DebugSaveWriter.WriteSave(saveData, $"save.{saveData.Group}.unpacked.json");
            
            await Writer.Write(saveData.Group, Serializer.ConvertToBytes(saveData), importance);
        }
    }
}