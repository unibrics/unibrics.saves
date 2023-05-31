namespace Unibrics.Saves.API
{
    using System;

    internal interface ISaveBlocker
    {
        event Action SaveUnblocked;

        bool IsBlocked { get; }
    }
}