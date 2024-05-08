namespace Unibrics.Saves.Utils
{
    using System.IO;
    using Core;
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

        private readonly IApplication application;

        public DebugSaveWriter(IInstanceProvider instanceProvider, IApplication application)
        {
            jsonNetPipelineStage = instanceProvider.GetInstance<JsonNetPipelineStage>();
            this.application = application;
        }

        public void WriteSave(SaveModel save, string filename)
        {
#if DEBUG
            File.WriteAllText($"{application.PersistentDataPath}/{filename}",
                jsonNetPipelineStage.ProcessOut(save).ToString(Formatting.Indented));
#endif
        }
    }
}