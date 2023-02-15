namespace Unibrics.Saves
{
    using Model;

    interface ISaveObject
    {
        SaveModel Result { get; }
        
        bool IsInitial { get; }
        
        bool IsEmpty { get; }
    }
}