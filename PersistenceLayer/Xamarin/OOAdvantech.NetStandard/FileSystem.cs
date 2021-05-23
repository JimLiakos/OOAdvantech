using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech
{
    /// <MetaDataID>{7b75fbbe-2afc-4f43-bf06-a48f747d91ce}</MetaDataID>
    static public class FileSystem
    {

        public static System.Collections.Generic.List<string> GetFolders(string filePath)
        {
            System.Collections.Generic.List<string> folders = new System.Collections.Generic.List<string>();
            //string filePath = "dfg";// @"DirB\tes.txt".Trim();
            filePath = filePath.Trim();
            if (filePath.Length > 0)
            {
                if (filePath[0] == '\\')
                    filePath = filePath.Substring(1);
                string fileName = Path.GetFileName(filePath);
                string folderPath = Path.GetDirectoryName(filePath);
                while (folderPath.IndexOf(@"\") != -1)
                {
                    string folder = folderPath.Substring(0, folderPath.IndexOf(@"\"));
                    folderPath = folderPath.Substring(folderPath.IndexOf(@"\") + 1);
                    folders.Add(folder);
                }
                if (!string.IsNullOrWhiteSpace(folderPath))
                    folders.Add(folderPath);
            }
            return folders;
        }

        static public string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath.Replace(@"\", "/"));
            //return Path.GetFileName(filePath);
        }
        //     The caller does not have the required permission.
        public static void WriteAllText(string path, string contents)
        {
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            IFileSystem fileSystem = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as IFileSystem;

            using (System.IO.Stream stream = fileSystem.Open(path))
            {
                stream.SetLength(0);
                var contentsBytes = System.Text.Encoding.Unicode.GetBytes(contents);
                stream.Write(contentsBytes, 0, contentsBytes.Length);
            }


        }

        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            IFileSystem fileSystem = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as OOAdvantech.IFileSystem;
            using (System.IO.Stream stream = fileSystem.Open(path))
            {
                stream.SetLength(0);
                var contentsBytes = encoding.GetBytes(contents);
                stream.Write(contentsBytes, 0, contentsBytes.Length);
            }
        }

        public static string ReadAllText(string path)
        {
            string contents = null;
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IFileSystem fileSystem = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as OOAdvantech.IFileSystem;
            if (!fileSystem.FileExists(path))
                return contents;
            
            using (System.IO.Stream stream = fileSystem.Open(path))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                contents=Encoding.Unicode.GetString(buffer);
            }
            return contents;

        }

        public static string ReadAllText(string path, Encoding encoding)
        {
            string contents = null;
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IFileSystem fileSystem = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IFileSystem)) as OOAdvantech.IFileSystem;
            if (!fileSystem.FileExists(path))
                return contents;

            using (System.IO.Stream stream = fileSystem.Open(path))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                contents = encoding.GetString(buffer);
            }
            return contents;
        }
    }


    /// <MetaDataID>{0b2fe50d-f195-4f4a-8b50-0e061cf99723}</MetaDataID>
    public interface IFileSystem
    {
        bool FileExists(string filePath);

        System.IO.Stream Open(string filePath);

        void DeleteFile(string filePath);

        void DeleteFolder(string folderPath);
        string GetDeviceSpecificPath(string sQLiteFilePath);
    }

    /// <MetaDataID>{946eb592-048b-4e0f-a7ec-d4f12b1bdc6c}</MetaDataID>
    public interface IDBConnaction
    {
        bool IsOpen { get; }

    }


}
