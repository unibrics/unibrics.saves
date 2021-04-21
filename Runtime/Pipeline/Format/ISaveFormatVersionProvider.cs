namespace Unibrics.Saves.Format
{
    public interface ISaveFormatVersionProvider
    {
        int MinimumSupportedSaveFormatVersion { get; }
        
        int SaveFormatVersion { get; }
    }
}