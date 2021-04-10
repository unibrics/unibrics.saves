namespace Unibrics.Saves
{
    using Model;
    using UnityEngine;

    interface ISaveWriter
    {
        void Write(SaveModel saveData);
    }

    internal class SaveWriter : ISaveWriter
    {
        public void Write(SaveModel saveData)
        {
            Debug.Log(string.Join(",", saveData.Components));
        }
    }
}