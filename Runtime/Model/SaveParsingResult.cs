namespace Unibrics.Saves.Model
{
    internal struct SaveParsingResult
    {
        public static SaveParsingResult Empty => new SaveParsingResult();
        
        public static SaveParsingResult Corrupted => new SaveParsingResult(){HasErrors = true};
        
        public SaveModel SaveModel { get; set; }
        
        public bool HasErrors { get; set; }

        public bool IsEmpty => SaveModel == null;

        public SaveParsingResult(SaveModel saveModel) : this()
        {
            SaveModel = saveModel;
        }
    }
}