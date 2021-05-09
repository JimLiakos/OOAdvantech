using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OOAdvantech.WindowsUniversal
{
    /// <MetaDataID>{9580efe9-df9d-4785-b713-ddc4f036b606}</MetaDataID>
    public class DeviceFileSystem : IFileSystem
    {


        void OOAdvantech.IFileSystem.DeleteFile(string filePath)
        {
            
            DeleteFile(filePath);
        }
        public static void DeleteFile(string filePath)
        {
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            foreach (var folder in OOAdvantech.FileSystem.GetFolders(filePath))
            {
                var folderTask = storageFolder.GetFoldersAsync().AsTask();
                folderTask.Wait();
                bool folderExist = false;
                IReadOnlyList<StorageFolder> localFolders = folderTask.Result;
                foreach (var localFolder in localFolders)
                {
                    if (localFolder.Name.ToLower() == folder.ToLower())
                        folderExist = true;
                }
                if (!folderExist)
                    return;
            }

            string fileName = OOAdvantech.FileSystem.GetFileName(filePath);
            var fileTask = storageFolder.GetFilesAsync().AsTask();
            fileTask.Wait();
            IReadOnlyList<StorageFile> localfiles = fileTask.Result;

            foreach (var storageFile in localfiles)
            {
                if (storageFile.Name.ToLower() == fileName.ToLower())
                {
                    var res = storageFile.DeleteAsync();
                    res.AsTask().Wait();
                    return;
                }
            }
        }

        void OOAdvantech.IFileSystem.DeleteFolder(string folderPath)
        {
            DeleteFolder(folderPath);
        }

        public static void DeleteFolder(string folderPath)
        {
            throw new NotImplementedException();
        }
        bool OOAdvantech.IFileSystem.FileExists(string filePath)
        {
            return FileExists(filePath);
        }

        public static bool  FileExists(string filePath)
        {

            StorageFolder storageFolder =Windows.Storage.ApplicationData.Current.LocalFolder;
            foreach (var folder in OOAdvantech.FileSystem.GetFolders(filePath))
            {
                var folderTask = storageFolder.GetFoldersAsync().AsTask();
                folderTask.Wait();
                bool folderExist = false;
                IReadOnlyList<StorageFolder> localFolders = folderTask.Result;
                foreach (var localFolder in localFolders)
                {
                    if (localFolder.Name.ToLower() == folder.ToLower())
                        folderExist = true;
                }
                if (!folderExist)
                    return false;
            }

            string fileName = OOAdvantech.FileSystem.GetFileName(filePath);
            var fileTask = storageFolder.GetFilesAsync().AsTask();
            fileTask.Wait();
            bool fileExist = false;
            IReadOnlyList<StorageFile> localfiles = fileTask.Result;

            foreach (var storageFile in localfiles)
            {
                if (storageFile.Name.ToLower() == fileName.ToLower())
                    return true;
            }
            return false;

        }


        Stream OOAdvantech.IFileSystem.Open(string filePath)
        {
            return Open(filePath);
        }

        public static Stream Open(string filePath)
        {

            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
          //  ExternalStorageDevice _sdCard = (await ExternalStorage.GetExternalStorageDevicesAsync()).FirstOrDefault();

            foreach (var folder in OOAdvantech.FileSystem.GetFolders(filePath))
            {

                var folderTask = storageFolder.GetFoldersAsync().AsTask();
                folderTask.Wait();
                bool folderExist = false;
                IReadOnlyList<StorageFolder> localFolders = folderTask.Result;
                foreach (var localFolder in localFolders)
                {
                    if (localFolder.Name == folder)
                        folderExist = true;
                }
                if (!folderExist)
                {
                    var folderCreationTask = storageFolder.CreateFolderAsync(folder).AsTask();
                    folderCreationTask.Wait();
                    storageFolder = folderCreationTask.Result;
                }
            }

            string fileName = OOAdvantech.FileSystem.GetFileName(filePath);
            var fileOpenCreationTask = storageFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists).AsTask();
            fileOpenCreationTask.Wait();
            var streamTask = fileOpenCreationTask.Result.OpenAsync(FileAccessMode.ReadWrite).AsTask();
            streamTask.Wait();

            return streamTask.Result.AsStream();
        }


        internal static string GetDeviceSpecificPath(string sqliteFilePath)
        {
            sqliteFilePath = sqliteFilePath.Trim();
            if (sqliteFilePath.IndexOf(@"\") == 0)
                sqliteFilePath=sqliteFilePath.Substring(1);


            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilePath);

            return path;
        }

        string IFileSystem.GetDeviceSpecificPath(string sQLiteFilePath)
        {
            return GetDeviceSpecificPath(sQLiteFilePath);
        }
    }
}
