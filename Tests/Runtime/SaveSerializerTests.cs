namespace Unibrics.Saves.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Model;
    using NUnit.Framework;
    using Pipeline;
    using Pipeline.Gzip;
    using Pipeline.JsonNet;

    [TestFixture]
    public class SaveSerializerTests
    {
        private const int TestValue = 5;

        private readonly ISavePipeline defaultPipeline = new SavePipeline("initial",
            new ISavePipelineStage[] {new JsonNetPipelineStage(), new JObjectToBytesStage()});

        private readonly ISavePipeline extendedPipeline = new SavePipeline("extended",
            new ISavePipelineStage[] {new JsonNetPipelineStage(), new JObjectToBytesStage(), new GzipStage()});

        private SaveModel SampleModel => new SaveModel(new SerializationHeader(DateTime.Now, 1),
            new List<ISaveComponent> {new StubSaveable() {TestValue = TestValue}});

        [Test]
        public void _01ShouldProcessOwnSaves()
        {
            var serializer = new MultiPipelineSaveModelSerializer(defaultPipeline, new List<ISavePipeline>());

            var saved = serializer.ConvertToBytes(SampleModel);
            var restored = serializer.ConvertFromBytes(saved);

            Assert.That(!restored.HasErrors);
            Assert.That(restored.SaveModel.Components, Has.Count.EqualTo(1));
            Assert.That(restored.SaveModel.Components.OfType<StubSaveable>().First().TestValue, Is.EqualTo(TestValue));
        }

        [Test]
        public void _02WhenPipelineIsAcceptable_ShouldParse()
        {
            var serializer = new MultiPipelineSaveModelSerializer(defaultPipeline, new List<ISavePipeline>());
            var extendedSerializer =
                new MultiPipelineSaveModelSerializer(extendedPipeline, new List<ISavePipeline> {defaultPipeline});

            var saved = serializer.ConvertToBytes(SampleModel);
            var restored = extendedSerializer.ConvertFromBytes(saved);

            Assert.That(!restored.HasErrors);
            Assert.That(restored.SaveModel.Components, Has.Count.EqualTo(1));
            Assert.That(restored.SaveModel.Components.OfType<StubSaveable>().First().TestValue, Is.EqualTo(TestValue));
        }

        [Test]
        public void _03WhenPipelineIsUnacceptable_ShouldReturnError()
        {
            var serializer = new MultiPipelineSaveModelSerializer(defaultPipeline, new List<ISavePipeline>());
            var extendedSerializer = new MultiPipelineSaveModelSerializer(extendedPipeline, new List<ISavePipeline>());

            var saved = serializer.ConvertToBytes(SampleModel);
            var restored = extendedSerializer.ConvertFromBytes(saved);


            Assert.That(restored.HasErrors);
        }
    }

    class StubSaveable : SaveComponent
    {
        protected override string Name => "Stub";

        public int TestValue { get; set; }
    }
}