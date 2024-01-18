namespace Unibrics.Saves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Core.DI;
    using Core.Utils;
    using Format;
    using Groups;
    using Injector;
    using Model;
    using Settings;
    using UnityEngine;

    /// <summary>
    /// This entity is responsible for extracting save from saveable components
    /// and for injecting saves into them
    /// </summary>
    internal interface ISaveProcessor
    {
        void LoadFromSave(ISaveObject saveObject);
        
        IEnumerable<SaveModel> GetSavesForCurrentState(List<string> groups);
    }

    class SaveProcessor : ISaveProcessor, INewSaveablesInitializer
    {
        [Inject]
        public ISaveInjector Injector { get; set; }
        
        [Inject]
        public ISaveFormatVersionProvider FormatVersionProvider { get; set; }

        [Inject]
        public List<ISaveComponentsProcessor> Processors { get; set; }

        [Inject]
        public IDeviceFingerprintProvider DeviceFingerprintProvider { get; set; }

        private ISaveGroupsDependenciesResolver dependenciesResolver;

        private readonly List<ISaveable> saveables;

        private readonly List<ISaveable> initialSaveables = new();

        public SaveProcessor(List<ISaveable> saveables, ISaveGroupProvider saveGroupProvider, ISaveGroupsDependenciesResolver dependenciesResolver)
        {
            this.saveables = saveables;
            this.dependenciesResolver = dependenciesResolver;
            
            foreach (var saveable in saveables)
            {
                var component = saveable.SaveComponentName;
                saveable.InitializeSaveGroup(saveGroupProvider.GetGroupFor(component));
            }
            
            dependenciesResolver.ValidateAndPrepare(saveGroupProvider.GroupsDependencies);
        }

        public void LoadFromSave(ISaveObject saveObject)
        {
            if (saveObject.IsInitial)
            {
                MarkForLaterInit(saveables);
            }
            else
            {
                TryRestore(saveObject, saveObject.Result.Header.Timestamp);
            }
        }
        
        private void MarkForLaterInit(IReadOnlyList<ISaveable> saveables)
        {
            foreach (var persistent in saveables)
            {
                initialSaveables.Add(persistent);
            }
        }
        
        private void TryRestore(ISaveObject saveObject, DateTime lastSaveTime)
        {
            foreach (var persistent in saveables)
            {
                var result = Injector.TryInjectSaves(saveObject.Result.Components, persistent, lastSaveTime);
                if (result == SaveInjectionResult.Fail)
                {
                    throw new Exception($"Could not insert into {persistent}");
                }

                if (result == SaveInjectionResult.NoSaveFound)
                {
                    initialSaveables.Add(persistent);
                }
            }
        }

        public IEnumerable<SaveModel> GetSavesForCurrentState(List<string> groups)
        {
            // if groups is not null, add all dependencies to that list
            groups = groups?.Concat(dependenciesResolver.ResolveGroupsToSave(groups)).Distinct().ToList();
            
            // we are processing the most often case of saving one group differently to exclude extra LINQ stuff
            if (groups is { Count: 1 })
            {
                var group = groups[0];
                yield return new SaveModel(GetHeaderFor(group),
                    GetSaves(saveables.Where(saveable => saveable.SaveGroup == group)));
                
                yield break;
            }
            
            IEnumerable<ISaveable> toSave = saveables;
            if (groups != null)
            {
                toSave = saveables.Where(saveable => groups.Contains(saveable.SaveGroup));
            }
            
            foreach (var saveablesGroup in toSave.GroupBy(saveable => saveable.SaveGroup))
            {
                yield return new SaveModel(GetHeaderFor(saveablesGroup.Key), GetSaves(saveablesGroup));        
            }

            List<ISaveComponent> GetSaves(IEnumerable<ISaveable> saveables)
            {
                return saveables.Select(saveable => Injector.GetSave(saveable)).ToList();
            }

            SerializationHeader GetHeaderFor(string group)
            {
                var fingerprint = DeviceFingerprintProvider.DeviceFingerprint;
                return new SerializationHeader(DateTime.UtcNow, group, FormatVersionProvider.SaveFormatVersion, fingerprint);
            }
        }

        public void InitializeComponentsWithoutSaves()
        {
            var orderedProcessors = Processors.OrderByDescending(processor => processor.Priority).ToList();
            foreach (var initialSaveable in initialSaveables)
            {
                initialSaveable.PrepareInitial(orderedProcessors);
            }
        }
    }
}