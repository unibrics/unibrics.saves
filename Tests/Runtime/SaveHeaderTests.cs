namespace Unibrics.Saves.Tests
{
    using NUnit.Framework;
    using Pipeline;

    [TestFixture]
    public class SaveHeaderTests
    {
        [TestCase("sample.id")]
        public void ShouldInsertAndExtractHeader(string id)
        {
            var array = new byte[48];
            var header = SaveBinaryHeader.FromPipelineId(id);
            var arrayWithHeader = header.AddItselfTo(array);

            var newHeader = SaveBinaryHeader.FromPrefixedArray(arrayWithHeader);
            
            Assert.That(newHeader.IsOk);
            Assert.That(newHeader.Value.PipelineId, Is.EqualTo(header.PipelineId));
        }
    }
}