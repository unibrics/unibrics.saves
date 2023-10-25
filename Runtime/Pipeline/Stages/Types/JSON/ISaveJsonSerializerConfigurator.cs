namespace Unibrics.Saves.Pipeline.JsonNet
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface ISaveJsonSerializerConfigurator
    {
        void AddConverter(JsonConverter converter);
        
        internal List<JsonConverter> ExtraConverters { get; }
    }

    class SaveJsonSerializerConfigurator : ISaveJsonSerializerConfigurator
    {
        private readonly List<JsonConverter> extraConverters = new();

        public void AddConverter(JsonConverter converter)
        {
            extraConverters.Add(converter);
        }

        List<JsonConverter> ISaveJsonSerializerConfigurator.ExtraConverters => extraConverters;
    }
}