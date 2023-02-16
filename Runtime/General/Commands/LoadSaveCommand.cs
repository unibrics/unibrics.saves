namespace Unibrics.Saves.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                DebugSaveWriter.WriteSave(chosenSave.Result, "save.loaded.json");
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
            var parts = await worker.Read();
            return Combine(parts.Select(part => Serializer.ConvertFromBytes(part)).ToList());

            SaveParsingResult Combine(List<SaveParsingResult> results)
            {
                if (results.Any(result => result.HasErrors))
                {
                    return SaveParsingResult.Corrupted;
                }

                var fullComponents = results.Where(result => !result.IsEmpty).ToList();
                if (!fullComponents.Any())
                {
                    return SaveParsingResult.Empty;
                }
                
                var headers = results.Select(result => result.SaveModel.Header).ToList();
                
                // timestamp and version of combined header is defined by maximum version and timestamp.
                // Actually, all versions must be equal at that point because of conversion to latest version
                var combinedHeader = new SerializationHeader(headers.Max(header => header.Timestamp),
                    headers.Select(header => new Version(header.BuildVersion)).Max().ToString(),
                    "combined", headers.Max(header => header.FormatVersion));
                var combined = new SaveModel(combinedHeader,
                    fullComponents.SelectMany(component => component.SaveModel.Components).ToList());

                return new SaveParsingResult(combined, fullComponents.Sum(result => result.BytesTotal));
            }
        }
    }
}