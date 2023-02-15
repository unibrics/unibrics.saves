namespace Unibrics.Saves.Conflicts
{
    using Cysharp.Threading.Tasks;

    interface ICombinedSaveConflictSolver
    {
        UniTask<SaveObject> ChooseCorrectData(SaveObject localData, SaveObject remoteData, bool tryAutoSolve = true);
    }

    class CombinedSaveConflictSolver : ICombinedSaveConflictSolver
    {
        public UniTask<SaveObject> ChooseCorrectData(SaveObject localData, SaveObject remoteData, bool tryAutoSolve = true)
        {
            throw new System.NotImplementedException();
        }
    }
}