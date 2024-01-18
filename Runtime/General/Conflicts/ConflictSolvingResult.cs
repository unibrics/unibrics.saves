namespace Unibrics.Saves.Conflicts
{
    public class ConflictSolvingResult
    {
        public ConflictSolvingResultType Type { get; }
        
        public SaveObject Result { get; }

        private ConflictSolvingResult(SaveObject result, ConflictSolvingResultType type)
        {
            Type = type;
            Result = result;
        }

        public static ConflictSolvingResult Unsolved() =>
            new ConflictSolvingResult(null, ConflictSolvingResultType.Unsolved);
        
        public static ConflictSolvingResult Choose(SaveObject saveData) =>
            new ConflictSolvingResult(saveData, ConflictSolvingResultType.Solved);
    }
}