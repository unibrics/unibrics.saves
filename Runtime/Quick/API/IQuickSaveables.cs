namespace Unibrics.Saves.API
{
    public interface IQuickSaveables
    {
        internal IQuickSaveable<T> Get<T>(string key);
    }
    
    public interface IQuickSaveable<T>
    {
        string Key { get; }
        
        T Value { get; set; }
    }

    public static class QuickSaveablesExtensions
    {
        public static IQuickSaveable<int> GetInt(this IQuickSaveables q, string key) => q.Get<int>(key);
        
        public static IQuickSaveable<bool> GetBool(this IQuickSaveables q, string key) => q.Get<bool>(key);
        
        public static IQuickSaveable<float> GetFloat(this IQuickSaveables q, string key) => q.Get<float>(key);
        
        public static IQuickSaveable<string> GetString(this IQuickSaveables q, string key) => q.Get<string>(key);
    } 
        
}