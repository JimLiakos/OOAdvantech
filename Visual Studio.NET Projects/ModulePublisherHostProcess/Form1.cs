using System;
//using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ModulePublisherHostProcess
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    /// <MetaDataID>{b3ab2e39-3320-4ffb-9c98-8e093c7ec62b}</MetaDataID>
    public class ModulePublisherHostProcess
    {
         

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            

            //try
            //{
            //    System.IO.File.Delete(@"E:\X-Drive\source\OpenVersions\Visual Studio.NET Projects\ModulePublisher\bin\Debug\PerformanceTest\ModulePublisher.dll");
            //}
            //catch (Exception)
            //{

                 
            //} 
           //System.Windows.Forms.MessageBox.Show(System.Environment.UserName);
             
            bool remove = false;
            bool encrypted = false;
            bool setup = false;
            string assemblyFileName = null;
            string errorFile = null;
            string projectFullPath = null;

            foreach (string arg in args)
            {

                string _arg = arg.Trim();
                if (_arg.Length <= 1)
                {
                    System.Console.WriteLine("unknown argument \"" + arg + "\"");
                    return -20;
                }
                if (_arg[0] == '/')
                {
                    _arg = _arg.Substring(1);
                    _arg = _arg.Trim();
                    if (_arg == "u")
                    {
                        remove = true;
                        continue;
                    }
                    if (_arg == "enc")
                    {
                        encrypted = true;
                        continue;
                    }
                    if (_arg == "s")
                    {
                        setup = true;
                        continue;
                    }
                    _arg = _arg.ToLower();
                    if (_arg.IndexOf("errorfile:") != -1)
                    {
                        errorFile = _arg.Replace("errorfile:", "");
                        continue;
                    }
                    if (_arg.IndexOf("projectpath:") != -1)
                    {
                        projectFullPath = _arg.Replace("projectpath:", "");
                        continue;
                    }
                    System.Console.WriteLine("unknown argument \"" + arg + "\"");
                    return -20;
                }

                assemblyFileName = _arg;
                if (!System.IO.File.Exists(assemblyFileName))
                {
                    System.Console.WriteLine("Assembly \"" + assemblyFileName + "\" doesn't exist");
                    return -21;

                }
            }
            //System.Windows.Forms.MessageBox.Show(assemblyFileName);

            List<string> refrences = new List<string>();
            if(System.IO.File.Exists(errorFile))
            {
                try
                {
                    var xDoc = XDocument.Load(errorFile);
                    refrences=xDoc.Descendants("Reference").Where(x => x.Attribute("path") != null).Select(x => x.Attribute("path").Value).ToList();
                }
                catch (Exception error)
                {
                }
            }
         
            if (assemblyFileName == null)
            {

                System.Console.WriteLine("There isn't Assembly file name");
                return -22;
            }




            try
            {
                System.Reflection.Assembly assembly = null;
                if (!remove)
                {
                    if (encrypted)
                        ModulePublisher.ClassRepository.Publish(assemblyFileName, "", errorFile, encrypted, setup);
                    else
                    {
                        if (System.Reflection.AssemblyName.GetAssemblyName(assemblyFileName).ProcessorArchitecture != System.Reflection.ProcessorArchitecture.MSIL)
                            return 0;
                        try
                        {
                            ModulePublisher.ClassRepository.RemoveFromGAC(System.Reflection.AssemblyName.GetAssemblyName(assemblyFileName).FullName);
                        }
                        catch (Exception error)
                        {

                            throw;
                        }
                        assembly = System.Reflection.Assembly.LoadFrom(assemblyFileName);
                        if(assembly!=null)
                        {
                            foreach(var assemblyReference in assembly.GetReferencedAssemblies())
                            {
                                if (assemblyReference.Name == "Mono.Android" || assemblyReference.Name =="Xamarin.Forms.Core")
                                    return 0;
                            }
                        }
                        
                        ModulePublisher.ClassRepository.AddToGAC(assemblyFileName);
                        
                        ModulePublisher.ClassRepository.RemoveFromGAC(assembly.FullName);
                        if (assembly.GetCustomAttributes(typeof(ModulePublisher.Encrypt), false).Length > 0)
                        {



                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(assemblyFileName);
                            DateTime LastWriteTime = fileInfo.LastWriteTime;

                            string DebugInfo = fileInfo.FullName;
                            DebugInfo = DebugInfo.Replace(fileInfo.Extension, "");
                            DebugInfo += ".pdb";

                            System.IO.FileInfo DebugfileInfo = new System.IO.FileInfo(DebugInfo);
                            string dirName = fileInfo.Directory.Name;
                            System.IO.DirectoryInfo EncrypdirectoryInfo = new System.IO.DirectoryInfo(fileInfo.Directory.FullName + "\\enc" + fileInfo.Directory.Name);
                            if (!EncrypdirectoryInfo.Exists)
                                EncrypdirectoryInfo.Create();



                            //System.IO.FileInfo fileInfo=new System.IO.FileInfo(AssemblyFileName);

                            //foreach(System.IO.
                            string assemplyName = fileInfo.Name.Replace(fileInfo.Extension, "");
                            Microsoft.Win32.RegistryKey registryKey = null;

                            try
                            {
                                registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Assembly\\" + assemplyName + "\\" + System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyFileName).FileVersion + "\\");
                                if (registryKey != null)
                                {
                                    System.DateTime fileDateTime;
                                    fileDateTime = System.DateTime.Parse(registryKey.GetValue("Date") as string);
                                    registryKey.Close();

                                    if (fileDateTime.ToString() == LastWriteTime.ToString())
                                        return 0;
                                }
                            }
                            catch (System.Exception error)
                            {
                                int erte = 0;
                            }


                            System.IO.File.Copy(fileInfo.FullName, EncrypdirectoryInfo.FullName + "\\" + fileInfo.Name, true);
                            if (DebugfileInfo.Exists)
                                System.IO.File.Copy(DebugfileInfo.FullName, EncrypdirectoryInfo.FullName + "\\" + DebugfileInfo.Name, true);

                            string EncryptAssemblyFileName = EncrypdirectoryInfo.FullName + "\\" + fileInfo.Name;



                            //if(args.Length>1)
                            //	ModulePublisher.ClassRepository.EncryptandPublish(AssemblyFileName,args[1]);
                            //else


                            ModulePublisher.ClassRepository.EncryptandPublish(EncryptAssemblyFileName, "", errorFile); /**/

                            registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Assembly\\" + assemplyName + "\\" + System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyFileName).FileVersion + "\\", true);
                            if (registryKey != null)
                            {
                                registryKey.SetValue("Date", LastWriteTime.ToString());
                                registryKey.Close();
                            }



                        }
                        else
                        {
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(assemblyFileName);
                            DateTime LastWriteTime = fileInfo.LastWriteTime;

                            string assemplyName = fileInfo.Name.Replace(fileInfo.Extension, "");
                            Microsoft.Win32.RegistryKey registryKey = null;

                            try
                            {
                                registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Assembly\\" + assemplyName + "\\" + System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyFileName).FileVersion + "\\");
                                if (registryKey != null)
                                {
                                    System.DateTime fileDateTime;
                                    fileDateTime = System.DateTime.Parse(registryKey.GetValue("Date") as string);
                                    registryKey.Close();

                                    if (fileDateTime.ToString() == LastWriteTime.ToString())
                                    {
                                        ModulePublisher.ClassRepository.AddToGAC(assemblyFileName);
                                        return 0;
                                    }
                                }
                            }
                            catch (System.Exception error)
                            {
                                int erte = 0;
                            }
                            // System.Windows.Forms.MessageBox.Show("TMPStop");
                            //if(args.Length>1)
                            //	ModulePublisher.ClassRepository.Publish(AssemblyFileName,args[1]);
                            //else
                            ModulePublisher.ClassRepository.Publish(assemblyFileName, "", errorFile, setup); /**/
                        }
                    }
                }
                else
                    ModulePublisher.ClassRepository.Remove(assemblyFileName, "", encrypted);
               
            }
           
            catch (System.Exception Error)
            {


                while (Error.InnerException != null)
                    Error = Error.InnerException;
                System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                System.Xml.XmlElement errorsElement = xmlDocument.CreateElement("Errors");
                xmlDocument.AppendChild(errorsElement);
                System.Xml.XmlElement errorElement = xmlDocument.CreateElement("Error");
                errorsElement.AppendChild(errorElement);

                if (Error.GetType().FullName == "OOAdvantech.MetaDataRepository.MetaDataException")
                {
                    object metaDataError = Error.GetType().GetField("MetaDataError").GetValue(Error);
                    errorElement.InnerText = metaDataError.GetType().GetField("ErrorMessage").GetValue(metaDataError) as string; ;
                    errorElement.SetAttribute("FileName", metaDataError.GetType().GetField("ErrorPath").GetValue(metaDataError) as string);
                    errorElement.SetAttribute("LineNumber", ((int)1).ToString());
                }
                else
                {
                    errorElement.InnerText = Error.Message + "  " + Error.StackTrace;
                    errorElement.SetAttribute("FileName", " ");
                    errorElement.SetAttribute("LineNumber", ((int)1).ToString());
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(errorFile))
                        xmlDocument.Save(errorFile);

                }
                catch (Exception error)
                {
                    
                }
                //System.Windows.Forms.MessageBox.Show(Error.Message + "  \n" + Error.StackTrace);
                //System.Windows.Forms.MessageBox.Show(AssemblyFileName);
                return -23;
            }
            finally
            { 

               //System.Windows.Forms.MessageBox.Show(projectFullPath);

                if (!string.IsNullOrWhiteSpace(projectFullPath) &&System.IO.Directory.Exists(projectFullPath))
                {
                    projectFullPath=projectFullPath.Trim();
                    if (projectFullPath[projectFullPath.Length - 1] == '\\')
                        projectFullPath = projectFullPath.Substring(0, projectFullPath.Length - 1);
                    ModulePublisher.ClassRepository.ErrorLoadingRefDll.Clear();
                    ModulePublisher.ClassRepository.RefDlls = refrences;

                    var assembly= System.Reflection.Assembly.LoadFrom(assemblyFileName);
                    FacadeProxiesGenerator.ProxiesGenerator.GenerateProxies(assembly, FacadeProxiesGenerator.ProxiesOutput.CSharp, projectFullPath, errorFile);



                
                }
            }
            return 0;


        }
    }
}
