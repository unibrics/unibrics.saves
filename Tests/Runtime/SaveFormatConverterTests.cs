namespace Unibrics.Saves.Tests
{
    using Format;
    using Newtonsoft.Json.Linq;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SaveFormatConverterTests
    {
        [Test]
        public void WhenVersionIsLessThanMinimumVersion_ShouldThrow()
        {
            Assert.Throws<SaveFormatConversionException>(() =>
                new SaveFormatConverter(new MockVersionProvider(5, 3), new IncrementalSaveFormatConverter[0]));
        }

        [Test]
        public void WhenThereIsNoWayToCurrentVersion_ShouldThrow()
        {
            Assert.Throws<SaveFormatConversionException>(() =>
                new SaveFormatConverter(new MockVersionProvider(1, 4), new []
                {
                    new MockIncrementalConverter(1),
                    new MockIncrementalConverter(2),
                }));
        }
        
        [Test]
        public void WhenThereIsConflictingConverters_ShouldThrow()
        {
            Assert.Throws<SaveFormatConversionException>(() =>
                new SaveFormatConverter(new MockVersionProvider(1, 3), new []
                {
                    new MockIncrementalConverter(1),
                    new MockIncrementalConverter(1),
                }));
        }
        
        [Test]
        public void WhenConvertingToBiggerThanCurrentVersion_ShouldThrow()
        {
            Assert.Throws<SaveFormatConversionException>(() =>
                new SaveFormatConverter(new MockVersionProvider(1, 2), new []
                {
                    new MockIncrementalConverter(1),
                    new MockIncrementalConverter(2),
                    new MockIncrementalConverter(3),
                }));
        }

        [Test]
        public void WhenPathForUpgradingExists_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() =>
                new SaveFormatConverter(new MockVersionProvider(1, 4), new []
                {
                    new MockIncrementalConverter(1),
                    new MockIncrementalConverter(2),
                    new MockIncrementalConverter(3),
                }));
        }
    }

    class MockVersionProvider : ISaveFormatVersionProvider
    {
        public int MinimumSupportedSaveFormatVersion { get; }
        public int SaveFormatVersion { get; }

        public MockVersionProvider(int minimumSupportedSaveFormatVersion, int saveFormatVersion)
        {
            MinimumSupportedSaveFormatVersion = minimumSupportedSaveFormatVersion;
            SaveFormatVersion = saveFormatVersion;
        }
    }

    class MockIncrementalConverter : IncrementalSaveFormatConverter
    {
        public MockIncrementalConverter(int @from)
        {
            OriginalVersion = @from;
        }

        public override int OriginalVersion { get; }

        public override void Process(JObject jObject)
        {
            
        }
    }
}