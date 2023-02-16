namespace Unibrics.Saves.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Settings;

    [TestFixture]
    public class SaveGroupsDependenciesResolverTests
    {
        private ISaveGroupsDependenciesResolver resolver;

        [SetUp]
        public void SetUp()
        {
            resolver = new SaveGroupsDependenciesResolver();
        }
        
        [Test]
        public void CircularDependencies_ShouldRaise()
        {
            var deps = new Dictionary<string, List<string>>()
            {
                ["a"] = new() { "b" },
                ["b"] = new() { "c" },
                ["c"] = new() { "a" },
            };

            Assert.Throws<Exception>(() => resolver.ValidateAndPrepare(deps));
        }
        
        [Test]
        public void Dependencies_ShouldBeResolved()
        {
            var deps = new Dictionary<string, List<string>>()
            {
                ["a"] = new() { "b", "c" },
                ["b"] = new() { "d", "e" },
                ["c"] = new() { "d" },
                ["f"] = new() { "g" },
            };

            resolver.ValidateAndPrepare(deps);
            var list = resolver.ResolveGroupsToSave(new List<string> {"a"});
            CollectionAssert.AreEquivalent(new List<string>(){"b", "c", "d", "e"}, list);
            
            list = resolver.ResolveGroupsToSave(new List<string> {"a", "b", "f"});
            CollectionAssert.AreEquivalent(new List<string>(){"b", "c", "d", "e", "g"}, list);
        }
        
        [Test]
        public void NpDependencies_ShouldBeResolved()
        {
            var deps = new Dictionary<string, List<string>>()
            {
                ["b"] = new(){"c"}
            };

            resolver.ValidateAndPrepare(deps);
            var list = resolver.ResolveGroupsToSave(new() {"a"});
            CollectionAssert.AreEqual(new List<string>(), list);
        }
    }
}