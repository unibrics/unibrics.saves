namespace Unibrics.Saves.API
{
    using System;

    public interface ISaveBlocker
    {
        event Action SaveUnblocked;

        bool IsBlocked { get; }
    }
}