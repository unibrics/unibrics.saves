namespace Unibrics.Saves.Pipeline.JsonNet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Types = Tools.Types;

    public class SaveModelConverter : JsonConverter
    {
        private const string HeaderKey = "Header";

        private static readonly IDictionary<string, Type> SaveTypes = new Dictionary<string, Type>();

        static SaveModelConverter()
        {
            var types = Types.WithParent<SaveComponent>();

            foreach (var type in types)
            {
                SaveTypes[((SaveComponent)Activator.CreateInstance(type)).GetName()] = type;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var save = (SaveModel) value;

            writer.WriteStartObject();

            writer.WritePropertyName(HeaderKey);
            serializer.Serialize(writer, save.Header);

            foreach (var saveComponent in save.Components)
            {
                writer.WritePropertyName(saveComponent.GetName());
                serializer.Serialize(writer, saveComponent);
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var components = new Dictionary<Type, ISaveComponent>();
            SerializationHeader header = null;

            foreach (var entry in jObject)
            {
                if (entry.Value == null)
                {
                    throw new Exception($"Null entries are not supported is settings file, but entry {entry.Key} found");
                }
                var key = entry.Key;
                if (key == HeaderKey)
                {
                    if (header != null)
                    {
                        throw new Exception("Duplicate header in save file");
                    }

                    header = entry.Value.ToObject<SerializationHeader>(serializer);
                    continue;
                }

                if (!SaveTypes.ContainsKey(key))
                {
                    continue;
                }

                var saveType = SaveTypes[key];
                var component = entry.Value.ToObject(saveType, serializer);
                var saveComponent = component as ISaveComponent;
                if (saveComponent == null)
                {
                    throw new Exception($"Type {saveType} is not {nameof(ISaveComponent)}!");
                }

                components[saveType] = saveComponent;
            }

            if (header == null)
            {
                header = new SerializationHeader(DateTime.UtcNow, "default", 1, null);
            }

            return new SaveModel(header, components.Values.ToList());
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SaveModel);
        }
    }
}