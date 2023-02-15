namespace Unibrics.Saves.Conflicts
{
    interface ISaveSummaryExtractor
    {
        ISaveSummary ExtractFrom(SaveObject saveData);
    }
    
    
    class SimpleSaveSummaryExtractor : ISaveSummaryExtractor
    {
        public ISaveSummary ExtractFrom(SaveObject saveData)
        {
            var serializationHeader = saveData.Result.Header;
            return new SimpleSaveSummary()
            {
                Version = serializationHeader.FormatVersion,
                CreationTime = serializationHeader.Timestamp
            };
        }
    }
    
    public class SimpleSaveSummary : SaveSummary<SimpleSaveSummary>
    {
        
    }
}