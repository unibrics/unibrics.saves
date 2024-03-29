namespace Unibrics.Saves.IoWorkers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using API;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class LocalSaveWorker : ILocalSaveIoWorker
    {
        private const string Prefix = "save.";
        
        private string FilenameFor(string group) => Path.Combine(Application.persistentDataPath, $"{Prefix}{group}.dat");

        public UniTask<IEnumerable<byte[]>> Read()
        {
            return UniTask.FromResult(ReadSync());
        }

        public IEnumerable<byte[]> ReadSync()
        {
            foreach (var fileName in Directory.GetFiles(Application.persistentDataPath, $"{Prefix}*.dat"))
            {
                Debug.Log(fileName);
                yield return File.ReadAllBytes(fileName);
            }
        }

        public bool WriteSync(string saveGroup, byte[] data)
        {
            try
            {
                SafeWriteFile(data, FilenameFor(saveGroup));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public UniTask<bool> Write(string saveGroup, byte[] data, SaveImportance importance)
        {
            return UniTask.FromResult(WriteSync(saveGroup, data));
        }

        /// <summary>
        /// writes file into temp destination, then swaps temp file with goal file,
        /// rolls back on error
        /// </summary>
        private static void SafeWriteFile(byte[] bytes, string filename)
        {
            var sourceFileName = $"{filename}.tmp";
            var backupFileName = $"{filename}.bak";
            File.WriteAllBytes(sourceFileName, bytes);
            SafeCommitFile(sourceFileName, filename, backupFileName);
        }

        private static void SafeCommitFile(string sourceFileName, string destinationFileName,
            string backupFileName = null)
        {
            if (backupFileName == null)
            {
                backupFileName = destinationFileName + ".bak";
            }

            File.Delete(backupFileName);
            if (File.Exists(destinationFileName))
            {
                File.Move(destinationFileName, backupFileName);
            }

            try
            {
                File.Move(sourceFileName, destinationFileName);
            }
            catch (Exception)
            {
                if (File.Exists(backupFileName))
                {
                    File.Move(backupFileName, destinationFileName); // Try to move old file back.
                }

                throw;
            }
        }
    }
}