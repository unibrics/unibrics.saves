namespace Unibrics.Saves.Commands
{
    using System;
    using Core.Execution;
    using IoWorkers;
    using Pipeline;
    using UnityEngine;

    public class LoadSaveCommand : ExecutableCommand
    {
        private readonly ISaveIoWorker worker;
        private readonly ISaveProcessor saver;
        private readonly ISaveModelSerializer serializer;

        internal LoadSaveCommand(ISaveIoWorker worker, ISaveProcessor saver, ISaveModelSerializer serializer)
        {
            this.worker = worker;
            this.saver = saver;
            this.serializer = serializer;
        }

        protected override async void ExecuteInternal()
        {
            Retain();
            var bytes = await worker.Read();

            var header = SaveBinaryHeader.FromPrefixedArray(bytes);
            if (header.HasError)
            {
                ReleaseAndComplete();
                return;
            }

            ISaveObject save = new LocalSaveObject(serializer.ConvertFromBytes(bytes).SaveModel);
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


    }
}