namespace Unibrics.Saves.Conflicts.Manual
{
    using Cysharp.Threading.Tasks;

    interface IManualConflictSolver
    {
        UniTask<SaveObject> Solve(SaveObject local, SaveObject remote);
    }
    
    class LocalChoosingSolver : IManualConflictSolver 
    {
        public UniTask<SaveObject> Solve(SaveObject local, SaveObject remote)
        {
            return UniTask.FromResult(local);
        }
    }
}