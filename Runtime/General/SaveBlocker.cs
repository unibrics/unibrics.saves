namespace Unibrics.Saves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API;
    using Blocker;
    using Logs;

    public class SaveContext : ISaveContext, ISaveBlocker
    {
        public event Action SaveUnblocked;

        public bool IsBlocked => saveBlockers.Any();

        private readonly List<ISaveBlockingContext> saveBlockers = new() { SaveBlockingContext.Initial };

        public void Block(ISaveBlockingContext context)
        {
            saveBlockers.Add(context);
        }

        public void Unblock(ISaveBlockingContext context)
        {
            if (!saveBlockers.Contains(context))
            {
                Logger.Log("Saves", $"Trying to remove {context.Id} blocking context when it wasn't blocking!");
                return;
            }

            saveBlockers.Remove(context);
            if (!IsBlocked)
            {
                SaveUnblocked?.Invoke();
            }
        }
    }
}