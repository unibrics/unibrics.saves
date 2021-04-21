namespace Unibrics.Saves.Format
{
    using Newtonsoft.Json.Linq;

    public abstract class IncrementalSaveFormatConverter
    {
        public abstract bool CanConvertableFrom(int version);
        
        public abstract int GoalVersion { get; }

        public abstract void Process(JObject jObject);
    }
}