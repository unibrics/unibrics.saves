namespace Unibrics.Saves.Pipeline.JsonNet
{
    using System.Runtime.Serialization;
    using Format;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;

    [SavePipelineStage("serialize.jobject")]
    internal class JsonNetPipelineStage : SavePipelineStage<SaveModel, JObject>
    {
        private JsonSerializer serializer;
        private JsonSerializer Serializer => serializer ??= CreateSerializer();

        private readonly ISaveFormatConverter saveFormatConverter;

        private readonly ISaveJsonSerializerConfigurator configurator;

        public JsonNetPipelineStage(ISaveFormatConverter saveFormatConverter,
            ISaveJsonSerializerConfigurator configurator)
        {
            this.saveFormatConverter = saveFormatConverter;
            this.configurator = configurator;
        }

        private JsonSerializer CreateSerializer()
        {
            var serializer = JsonSerializer.Create
            (new JsonSerializerSettings
            {
                Context = new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence),
                NullValueHandling = NullValueHandling.Ignore
            });
            
            serializer.Converters.Add(new SaveModelConverter());
            serializer.Converters.Add(new StringEnumConverter());
            foreach (var extraConverter in configurator.ExtraConverters)
            {
                serializer.Converters.Add(extraConverter);
            }
            
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;

            return serializer;
        }
        
        public override JObject ProcessOut(SaveModel model)
        {
            return JObject.FromObject(model, Serializer);
        }

        public override SaveModel ProcessIn(JObject json)
        {
            return saveFormatConverter.ConvertToLatest(json).ToObject<SaveModel>(Serializer);
        }
    }
}