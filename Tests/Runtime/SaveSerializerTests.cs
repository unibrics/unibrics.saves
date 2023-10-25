namespace Unibrics.Saves.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Format;
    using Model;
    using Newtonsoft.Json.Linq;
    using NSubstitute;
    using NUnit.Framework;
    using Pipeline;
    using Pipeline.Gzip;
    using Pipeline.JsonNet;

    [TestFixture]
    public class SaveSerializerTests
    {
        private const int TestValue = 5;

        private readonly ISavePipeline defaultPipeline = new SavePipeline("initial",
            new ISavePipelineStage[] {new JsonNetPipelineStage(new StubJsonConverter(), new SaveJsonSerializerConfigurator()), new JObjectToBytesStage()});

        private readonly ISavePipeline extendedPipeline = new SavePipeline("extended",
            new ISavePipelineStage[]
                {new JsonNetPipelineStage(new StubJsonConverter(), new SaveJsonSerializerConfigurator()), new JObjectToBytesStage(), new GzipStage()});

        private SaveModel SampleModel => new SaveModel(new SerializationHeader(DateTime.Now, "default", 1),
            new List<ISaveComponent> {new StubSaveable() {TestValue = TestValue}});

        [Test]
        public void _01ShouldProcessOwnSaves()
        {
            var serializer =
                new MultiPipelineSaveModelSerializer(new PredefinedPipelinesProvider(defaultPipeline,
                    new List<ISavePipeline>()));

            var saved = serializer.ConvertToBytes(SampleModel);
            var restored = serializer.ConvertFromBytes(saved);

            Assert.That(!restored.HasErrors);
            Assert.That(restored.SaveModel.Components, Has.Count.EqualTo(1));
            Assert.That(restored.SaveModel.Components.OfType<StubSaveable>().First().TestValue, Is.EqualTo(TestValue));
        }

        [Test]
        public void _02WhenPipelineIsAcceptable_ShouldParse()
        {
            var serializer =
                new MultiPipelineSaveModelSerializer(new PredefinedPipelinesProvider(defaultPipeline,
                    new List<ISavePipeline>()));
            var extendedSerializer =
                new MultiPipelineSaveModelSerializer(new PredefinedPipelinesProvider(extendedPipeline,
                    new List<ISavePipeline> {defaultPipeline}));

            var saved = serializer.ConvertToBytes(SampleModel);
            var restored = extendedSerializer.ConvertFromBytes(saved);

            Assert.That(!restored.HasErrors);
            Assert.That(restored.SaveModel.Components, Has.Count.EqualTo(1));
            Assert.That(restored.SaveModel.Components.OfType<StubSaveable>().First().TestValue, Is.EqualTo(TestValue));
        }

        [Test]
        public void _03WhenPipelineIsUnacceptable_ShouldReturnError()
        {
            var serializer =
                new MultiPipelineSaveModelSerializer(new PredefinedPipelinesProvider(defaultPipeline,
                    new List<ISavePipeline>()));
            var extendedSerializer =
                new MultiPipelineSaveModelSerializer(new PredefinedPipelinesProvider(extendedPipeline,
                    new List<ISavePipeline>()));

            var saved = serializer.ConvertToBytes(SampleModel);
            var restored = extendedSerializer.ConvertFromBytes(saved);


            Assert.That(restored.HasErrors);
        }
    }

    class PredefinedPipelinesProvider : ISavePipelinesProvider
    {
        private readonly ISavePipeline defaultPipeline;

        private readonly List<ISavePipeline> acceptablePipelines;

        public PredefinedPipelinesProvider(ISavePipeline defaultPipeline, List<ISavePipeline> acceptablePipelines)
        {
            this.defaultPipeline = defaultPipeline;
            this.acceptablePipelines = acceptablePipelines;
        }

        public ISavePipeline GetDefaultPipeline()
        {
            return defaultPipeline;
        }

        public List<ISavePipeline> GetAcceptablePipelines()
        {
            return acceptablePipelines;
        }
    }

    class StubSaveable : SaveComponent
    {
        protected override string Name => "Stub";

        public int TestValue { get; set; }
    }

    class StubJsonConverter : ISaveFormatConverter
    {
        public JObject ConvertToLatest(JObject json)
        {
            return json;
        }
    }
}