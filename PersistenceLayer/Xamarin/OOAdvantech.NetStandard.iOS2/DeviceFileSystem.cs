using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace OOAdvantech.iOS
{
    /// <MetaDataID>{93e8ed40-e5ff-4cf0-ad70-58feca95f976}</MetaDataID>
    public class DeviceFileSystem : OOAdvantech.IFileSystem
    {
        public void DeleteFile(string filePath)
        {
            var path = CreatePathToFile(filePath);
            File.Delete(path);
        }

        public void DeleteFolder(string folderPath)
        {
            Directory.Delete(CreatePathToFile(folderPath));
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(CreatePathToFile(filePath));
        }

        public string GetDeviceSpecificPath(string sqliteFilePath)
        {
            return CreatePathToFile(sqliteFilePath);
        }

        public Stream Open(string filePath)
        {
            string path = CreatePathToFile(filePath);
            FileStream stream = File.Open(path, FileMode.OpenOrCreate);
            return stream;
        }

        public static string DocumentsPath
        {
            get
            {
                var documentsDirUrl = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User).Last();
                return documentsDirUrl.Path;
            }
        }

        static string CreatePathToFile(string fileName)
        {
            return Path.Combine(DocumentsPath, fileName);
        }
    }



}