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
