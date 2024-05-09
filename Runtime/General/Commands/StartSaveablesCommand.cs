namespace Unibrics.Saves.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Core.DI;
    using Core.Execution;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class StartSaveablesCommand : ExecutableCommand
    {
        [Inject]
        List<ISaveable> Saveables { get; set; }
        
        protected override async void ExecuteInternal()
        {
            Retain();
            await Enumerable.Select(Saveables, Start).ToList();
            ReleaseAndComplete();
        }

        private async UniTask Start(ISaveable saveable)
        {
            try
            {
                await saveable.Start();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during starting saveable {saveable}");
                Debug.LogException(e);
                throw;
            }
        }
    }
}