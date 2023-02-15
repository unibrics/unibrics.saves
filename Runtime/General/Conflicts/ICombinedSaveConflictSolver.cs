namespace Unibrics.Saves.Conflicts
{
    using System;
    using Auto;
    using Core.DI;
    using Cysharp.Threading.Tasks;
    using Format;
    using Manual;
    using UnityEngine;
    using Logger = Logs.Logger;

    interface ICombinedSaveConflictSolver
    {
        UniTask<SaveObject> ChooseCorrectData(SaveObject localData, SaveObject remoteData, bool tryAutoSolve = true);
    }

    class CombinedSaveConflictSolver : ICombinedSaveConflictSolver
    {
        [Inject]
        public IAutoConflictSolver AutoConflictSolver { get; set; }

        [Inject]
        public IManualConflictSolver ManualConflictSolver { get; set; }
        
        [Inject]
        public ISaveFormatVersionProvider SaveFormatVersionProvider { get; set; }
        
        public async UniTask<SaveObject> ChooseCorrectData(SaveObject localData, SaveObject remoteData, bool tryAutoSolve = true)
        {
            if (SaveCameFromNewerBuild(localData) || SaveCameFromNewerBuild(remoteData))
            {
                //await UiBuilder.CreateWindow<SaveCameFromNewerBuildWindow>(UiRootType.Any).WaitForClosed();
                return null;
            }
            
            
            //if both are empty, it's probably first session, so no difference what to return
            if (localData.IsEmpty && remoteData.IsEmpty)
            {
                Log("Both saves are empty");
                return localData;
            }

            if (localData.IsEmpty)
            {
                Log("No local save found, returning remote");
                return remoteData;
            }
            
            if (remoteData.IsEmpty)
            {
                Log("No remote save fetched, returning local");
                return localData;
            }

            if (tryAutoSolve)
            {
                var solveResult = AutoConflictSolver.TrySolve(localData, remoteData);
                if (solveResult.Type == ConflictSolvingResultType.Solved)
                {
                    return solveResult.Result;
                }    
            }

            return await ManualConflictSolver.Solve(localData, remoteData);

            void Log(string log)
            {
                Logger.Log("Saves", log);
            }
        }
        
        private bool SaveCameFromNewerBuild(SaveObject saveData)
        {
            var currentVersion = new Version(Application.version);
            if (saveData.IsEmpty)
            {
                return false;
            }

            var serializationHeader = saveData.Result.Header;
            var newerAppVersion = new Version(serializationHeader.BuildVersion) > currentVersion;
            if (newerAppVersion)
            {
                return true;
            }

            return SaveFormatVersionProvider.SaveFormatVersion < serializationHeader.FormatVersion;
        }
    }
}