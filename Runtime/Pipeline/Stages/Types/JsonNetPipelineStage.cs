namespace Unibrics.Saves.Pipeline
{
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;

    [SavePipelineStage("serialize.json.text")]
    class JsonNetPipelineStage : SavePipelineStage<SaveModel, JObject>
    {
        private JsonSerializer serializer;

        private JsonSerializer Serializer => serializer ??= CreateSerializer();
        
        private JsonSerializer CreateSerializer()
        {
            var serializer = JsonSerializer.Create
            (new JsonSerializerSettings
            {
                Context = new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence),
                NullValueHandling = NullValueHandling.Ignore
            });
            
            // serializer.Converters.Add(new SaveModelConverter());
            serializer.Converters.Add(new StringEnumConverter());
            // serializer.Converters.AddRange(extraConverters);
            
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
            return json.ToObject<SaveModel>(Serializer);
        }
    }
}