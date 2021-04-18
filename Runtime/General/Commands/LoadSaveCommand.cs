namespace Unibrics.Saves.Commands
{
    using System;
    using Core.DI;
    using Core.Execution;
    using IoWorkers;
    using Pipeline;
    using UnityEngine;

    public class LoadSaveCommand : ExecutableCommand
    {
        private readonly ISaveIoWorker worker;
        private readonly ISaveProcessor saver;
        private readonly ISavePipeline savePipeline;

        internal LoadSaveCommand(ISaveIoWorker worker, ISaveProcessor saver, ISavePipeline savePipeline)
        {
            this.worker = worker;
            this.saver = saver;
            this.savePipeline = savePipeline;
        }

        protected override async void ExecuteInternal()
        {
            Retain();
            var bytes = await worker.Read();

            var header = SaveBinaryHeader.FromPrefixedArray(bytes);
            if (header.HasError)
            {
                return;
            }

            var dataWithoutHeader = header.Value.ExtractPayload(bytes);
            var pipeline = GetCorrectPipelineFor(header.Value.PipelineId);

            ISaveObject save = new LocalSaveObject(pipeline.ConvertFromBytes(dataWithoutHeader).SaveModel);
            if (save.IsEmpty)
            {
                save = new InitialSaveObject();
                //FirstSessionChecker.MarkCurrentSessionAsFirst();
            }
            else
            {
                //WriteLoadedSaveAsJson(save);    
            }

            try
            {
                saver.LoadFromSave(save);
            }
            catch (Exception e)
            {
                Debug.LogError($"Critical error occured during save applying");
                Debug.LogError(e);
                return;
            }

            ReleaseAndComplete();
        }


        private ISavePipeline GetCorrectPipelineFor(string id)
        {
            if (savePipeline.Id == id)
            {
                return savePipeline;
            }

            throw new Exception("Unknown pipeline");
        }
    }
}