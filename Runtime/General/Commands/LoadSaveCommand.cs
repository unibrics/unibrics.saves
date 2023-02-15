namespace Unibrics.Saves.Commands
{
    using System;
    using API;
    using Conflicts;
    using Core.DI;
    using Core.Execution;
    using Cysharp.Threading.Tasks;
    using IoWorkers;
    using Model;
    using Pipeline;
    using UnityEngine;
    using Utils;

    public class LoadSaveCommand : ExecutableCommand
    {
        [Inject]
        public ISaveIoWorkersProvider WorkersProvider { get; set; }

        [Inject]
        ISaveModelSerializer Serializer { get; set; }
        
        [Inject]
        ICombinedSaveConflictSolver ConflictSolver { get; set; }
        
        [Inject]
        IDebugSaveWriter DebugSaveWriter { get; set; }

        [Inject]
        ISaveProcessor SaveProcessor { get; set; }

        [Inject]
        public IFirstSessionChecker FirstSessionChecker { get; set; }
        
        protected override async void ExecuteInternal()
        {
            Retain();
            
            SaveObject localSave = new LocalSaveObject(await LoadFrom(WorkersProvider.LocalWorker));
            SaveObject remoteSave = new RemoteSaveObject(await LoadFrom(WorkersProvider.RemoteWorker));

            var chosenSave = await ConflictSolver.ChooseCorrectData(localSave, remoteSave);
            if (chosenSave.IsEmpty)
            {
                chosenSave = new InitialSaveObject();
                FirstSessionChecker.MarkCurrentSessionAsFirst();
            }
            else
            {
                DebugSaveWriter.WriteSave(chosenSave.Result, "savefile.loaded.json");
            }

            try
            {
                SaveProcessor.LoadFromSave(chosenSave);
            }
            catch (Exception e)
            {
                Debug.LogError($"Critical error occured during save applying");
                Debug.LogError(e);
                return;
            }

            ReleaseAndComplete();
        }

        private async UniTask<SaveParsingResult> LoadFrom(ISaveIoWorker worker)
        {
            var bytes = await worker.Read();
            return Serializer.ConvertFromBytes(bytes);
        }
    }
}