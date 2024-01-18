namespace Unibrics.Saves.IoWorkers
{
    using API;
    using Cysharp.Threading.Tasks;

    public interface ISaveIoWriter
    {
        UniTask<bool> Write(string saveDataGroup, byte[] data, SaveImportance importance);
    }


}