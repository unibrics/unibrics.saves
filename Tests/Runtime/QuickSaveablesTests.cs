namespace Unibrics.Saves.Tests
{
    using System;
    using API;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using Quick;

    [TestFixture]
    public class QuickSaveablesTests
    {
        private void CheckQuickSaveable<T>(T value)
        {
            var qs = new QuickSaveables()
            {
                SaveScheduler = Substitute.For<ISaveScheduler>()
            };
            var quickSaveables = ((IQuickSaveables)qs);
            
            var key = "test";
            var saveable = quickSaveables.Get<T>(key);
            saveable.Value = value;
            Assert.That(quickSaveables.Get<T>(key).Value, Is.EqualTo(value));

            var serialized = qs.Serialize();
            qs.Deserialize(JsonConvert.DeserializeObject<QuickSaveablesSaveData>(JsonConvert.SerializeObject(serialized)), DateTime.Now);
            Assert.That(quickSaveables.Get<T>(key).Value, Is.EqualTo(value));
        }
        
        [Test]
        public void _01_ShouldSave_Ints()
        {
            CheckQuickSaveable(17);
        }
        
        [Test]
        public void _02_ShouldSave_Bool()
        {
            CheckQuickSaveable(true);
            CheckQuickSaveable(false);
        }
        
        [Test]
        public void _03_ShouldSave_Float()
        {
            CheckQuickSaveable(17.1f);
        }
        
        [Test]
        public void _04_ShouldSave_String()
        {
            CheckQuickSaveable("testtest");
        }
    }
}