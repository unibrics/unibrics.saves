namespace Unibrics.Saves.Model
{
    public struct SaveParsingResult
    {
        public static SaveParsingResult Empty => new SaveParsingResult();
        
        public static SaveParsingResult Corrupted => new SaveParsingResult(){HasErrors = true};
        
        public SaveModel SaveModel { get; set; }
        
        public int BytesTotal { get; }
        
        public bool HasErrors { get; set; }

        public bool IsEmpty => SaveModel == null;

        public SaveParsingResult(SaveModel saveModel, int bytesTotal) : this()
        {
            SaveModel = saveModel;
            BytesTotal = bytesTotal;
        }
    }
}