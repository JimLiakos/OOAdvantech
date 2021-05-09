using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace OOAdvantech.Droid
{
    /// <MetaDataID>{921220d2-f148-4903-af7f-4b4d308f2847}</MetaDataID>
    public class DeviceFileSystem : OOAdvantech.IFileSystem
    {
        void OOAdvantech.IFileSystem.DeleteFile(string filePath)
        {
            DeleteFile(filePath);
        }

        public static void DeleteFile(string filePath)
        {
            var path = CreatePathToFile(filePath);
            File.Delete(path);
        }


        void OOAdvantech.IFileSystem.DeleteFolder(string folderPath)
        {
            DeleteFolder(folderPath);
        }

        public static void DeleteFolder(string folderPath)
        {
            throw new NotImplementedException();
        }

        bool OOAdvantech.IFileSystem.FileExists(string filename)
        {
            return FileExists(CreatePathToFile(filename));
        }
        public static bool FileExists(string filename)
        {
            return File.Exists(CreatePathToFile(filename));
        }


        static string CreatePathToFile(string filename)
        {
            string path = null;
            foreach (var folder in OOAdvantech.FileSystem.GetFolders(filename))
            {
                if (!string.IsNullOrWhiteSpace(path))
                    path += "/";
                path += folder;
            }
            if (!string.IsNullOrWhiteSpace(path))
                path += "/";
            filename=path + OOAdvantech.FileSystem.GetFileName(filename);

            


            var docsPath =System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(docsPath, filename);
        }


        Stream OOAdvantech.IFileSystem.Open(string filePath)
        {
            return Open(filePath);
        }

        public static Stream Open(string filePath)
        {
            var path = CreatePathToFile(filePath);
            Stream sw = File.Open(path,FileMode.OpenOrCreate,FileAccess.ReadWrite);
            return sw;


            //StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            ////  ExternalStorageDevice _sdCard = (await ExternalStorage.GetExternalStorageDevicesAsync()).FirstOrDefault();

            //foreach (var folder in OOAdvantech.FileSystem.GetFolders(filePath))
            //{

            //    var folderTask = storageFolder.GetFoldersAsync().AsTask();
            //    folderTask.Wait();
            //    bool folderExist = false;
            //    IReadOnlyList<StorageFolder> localFolders = folderTask.Result;
            //    foreach (var localFolder in localFolders)
            //    {
            //        if (localFolder.Name == folder)
            //            folderExist = true;
            //    }
            //    if (!folderExist)
            //    {
            //        var folderCreationTask = storageFolder.CreateFolderAsync(folder).AsTask();
            //        folderCreationTask.Wait();
            //        storageFolder = folderCreationTask.Result;
            //    }
            //}

            //string fileName = OOAdvantech.FileSystem.GetFileName(filePath);
            //var fileOpenCreationTask = storageFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists).AsTask();
            //fileOpenCreationTask.Wait();
            //var streamTask = fileOpenCreationTask.Result.OpenAsync(FileAccessMode.ReadWrite).AsTask();
            //streamTask.Wait();

            //return streamTask.Result.AsStream();
        }

        internal static string GetDeviceSpecificPath(string sqliteFilePath)
        {
            return CreatePathToFile(sqliteFilePath); 
        }

        string IFileSystem.GetDeviceSpecificPath(string sqliteFilePath)
        {
            return GetDeviceSpecificPath(sqliteFilePath);
        }
    }
}