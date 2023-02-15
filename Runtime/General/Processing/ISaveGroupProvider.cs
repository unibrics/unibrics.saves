namespace Unibrics.Saves.Groups
{
    interface ISaveGroupProvider
    {
        string GetGroupFor(string component);
    }
}