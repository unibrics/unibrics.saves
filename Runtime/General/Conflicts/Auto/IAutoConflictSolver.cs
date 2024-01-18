namespace Unibrics.Saves.Conflicts.Auto
{
    using Core.DI;
    using Logs;

    public interface IAutoConflictSolver
    {
        ConflictSolvingResult TrySolve(SaveObject local, SaveObject remote);
    }
    
    class AutoConflictSolver : IAutoConflictSolver
    {
        [Inject]
        public ISaveSummaryExtractor SummaryExtractor { get; set; }
        
        public ConflictSolvingResult TrySolve(SaveObject local, SaveObject remote)
        {
            var localSummary = SummaryExtractor.ExtractFrom(local);
            var remoteSummary = SummaryExtractor.ExtractFrom(remote);

            var result = localSummary.CompareTo(remoteSummary);
            switch (result)
            {
                case SaveCompareResult.Equal:
                case SaveCompareResult.Greater:
                    Logger.Log("Saves", "Autosolved with local");
                    return ConflictSolvingResult.Choose(local);
                case SaveCompareResult.Less:
                    Logger.Log("Saves", "Autosolved with remote");
                    return ConflictSolvingResult.Choose(remote);
                default:
                    Logger.Log("Saves", "Autosolving failed");
                    return ConflictSolvingResult.Unsolved();
            }
        }
    }
}