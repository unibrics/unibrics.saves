namespace Unibrics.Saves.Format
{
    using Newtonsoft.Json.Linq;

    public abstract class IncrementalSaveFormatConverter
    {
        public abstract int OriginalVersion { get; }

        public int GoalVersion => OriginalVersion + 1;

        public abstract void Process(JObject jObject);
    }
}