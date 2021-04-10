namespace Unibrics.Saves.Injector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Core;
    using Tools;

    internal class SaveInjector : ISaveInjector
    {
        private readonly List<SaveComponentInjector> injectors = new List<SaveComponentInjector>();

        public SaveInjector()
        {
            injectors.AddRange(Types.WithParent<ISaveComponent>()
                .Where(type => !type.IsAbstract)
                .Select(type => typeof(SaveComponentInjector<>).MakeGenericType(type))
                .CreateInstances<SaveComponentInjector>());
        }

        public SaveInjectionResult TryInjectSaves(List<ISaveComponent> saves, ISaveable persistent,
            DateTime lastSaveTime)
        {
            var injector = injectors.FirstOrDefault(inj => inj.CanWorkWith(persistent));
            if (injector == null)
            {
                throw new Exception(
                    $"No save injector found for type {persistent} - probably," +
                    $" you forget to add [{nameof(UnibricsDiscoverableAttribute)}] to assembly");
            }

            var save = injector.GetSaveToInject(saves);
            if (save == null)
            {
                return SaveInjectionResult.NoSaveFound;
            }

            return injector.TryInject(save, persistent, lastSaveTime)
                ? SaveInjectionResult.Success
                : SaveInjectionResult.Fail;
        }

        public ISaveComponent GetSave(ISaveable persistent)
        {
            var injector = injectors.FirstOrDefault(inj => inj.CanWorkWith(persistent));
            if (injector == null)
            {
                throw new Exception(
                    $"No save injector found for type {persistent} - probably, you forget to register type");
            }

            var componentSave = injector.ExtractSave(persistent);
            if (componentSave == null)
            {
                throw new Exception($"Null came from {persistent}");
            }

            return componentSave;
        }
    }
}