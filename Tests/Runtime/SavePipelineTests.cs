namespace Unibrics.Saves.Tests
{
    using System;
    using Exceptions;
    using Model;
    using NSubstitute;
    using NUnit.Framework;
    using Pipeline;

    [TestFixture]
    public class SavePipelineTests
    {
        [Test]
        public void _01WhenEmptyPipeline_ShouldThrow()
        {
            Assert.Throws<BrokenSavePipelineException>(() => new SavePipeline(new ISavePipelineStage[0]));
        }

        [Test]
        public void _02FirstStageMustTakeSaveModel()
        {
            var incorrectStage = Substitute.For<ISavePipelineInboundStage<string>>();

            Assert.Throws<BrokenSavePipelineException>(() => new SavePipeline(new[] {incorrectStage}));
        }

        [Test]
        public void _03LastStageMustProduceBytes()
        {
            var firstStage = Substitute.For<ISavePipelineInboundStage<SaveModel>>();
            var incorrectStage = Substitute.For<ISavePipelineInboundStage<string>>();

            Assert.Throws<BrokenSavePipelineException>(() =>
                new SavePipeline(new ISavePipelineStage[] {firstStage, incorrectStage}));
        }
        
        [Test]
        public void _04LastStageMustBeCompatible()
        {
            var firstStage = new StubPipelineStage<SaveModel, string>();
            var secondStage = new StubPipelineStage<string, byte[]>();
            
            var incorrectStage = new StubPipelineStage<int, byte[]>();

            Assert.Throws<BrokenSavePipelineException>(() => new SavePipeline(new ISavePipelineStage[] {firstStage, incorrectStage}));
            Assert.DoesNotThrow(() => new SavePipeline(new ISavePipelineStage[] {firstStage, secondStage}));
        }
        
        class StubPipelineStage<TIn, TOut> : SavePipelineStage<TIn, TOut>
        {
            public override TOut ProcessOut(TIn model)
            {
                return default;
            }

            public override TIn ProcessIn(TOut data)
            {
                return default;
            }
        }
    }
}