namespace Unibrics.Saves.IoWorkers
{
    using System;
    using System.IO;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class LocalSaveWorker : ISaveIoWorker
    {
        private readonly string filename = Path.Combine(Application.persistentDataPath, "save.dat");

        public UniTask<byte[]> Read()
        {
            if (!File.Exists(filename))
            {
                return UniTask.FromResult<byte[]>(null);
            }

            return UniTask.FromResult(File.ReadAllBytes(filename));
        }

        public UniTask<bool> Write(byte[] data)
        {
            try
            {
                SafeWriteFile(data, filename);
                return UniTask.FromResult(true);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return UniTask.FromResult(false);
            }
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