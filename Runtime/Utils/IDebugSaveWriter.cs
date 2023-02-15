namespace Unibrics.Saves.Utils
{
    using System.IO;
    using Core.DI;
    using Model;
    using Newtonsoft.Json;
    using Pipeline.JsonNet;
    using UnityEngine;

    interface IDebugSaveWriter
    {
        void WriteSave(SaveModel save, string filename);
    }
    
    class DebugSaveWriter : IDebugSaveWriter
    {
        private readonly JsonNetPipelineStage jsonNetPipelineStage;

        public DebugSaveWriter(IInstanceProvider instanceProvider)
        {
            jsonNetPipelineStage = instanceProvider.GetInstance<JsonNetPipelineStage>();
        }

        public void WriteSave(SaveModel save, string filename)
        {
#if DEBUG
            File.WriteAllText($"{Application.persistentDataPath}/{filename}",
                jsonNetPipelineStage.ProcessOut(save).ToString(Formatting.Indented));
#endif
        }
    }
}