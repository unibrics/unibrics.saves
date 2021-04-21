namespace Unibrics.Saves.Format
{
    using Newtonsoft.Json.Linq;

    public interface ISaveFormatConverter
    {
        JObject ConvertToLatest(JObject json);
    }
}