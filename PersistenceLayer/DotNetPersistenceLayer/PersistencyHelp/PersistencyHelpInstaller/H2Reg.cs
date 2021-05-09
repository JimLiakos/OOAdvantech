using System;
using System.Collections.Generic;
using System.Text;

namespace PersistencyHelpInstaller
{
    /// <MetaDataID>{36b35918-396f-4817-8d10-dd475316b8c4}</MetaDataID>
    public class H2Reg
    {
        public static void UnPackResources(string directory)
        {

            EmbeddedResources.WriteEmbeddedResources(
                typeof(PersistencyHelpInstaller).Module.Assembly,
                "PersistencyHelpInstaller.Plug_In",
                directory);
        }
        public static void RegisterPlugIn(string directory)
        {
            string commandLine = directory + "\\H2Reg.exe";
            string arguments = " -R cmdfile=" + directory + "\\H2Reg.ini";
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(commandLine, arguments);
            short Count = 0;
            while (Count < 10 && !process.HasExited)
            {
                System.Threading.Thread.Sleep(500);
                Count++;
            }

        }
        public static void UnRegisterPlugIn(string directory)
        {
            string commandLine = directory + "\\H2Reg.exe";
            string arguments = " -U cmdfile=" + directory + "\\H2Reg.ini";
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(commandLine, arguments);
            short Count = 0;
            while (Count < 10 && !process.HasExited)
            {
                System.Threading.Thread.Sleep(500);
                Count++;
            }

        }

        public static void RemoveResourcesFiles(string directory)
        {
            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(directory);
            if (directoryInfo.Exists)
            {
                foreach (System.IO.FileInfo fileinfo in directoryInfo.GetFiles())
                    try { System.IO.File.Delete(fileinfo.FullName); }
                    catch (System.Exception) { }

            }
        }
        public static void RemoveTempFiles(string directory)
        {
            try { System.IO.File.Delete(directory + "\\H2Reg.exe"); }
            catch (System.Exception) { }
            try { System.IO.File.Delete(directory + "\\h2reg.ini"); }
            catch (System.Exception) { }
            try { System.IO.File.Delete(directory + "\\H2Reg_Log.txt"); }
            catch (System.Exception) { }

        }

        public static void BuildNames(string directory, string plug_In, string plug_InParent, string plug_InName, string helpCollection)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(directory + "\\Plug-In.HxC");
            try { System.IO.File.Delete(directory + "\\" + plug_In + ".HxC"); }
            catch (System.Exception) { }
            fileInfo.MoveTo(directory + "\\" + plug_In + ".HxC");
            fileInfo = new System.IO.FileInfo(directory + "\\Plug-In.HxK");
            try { System.IO.File.Delete(directory + "\\" + plug_In + ".HxK"); }
            catch (System.Exception) { }
            fileInfo.MoveTo(directory + "\\" + plug_In + ".HxK");
            fileInfo = new System.IO.FileInfo(directory + "\\Plug-In.HxT");
            try { System.IO.File.Delete(directory + "\\" + plug_In + ".HxT"); }
            catch (System.Exception) { }
            fileInfo.MoveTo(directory + "\\" + plug_In + ".HxT");


            fileInfo = new System.IO.FileInfo(directory + "\\Plug-In_F.HxK");
            try { System.IO.File.Delete(directory + "\\" + plug_In + "_F.HxK"); }
            catch (System.Exception) { }
            fileInfo.MoveTo(directory + "\\" + plug_In + "_F.HxK");

            fileInfo = new System.IO.FileInfo(directory + "\\Plug-In_K.HxK");
            try { System.IO.File.Delete(directory + "\\" + plug_In + "_K.HxK"); }
            catch (System.Exception) { }
            fileInfo.MoveTo(directory + "\\" + plug_In + "_K.HxK");

            string CommandFile = directory + "\\h2reg.ini";

            string h2regString;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(CommandFile))
            {
                h2regString = sr.ReadToEnd();
                sr.Close();
            }


            h2regString = h2regString.Replace("[Plug-In]", plug_In);
            h2regString = h2regString.Replace("[Plug-InName]", plug_InName);
            h2regString = h2regString.Replace("[Help]", helpCollection);
            h2regString = h2regString.Replace("[Plug-InParent]", plug_InParent);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(CommandFile))
            {
                sw.Write(h2regString);
                sw.Close();
            }

            string TOCFile = directory + "\\" + plug_In + ".HxT";

            string Plug_In_HxT;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(TOCFile))
            {
                Plug_In_HxT = sr.ReadToEnd();
                sr.Close();
            }
            Plug_In_HxT = Plug_In_HxT.Replace("[Plug-In]", plug_In);
            Plug_In_HxT = Plug_In_HxT.Replace("[Plug-InName]", plug_InName);
            Plug_In_HxT = Plug_In_HxT.Replace("[Help]", helpCollection);
            Plug_In_HxT = Plug_In_HxT.Replace("[URL]", "OOAdvanceTech/html/MSDN Library Start.htm");

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(TOCFile))
            {
                sw.Write(Plug_In_HxT);
                sw.Close();
            }


            string collectionFile = directory + "\\" + plug_In + ".HxC";

            string Plug_In_HxC;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(collectionFile))
            {
                Plug_In_HxC = sr.ReadToEnd();
                sr.Close();
            }
            Plug_In_HxC = Plug_In_HxC.Replace("[Plug-In]", plug_In);
            Plug_In_HxC = Plug_In_HxC.Replace("[Plug-InName]", plug_InName);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(collectionFile))
            {
                sw.Write(Plug_In_HxC);
                sw.Close();
            }


        }



    }
}
