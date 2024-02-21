using System;
using System.Globalization;
using System.Reflection;
//using System.ServiceModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace ModulePublisher
{
    /// <summary>Summary description for Class1.</summary>
    /// <MetaDataID>{82B498BC-BBAD-4B8B-8ABA-427F459A980E}</MetaDataID>
    public class ClassRepository
    {

#if !DeviceDotNet
        static string GACLocation
        {
            get
            {


                try
                {
                    string location;
                    Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\OOAdvantech");
                    if (registryKey == null)
                        registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\OOAdvantech");
#if Net4
                    location = registryKey.GetValue("Net4ApplicationPath") as string;
#else
                    location = registryKey.GetValue("ApplicationPath") as string;
#endif
                    registryKey.Close();
                    return location;
                }
                catch (System.Exception)
                {
                }
                ModulePublisher.ServiceReference.IModulePublisher sdf = null;
                return "";
            }



        }



        static ModulePublisher.ServiceReference.IModulePublisher _ModulePublisherService;
        static public ModulePublisher.ServiceReference.IModulePublisher ModulePublisherService
        {
            get
            {
                if (_ModulePublisherService == null)
                {
                    System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress("http://localhost:4321/ModulePublisherService");
                    System.ServiceModel.WSHttpBinding binding = new System.ServiceModel.WSHttpBinding();
                    _ModulePublisherService = new ModulePublisher.ServiceReference.ModulePublisherClient(binding, address);
                }
                return _ModulePublisherService;
            }
        }
#endif

        static public void AddToGAC(string assemblyFileName)
        {


#if !DeviceDotNet

            try
            {
                var assembly = System.Reflection.Assembly.LoadFrom(assemblyFileName);
                object[] objects = assembly.GetCustomAttributes(typeof(AddGAC), false);
                if (objects.Length == 0)
                    return;
            }
            catch (Exception error)
            {
                return;
            }
            System.Diagnostics.ProcessStartInfo ProcessStartInfo;
            string gacLocation = GACLocation;
            try
            {

                string fileName = null;
                string arguments = null;
                if (System.IO.File.Exists(gacLocation + "gacutil.exe"))
                {
                    fileName = gacLocation + "gacutil.exe";
                    arguments = "-if \"" + assemblyFileName + "\"";

                }
                else
                {
                    fileName = gacLocation + "gacutil.exe";
                    arguments = "-if \"" + assemblyFileName + "\"";
                }
                ModulePublisherService.ExecudeModulePublishCommand(fileName, arguments);
            }
            catch (Exception error)
            {


            }


            //ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(fileName,arguments);
            //ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            //System.Diagnostics.Process process = System.Diagnostics.Process.Start(ProcessStartInfo);
            //int Count = 0;
            //while (!process.HasExited && Count < 40)
            //{
            //    Count++;
            //    System.Threading.Thread.Sleep(200);
            //}

#endif
        }

        static public void RemoveFromGAC(string assemblyFileName)
        {
#if !DeviceDotNet

            try
            {
                var assembly = System.Reflection.Assembly.LoadFrom(assemblyFileName);
                object[] objects = assembly.GetCustomAttributes(typeof(AddGAC), false);
                if (objects.Length == 0)
                    return;
            }
            catch (Exception error)
            {
                return;
            }

            string gacLocation = GACLocation;

            string fileName = null;
            string arguments = null;
            try
            {
                if (System.IO.File.Exists(gacLocation + "gacutil.exe"))
                {
                    fileName = gacLocation + "gacutil.exe";
                    arguments = "-u \"" + assemblyFileName + "\"";

                }
                else
                {
                    fileName = gacLocation + "gacutil.exe";
                    arguments = "-u \"" + assemblyFileName + "\"";
                }

                ModulePublisherService.ExecudeModulePublishCommand(fileName, arguments);
            }
            catch (Exception error)
            {


            }

            //System.Diagnostics.ProcessStartInfo ProcessStartInfo;

            //if (System.IO.File.Exists(gacLocation + "gacutil.exe"))
            //    ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(gacLocation + "gacutil.exe", "-u \"" + assemblyFileName + "\"");
            //else
            //    ProcessStartInfo = new System.Diagnostics.ProcessStartInfo("gacutil.exe", "-u \"" + assemblyFileName + "\"");
            //ProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            //System.Diagnostics.Process process = System.Diagnostics.Process.Start(ProcessStartInfo);
            //int Count = 0;
            //while (!process.HasExited && Count < 40)
            //{
            //    Count++;
            //    System.Threading.Thread.Sleep(200);
            //}
#endif
        }

#if !DeviceDotNet
        static System.Reflection.Assembly GetAssemblyForClass(string classFullName, string assemblyData)
        {
            string AssemblyName = null, AssemblyFullName = null, AssemblyDllFile = null;
            bool GlobalAssemblyCache = false;
            bool Encrypted = false;
            if (!GetClassInfo(classFullName, assemblyData, ref AssemblyName, ref AssemblyFullName, ref GlobalAssemblyCache, ref AssemblyDllFile, ref Encrypted))
                return null;
            System.Reflection.Assembly assembly = null;
            if (ClassesAssemblies.ContainsKey(AssemblyFullName))
                return (System.Reflection.Assembly)ClassesAssemblies[AssemblyFullName];
            else
            {
                //Error prone
                if (GlobalAssemblyCache && !Encrypted)
                {
                    try
                    {
                        if (AssemblyFullName != null)
                            assembly = AppDomain.Load(AssemblyFullName);
                    }
                    catch (System.Exception)
                    {
                    }
                    return assembly;
                }


                if (Encrypted)
                    assembly = AssemblyNativeCode.AssemblyLoader.LoadAssembly(AssemblyDllFile);
                else
                    assembly = LoadFrom(AssemblyDllFile);

                foreach (System.Reflection.AssemblyName reference in assembly.GetReferencedAssemblies())
                {
                    try
                    {
                        if (!ClassesAssemblies.ContainsKey(reference.FullName))
                            ClassesAssemblies[reference.FullName] = AppDomain.Load(reference.FullName);

                    }
                    catch (Exception error)
                    {


                    }
                }

                ClassesAssemblies[AssemblyFullName] = assembly;
                return assembly;
            }

        }
        /// <MetaDataID>{b1c7a05c-90a9-425f-8c0d-4d8435399d3f}</MetaDataID>
        static public bool ClassVersionDataMatchWithAssembly(string versionData, string assemblyFullName)
        {
            string versionDataName, versionDataVersion, versionDataCulture, versionDataPublicKeyToken;
            string name, version, culture, publicKeyToken;
            GetAssemblyDataFromFullName(versionData, out versionDataName, out versionDataVersion, out versionDataCulture, out versionDataPublicKeyToken);


            GetAssemblyDataFromFullName(assemblyFullName, out name, out version, out culture, out publicKeyToken);

            if (!string.IsNullOrEmpty(versionDataName) && versionDataName.Trim().ToLower() != name.ToLower().Trim())
                return false;
            if (!string.IsNullOrEmpty(versionDataVersion) && versionDataVersion.Trim().ToLower() != version.ToLower().Trim())
                return false;
            if (!string.IsNullOrEmpty(versionDataCulture) && versionDataCulture.Trim().ToLower() != culture.ToLower().Trim())
                return false;
            if (!string.IsNullOrEmpty(versionDataPublicKeyToken) && versionDataPublicKeyToken.Trim().ToLower() != publicKeyToken.ToLower().Trim())
                return false;

            return true;

        }

        public static bool ClassVersionDataMatchAllWithAssembly(string assemblyData, string assemblyFullName)
        {
            if (assemblyData == null)
                assemblyData = "";

            string versionDataName, versionDataVersion, versionDataCulture, versionDataPublicKeyToken;
            string name, version, culture, publicKeyToken;
            GetAssemblyDataFromFullName(assemblyData, out versionDataName, out versionDataVersion, out versionDataCulture, out versionDataPublicKeyToken);


            GetAssemblyDataFromFullName(assemblyFullName, out name, out version, out culture, out publicKeyToken);

            if (versionDataName.Trim().ToLower() != name.ToLower().Trim())
                return false;
            if (versionDataVersion.Trim().ToLower() != version.ToLower().Trim())
                return false;
            if (versionDataCulture.Trim().ToLower() != culture.ToLower().Trim())
                return false;
            if (versionDataPublicKeyToken.Trim().ToLower() != publicKeyToken.ToLower().Trim())
                return false;

            return true;

        }

#endif
#if DeviceDotNet
        public static System.Collections.Generic.List<System.Reflection.Assembly> LoadedAssemblies = new System.Collections.Generic.List<System.Reflection.Assembly>();
#endif
        /// <MetaDataID>{BDAF9415-A8A2-4BD6-9528-E5FAE646F60F}</MetaDataID>
        public static System.Type GetType(string classFullName, string assemblyData)
        {
            try
            {
                System.Type type = null;
                if (Types.TryGetValue(classFullName + "[" + assemblyData + "]", out type))
                    return type;

                type = GetTypeFromEmbeddedModule(classFullName, assemblyData);
                if (type != null)
                {
                    Types[classFullName + "[" + assemblyData + "]"] = type;
                    return type;
                }
#if DeviceDotNet
                return type;
#else
                Assembly classAssembly = null;
                if (assemblyData != null)
                {
                    ClassesAssemblies.TryGetValue(assemblyData, out classAssembly);
                    if (classAssembly == null)
                        classAssembly = (from assem in System.AppDomain.CurrentDomain.GetAssemblies()
                                         where ClassVersionDataMatchWithAssembly(assemblyData, assem.FullName)
                                         select assem).FirstOrDefault();
                    else
                    {

                    }
                }

                if (classAssembly == null && !string.IsNullOrEmpty(assemblyData))
                {
                    try
                    {
                        classAssembly = Assembly.Load(assemblyData);
                    }
                    catch (Exception error)
                    {
                    }
                }

                if (classAssembly != null)
                {
                    ClassesAssemblies[classAssembly.FullName] = classAssembly;
                    type = classAssembly.GetType(classFullName);
                    if (type != null && !string.IsNullOrEmpty(assemblyData))
                    {
                        if (ClassVersionDataMatchWithAssembly(assemblyData, classAssembly.FullName))
                        {
                            Types[classFullName + "[" + assemblyData + "]"] = type;
                            return type;
                        }
                    }
                    else if (type != null && string.IsNullOrEmpty(assemblyData))
                    {
                        Types[classFullName + "[" + assemblyData + "]"] = type;
                        return type;
                    }
                }


                foreach (System.Reflection.Assembly currAssembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = currAssembly.GetType(classFullName);
                    if (type != null && !string.IsNullOrEmpty(assemblyData))
                    {
                        if (ClassVersionDataMatchWithAssembly(assemblyData, currAssembly.FullName))
                        {
                            Types[classFullName + "[" + assemblyData + "]"] = type;
                            return type;
                        }
                    }
                    else if (type != null && string.IsNullOrEmpty(assemblyData))
                    {
                        Types[classFullName + "[" + assemblyData + "]"] = type;
                        return type;
                    }
                }
                Assembly assembly = GetAssemblyForClass(classFullName, assemblyData);
                if (assembly != null)
                {
                    type = assembly.GetType(classFullName);
                    if (assemblyData == null)
                        assemblyData = "";

                    if (type != null)
                    {
                        if (ClassVersionDataMatchWithAssembly(assemblyData, assembly.FullName))
                        {
                            Types[classFullName + "[" + assemblyData + "]"] = type;
                            return type;
                        }
                    }

                }


                if (Assembly.GetEntryAssembly() != null)
                {
                    foreach (var assemblyReference in Assembly.GetEntryAssembly().GetReferencedAssemblies())
                    {
                        Assembly currAssembly = AppDomain.Load(assemblyReference);
                        if (currAssembly == null)
                            currAssembly = LoadAssembly(assemblyReference.FullName);
                        if (currAssembly != null)
                        {
                            type = currAssembly.GetType(classFullName);
                            if (type != null && !string.IsNullOrEmpty(assemblyData))
                            {
                                if (ClassVersionDataMatchWithAssembly(assemblyData, currAssembly.FullName))
                                {
                                    Types[classFullName + "[" + assemblyData + "]"] = type;
                                    return type;
                                }
                            }
                            else if (type != null && string.IsNullOrEmpty(assemblyData))
                            {
                                Types[classFullName + "[" + assemblyData + "]"] = type;
                                return type;
                            }
                        }
                    }
                }



                assembly = GetAssemblyForClass(classFullName, assemblyData);
                if (assembly == null)
                {
                    type = null;
                    foreach (System.Reflection.Assembly currAssembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        type = currAssembly.GetType(classFullName);
                        if (type != null)
                            break;
                    }
                    if (!Types.ContainsKey(classFullName + "[" + assemblyData + "]"))
                        Types[classFullName + "[" + assemblyData + "]"] = type;

                    return type;
                }
                //throw new System.Exception("System can't loads Assembly for class '" + ClassFullName + "'");
                type = assembly.GetType(classFullName);

                if (type == null)
                {
                    if (!Types.ContainsKey(classFullName + "[" + assemblyData + "]"))
                        Types[classFullName + "[" + assemblyData + "]"] = null;
                    return null;

                }
                if (!Types.ContainsKey(classFullName + "[" + assemblyData + "]"))
                    Types[classFullName + "[" + assemblyData + "]"] = type;
                return type;
#endif
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("ModulePublisher can't retrieve type of '" + classFullName + "'.", Error);
            }

        }


        public static System.Reflection.Assembly GetAssembly(string assemblyData)
        {
            try
            {


                var classAssembly = (from assem in System.AppDomain.CurrentDomain.GetAssemblies()
                                     where assem.GetName().FullName == assemblyData
                                     select assem).FirstOrDefault();

                if (classAssembly == null && !string.IsNullOrEmpty(assemblyData))
                {
                    try
                    {
                        classAssembly = Assembly.Load(assemblyData);
                    }
                    catch (Exception error)
                    {
                    }
                }

                if (classAssembly != null)
                    return classAssembly;



                foreach (System.Reflection.Assembly currAssembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (currAssembly.FullName == assemblyData)
                        return currAssembly;
                }



                if (Assembly.GetEntryAssembly() != null)
                {
                    foreach (var assemblyReference in Assembly.GetEntryAssembly().GetReferencedAssemblies())
                    {
#if !DeviceDotNet
                        Assembly currAssembly = AppDomain.Load(assemblyReference);
#else
                        Assembly currAssembly = null;
#endif
                        if (currAssembly == null)
                            currAssembly = LoadAssembly(assemblyReference.FullName);
                        if (currAssembly != null)
                            return currAssembly;

                    }
                }




                return null;
            }
            catch (System.Exception Error)
            {
                throw new System.Exception("ModulePublisher can't load assembly of '" + assemblyData + "'.", Error);
            }

        }


        public static ITypeLoader TypeLoader;
        private static Type GetTypeFromEmbeddedModule(string classFullName, string assemblyData)
        {
            //return null;
            if (TypeLoader == null)
            {
#if !DeviceDotNet
                Assembly OOAdvantech = AppDomain.CurrentDomain.Load("OOAdvantech, Version=1.0.2.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643");
                TypeLoader = OOAdvantech.CreateInstance("OOAdvantech.TypeLoader") as ITypeLoader;
#else
                TypeLoader = new OOAdvantech.TypeLoader();
#endif


            }
            return TypeLoader.GetType(classFullName, assemblyData);

        }
        /// <MetaDataID>{317A5FA2-EB51-48EC-8282-F588C892598F}</MetaDataID>
        private static System.Collections.Generic.Dictionary<string, Assembly> ClassesAssemblies = new System.Collections.Generic.Dictionary<string, Assembly>();

#if !DeviceDotNet


        /// <MetaDataID>{17E3024C-9CEF-43AB-A2E3-61CDD6CD1006}</MetaDataID>
        public static void GetAssemblyDataFromFullName(string fullName, out string name, out string version, out string culture, out string publicKeyToken)
        {
            version = "";
            culture = "";
            publicKeyToken = "";

            int startIndex = 0;
            int index = fullName.IndexOf(",", startIndex);
            if (index == -1)
            {
                name = fullName;
                return;


            }
            int versionStartIndex = fullName.ToLower().IndexOf("version");
            if (versionStartIndex != -1)
            {
                int versionEndIndex = fullName.IndexOf(",", versionStartIndex);
                if (versionEndIndex != -1)
                    version = fullName.Substring(versionStartIndex, versionEndIndex - versionStartIndex);
                else
                    version = fullName.Substring(versionStartIndex, fullName.Length - versionStartIndex);
                fullName = fullName.Replace(version, "");
                version = version.Substring(version.IndexOf('=') + 1).Trim();
            }

            int publicKeyTokenStartIndex = fullName.ToLower().IndexOf("publickeytoken");
            if (publicKeyTokenStartIndex != -1)
            {
                int publicKeyTokenEndIndex = fullName.IndexOf(",", publicKeyTokenStartIndex);
                if (publicKeyTokenEndIndex != -1)
                    publicKeyToken = fullName.Substring(publicKeyTokenStartIndex, publicKeyTokenEndIndex - publicKeyTokenStartIndex);
                else
                    publicKeyToken = fullName.Substring(publicKeyTokenStartIndex, fullName.Length - publicKeyTokenStartIndex);

                fullName = fullName.Replace(publicKeyToken, "");
                publicKeyToken = publicKeyToken.Substring(publicKeyToken.IndexOf('=') + 1).Trim();
            }

            int cultureStartIndex = fullName.ToLower().IndexOf("culture");
            if (cultureStartIndex != -1)
            {
                int cultureEndIndex = fullName.IndexOf(",", cultureStartIndex);
                if (cultureEndIndex != -1)
                    culture = fullName.Substring(cultureStartIndex, cultureEndIndex - cultureStartIndex);
                else
                    culture = fullName.Substring(cultureStartIndex, fullName.Length - cultureStartIndex);
                fullName = fullName.Replace(culture, "");
                culture = culture.Substring(culture.IndexOf('=') + 1).Trim();
            }
            fullName = fullName.Replace(",", "").Trim(); ;



            name = fullName;
            //startIndex = index + 1;
            //index = fullName.IndexOf(",", startIndex);
            //version = fullName.Substring(startIndex, index - startIndex);
            //version = version.Replace("Version", "");
            //version = version.Replace("=", "");
            //version = version.Trim();

            //startIndex = index + 1;
            //index = fullName.IndexOf(",", startIndex);
            //culture = fullName.Substring(startIndex, index - startIndex);
            //culture = culture.Replace("Culture", "");
            //culture = culture.Replace("=", "");
            //culture = culture.Trim();

            //startIndex = index + 1;
            //index = fullName.Length;
            //publicKeyToken = fullName.Substring(startIndex, index - startIndex);
            //publicKeyToken = publicKeyToken.Replace("PublicKeyToken", "");
            //publicKeyToken = publicKeyToken.Replace("=", "");
            //publicKeyToken = publicKeyToken.Trim();
        }


        /// <MetaDataID>{7D986B5C-A742-494C-B728-F9E315EC9807}</MetaDataID>
        static ClassRepository()
        {


            _AppDomain = System.AppDomain.CurrentDomain;
            _AppDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);
            _AppDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            AssemblyNativeCode.NativeCodeBridge.AppDomain = _AppDomain;

        }
        static AppDomain _AppDomain;
        static public AppDomain AppDomain
        {
            get
            {
                return _AppDomain;
            }
            //set
            //{
            //    if (_AppDomain != null)
            //    {
            //        _AppDomain.AssemblyResolve -= new ResolveEventHandler(MyResolveEventHandler);
            //        _AppDomain.AssemblyLoad -= new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            //    }
            //    _AppDomain = value;
            //    if (_AppDomain != null)
            //    {
            //        _AppDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);
            //        _AppDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);

            //    }
            //    AssemblyNativeCode.NativeCodeBridge.AppDomain = _AppDomain;
            //}
        }

        static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {

        }
        /// <MetaDataID>{41252044-5F32-4C55-A9D3-6963AF0030DD}</MetaDataID>
        static System.Reflection.Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            return LoadAssembly(args.Name);

        }

        public static Assembly LoadFrom(string assemblyDllFile)
        {
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.CodeBase = assemblyDllFile;
            Assembly assembly = AppDomain.Load(assemblyName);
            return assembly;
        }
#endif
        public static System.Collections.Generic.List<string> RefDlls = new System.Collections.Generic.List<string>();
        public static System.Collections.Generic.List<string> ErrorLoadingRefDll = new System.Collections.Generic.List<string>();
        static string LoadAssemblyFullName;
        static System.Collections.Generic.Dictionary<string, bool> TryToLoadAssemblies = new System.Collections.Generic.Dictionary<string, bool>();

        public static System.Reflection.Assembly LoadAssembly(string assemblyFullName)
        {
            try
            {
                if (LoadAssemblyFullName == assemblyFullName)
                    return null;
                LoadAssemblyFullName = assemblyFullName;
                System.Reflection.Assembly assembly = null;
                if (ClassesAssemblies.ContainsKey(assemblyFullName))
                    return ClassesAssemblies[assemblyFullName] as System.Reflection.Assembly;

                if (TryToLoadAssemblies.ContainsKey(assemblyFullName))
                    return null;

                TryToLoadAssemblies[assemblyFullName] = true;


                if (TypeLoader == null)
                {
#if !DeviceDotNet
                    Assembly OOAdvantech = AppDomain.Load("OOAdvantech, Version=1.0.2.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643");
                    TypeLoader = OOAdvantech.CreateInstance("OOAdvantech.TypeLoader") as ITypeLoader;
#else
                    TypeLoader = new OOAdvantech.TypeLoader();
#endif
                }
                assembly = TypeLoader.LoadAssembly(assemblyFullName);
                if (assembly != null)
                {
                    ClassesAssemblies[assemblyFullName] = assembly;
                    return assembly;
                }
#if !DeviceDotNet
                string name, version, culture, publicKeyToken;
                GetAssemblyDataFromFullName(assemblyFullName, out name, out version, out culture, out publicKeyToken);
                Microsoft.Win32.RegistryKey assemplyKey = null;
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(version))
                    assemplyKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Assembly\\" + name + "\\" + version);
                if (assemplyKey == null)
                {
                    try
                    { 
                        if (ClassesAssemblies.ContainsKey(assemblyFullName))
                            throw new System.IO.FileNotFoundException(assemblyFullName);

                        ClassesAssemblies[assemblyFullName] = null;
                        assembly = AppDomain.Load(assemblyFullName);
                    }
                    catch (Exception error)
                    { 
                        try
                        {
                            bool assemblyLoaded = false;
                            foreach (var refDll in RefDlls)
                            {
                                try
                                {
                                    var assemblyName = assemblyFullName.Split(',')[0] + ".dll";
                                    if (refDll.Contains(assemblyName))
                                    {
                                        assembly = LoadFrom(refDll);
                                        assemblyLoaded = true;
                                        break;
                                    }
                                }
                                catch (Exception loadAssemblyError)
                                {

                                }
                            }
                            if (!assemblyLoaded)
                            {

                                ErrorLoadingRefDll.Add(assemblyFullName);
                            }
                        }
                        catch (Exception inError)
                        {
                        }
                    }
                    return assembly;
                }
                bool Encrypted = bool.Parse((string)assemplyKey.GetValue("Encrypted"));
                string AssemblyDllFile = (string)assemplyKey.GetValue("AssemblyDllFile");

                try
                {
                    if (Encrypted)
                    {
                        assembly = AssemblyNativeCode.AssemblyLoader.LoadAssembly(AssemblyDllFile);
                        if (assembly == null)
                            System.Windows.Forms.MessageBox.Show("assembly==null");

                    }
                    else
                        assembly = LoadFrom(AssemblyDllFile);
                    if (assembly != null)
                        ClassesAssemblies[assemblyFullName] = assembly;
                    foreach (System.Reflection.AssemblyName reference in assembly.GetReferencedAssemblies())
                    {

                        try
                        {
                            AppDomain.Load(reference);
                        }
                        catch (System.Exception error)
                        {
                            if (System.IO.File.Exists(assembly.Location))
                            {
                                System.IO.FileInfo fileInfo = new System.IO.FileInfo(assembly.Location);
                                if (System.IO.File.Exists(fileInfo.Directory.FullName + "\\" + reference.Name))
                                    System.Reflection.Assembly.LoadFile(fileInfo.Directory.FullName + "\\" + reference.Name);
                                else if (System.IO.File.Exists(fileInfo.Directory.FullName + "\\" + reference.Name + ".dll"))
                                    System.Reflection.Assembly.LoadFile(fileInfo.Directory.FullName + "\\" + reference.Name + ".dll");
                                else if (System.IO.File.Exists(fileInfo.Directory.FullName + "\\" + reference.Name + ".exe"))
                                    System.Reflection.Assembly.LoadFile(fileInfo.Directory.FullName + "\\" + reference.Name + ".exe");



                            }
                        }
                    }
                }
                catch (Exception error)
                {


                }
#endif
                return assembly;
            }
            finally
            {
                if (TryToLoadAssemblies.ContainsKey(assemblyFullName))
                    TryToLoadAssemblies.Remove(assemblyFullName);

                LoadAssemblyFullName = null;
            }
        }




        /// <MetaDataID>{592ED6A4-4DF2-4E62-A127-C0451AD65E22}</MetaDataID>
        private static System.Collections.Generic.Dictionary<string, System.Type> Types = new System.Collections.Generic.Dictionary<string, Type>();
        /// <MetaDataID>{D76DF29A-D255-4FB9-9C25-2F1E0557FEB7}</MetaDataID>
        public static object CreateInstance(string ClassFullName, string assemblyData, params object[] ctorParams)
        {
            try
            {
                //string assemblyName = null;
                //string version = null;
                //string culture = null;
                //string publickeyToken = null;


                //GetAssemblyDataFromFullName(assemblyData, out assemblyName, out version, out culture, out publickeyToken);
                //"PublicKeyToken=2490a39b3bedae6e,Version=1.0.2.0"

                ClassFullName = ClassFullName.Trim();
                Type type = GetType(ClassFullName, assemblyData);
                Type baseType = null;

#if !DeviceDotNet
                baseType = type.BaseType;
                while (baseType != typeof(object) && baseType.FullName != "OOAdvantech.Remoting.MonoStateClass")
                    baseType = baseType.BaseType;
                if (baseType.FullName == "OOAdvantech.Remoting.MonoStateClass")
                {
                    object monoStateInsance = baseType.GetMethod("GetInstance").Invoke(null, new object[1] { type });
                    if (monoStateInsance != null)
                        return monoStateInsance;

                }

                if (ctorParams.Length > 0)
                {
                    object[] Params = new object[ctorParams.Length - 1];
                    int i = 0;
                    for (i = 0; i != ctorParams.Length - 1; i++)
                        Params[i] = ctorParams[i];
                    Type[] ParamTypes = ctorParams[i] as Type[];
                    if (ParamTypes == null)
                        throw new System.Exception("Missing types of parameters");
                    try
                    {
                        System.Reflection.ConstructorInfo constructorInfo = type.GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public, null, ParamTypes, null);
                        if (constructorInfo == null)
                            throw new System.Exception("Missing constructor with this parameters types");
                        return constructorInfo.Invoke(Params);
                    }
                    catch (System.Reflection.AmbiguousMatchException Error)
                    {
                        throw new System.Exception("The call is ambiguous");
                    }
                }
                else
                {
                    System.Reflection.ConstructorInfo constructorInfo = type.GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.DeclaredOnly, null, new System.Type[0], null);
                    if (constructorInfo == null)
                        throw new System.Exception("There isn't default constructor for '" + type.FullName + "'.");
                    return constructorInfo.Invoke(new object[0]);
                }
#else
                baseType = type.GetTypeInfo().BaseType;
                while (baseType != typeof(object) && baseType.FullName != "OOAdvantech.Remoting.MonoStateClass")
                    baseType = baseType.GetTypeInfo().BaseType;
                if (baseType.FullName == "OOAdvantech.Remoting.MonoStateClass")
                {
                    object monoStateInsance = baseType.GetRuntimeMethod("GetInstance", new Type[0]).Invoke(null, new object[1] { type });
                    if (monoStateInsance != null)
                        return monoStateInsance;

                }
                return Activator.CreateInstance(type, ctorParams);
#endif
                 
            }
            catch (System.Exception Error)
            {
#if !DeviceDotNet
                //Error prone γεμισει με message το log file τοτε παράγει exception
                if (!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
                    System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
                System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                myLog.Source = "PersistencySystem";
                System.Diagnostics.Debug.WriteLine(
                    Error.Message + Error.StackTrace);
                myLog.WriteEntry(Error.Message + Error.StackTrace, System.Diagnostics.EventLogEntryType.Error);
#endif
                throw new System.Exception("ModulePublisher can't create instance of '" + ClassFullName + "' because :" + Error.Message + ".", Error);
            }
        }


#if !DeviceDotNet
        public static bool DontUseRegistryClassesRoot;

        /// <MetaDataID>{D89274C2-5BF3-4858-8DC4-6A13A05338FF}</MetaDataID>
        static bool GetClassInfo(string classFullName, string assemblyData,
            ref string assemblyName, ref string assemblyFullName,
            ref bool globalAssemblyCache, ref string assemblyDllFile,
            ref bool encrypted)
        {
            if (assemblyData == null)
                assemblyData = "";

            if (DontUseRegistryClassesRoot)
                return false;
            classFullName = classFullName.Trim();
            int Index = classFullName.LastIndexOf(".");

            string _namespace = null;
            if (Index == -1)
                _namespace = "-";
            else
                _namespace = classFullName.Substring(0, Index);

            string className = classFullName.Substring(Index + 1, classFullName.Length - Index - 1);
            Microsoft.Win32.RegistryKey classKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Namespaces\\" + _namespace + "\\" + className);
            System.Collections.Generic.Dictionary<string, System.Version> assemblyVersions = new System.Collections.Generic.Dictionary<string, Version>();
            if (classKey != null)
            {
                foreach (string subKeyName in classKey.GetSubKeyNames())
                {
                    if (ClassVersionDataMatchAllWithAssembly(assemblyData, subKeyName))
                    {
                        classKey.Close();

                        classKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Namespaces\\" + _namespace + "\\" + className + "\\" + subKeyName);
                        string clrversion = classKey.GetValue("ImageRuntimeVersion") as string;
                        assemblyName = (string)classKey.GetValue("AssemblyName");
                        assemblyFullName = (string)classKey.GetValue("AssemblyFullName");
                        assemblyDllFile = (string)classKey.GetValue("AssemblyDllFile");
                        globalAssemblyCache = bool.Parse((string)classKey.GetValue("GlobalAssemblyCache"));
                        encrypted = bool.Parse((string)classKey.GetValue("Encrypted"));
                        classKey.Close();
                        return true;
                    }

                    if (string.IsNullOrEmpty(assemblyData) || ClassVersionDataMatchWithAssembly(assemblyData, subKeyName))
                    {
                        classKey.Close();
                        classKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Namespaces\\" + _namespace + "\\" + className + "\\" + subKeyName);
                        string name, version, culture, publicKeyToken;
                        GetAssemblyDataFromFullName(assemblyData, out name, out version, out culture, out publicKeyToken);

                        string clrVersion = classKey.GetValue("ImageRuntimeVersion") as string;
                        if (string.IsNullOrEmpty(clrVersion))
                            assemblyVersions[subKeyName] = null;
                        else if (clrVersion[0] == 'v')
                        {
                            clrVersion = clrVersion.Substring(1);
                            assemblyVersions[subKeyName] = new System.Version(clrVersion);
                        }


                        //assemblyName = (string)classKey.GetValue("AssemblyName");
                        //assemblyFullName = (string)classKey.GetValue("AssemblyFullName");
                        //assemblyDllFile = (string)classKey.GetValue("AssemblyDllFile");
                        //globalAssemblyCache = bool.Parse((string)classKey.GetValue("GlobalAssemblyCache"));
                        //encrypted = bool.Parse((string)classKey.GetValue("Encrypted"));
                        classKey.Close();
                        // return true;
                    }



                }
                string matchSubKeyName = null;
                bool majorMatch = false;
                bool minorMatch = false;
                bool buildMatch = false;
                bool revisionMatch = false;
                foreach (var entry in assemblyVersions)
                {
                    System.Version clrVersion = entry.Value;
                    if (clrVersion == null)
                    {
                        if (string.IsNullOrEmpty(matchSubKeyName))
                            matchSubKeyName = entry.Key;
                    }
                    else
                    {
                        if (System.Environment.Version.Major == clrVersion.Major)
                        {
                            if (!majorMatch)
                                matchSubKeyName = entry.Key;
                            majorMatch = true;
                            if (System.Environment.Version.Minor == clrVersion.Minor)
                            {
                                if (!minorMatch)
                                    matchSubKeyName = entry.Key;
                                minorMatch = true;
                                if (System.Environment.Version.Build == clrVersion.Build)
                                {
                                    if (!buildMatch)
                                        matchSubKeyName = entry.Key;
                                    buildMatch = true;
                                    if (System.Environment.Version.Revision == clrVersion.Revision)
                                    {
                                        if (!revisionMatch)
                                            matchSubKeyName = entry.Key;
                                        revisionMatch = true;
                                    }
                                }
                            }
                        }
                        else if (matchSubKeyName == null)
                            matchSubKeyName = entry.Key;
                    }

                }
                if (matchSubKeyName != null)
                {
                    classKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Namespaces\\" + _namespace + "\\" + className + "\\" + matchSubKeyName);
                    assemblyName = (string)classKey.GetValue("AssemblyName");
                    assemblyFullName = (string)classKey.GetValue("AssemblyFullName");
                    assemblyDllFile = (string)classKey.GetValue("AssemblyDllFile");
                    globalAssemblyCache = bool.Parse((string)classKey.GetValue("GlobalAssemblyCache"));
                    encrypted = bool.Parse((string)classKey.GetValue("Encrypted"));
                    classKey.Close();
                    return true;
                }


                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(assemblyData))
                    return false;
                else
                {
                    System.Reflection.Assembly assembly = LoadAssembly(assemblyData);
                    if (assembly == null)
                        return false;
                    else
                    {
                        assemblyName = assembly.GetName().Name;
                        assemblyFullName = assembly.FullName;
                        encrypted = false;
                        if (!assembly.GlobalAssemblyCache)
                            assemblyDllFile = assembly.CodeBase;
                        if (!string.IsNullOrEmpty(assemblyDllFile))
                            assemblyDllFile = assemblyDllFile.Replace(@"file:///", "");
                        globalAssemblyCache = assembly.GlobalAssemblyCache;
                        return true;
                    }
                }
            }
        }

        static bool IsAssemblyUpdated(string moduleFile, string version)
        {

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(moduleFile);
            DateTime lastWriteTime = DateTime.Now;
            string assemplyName = fileInfo.Name.Replace(fileInfo.Extension, "");
            Microsoft.Win32.RegistryKey registryKey = null;
            try
            {
                lastWriteTime = fileInfo.LastWriteTime;
                registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Assembly\\" + assemplyName + "\\" + System.Diagnostics.FileVersionInfo.GetVersionInfo(moduleFile).FileVersion + "\\");
                if (registryKey != null)
                {
                    System.DateTime fileDateTime;
                    fileDateTime = System.DateTime.Parse(registryKey.GetValue("Date") as string);
                    registryKey.Close();
                    if (fileDateTime.ToString() == fileInfo.LastWriteTime.ToString())
                        return false;
                }
            }
            catch (System.Exception error)
            {
            }


            return true;
        }

        static void AssemblyUpdated(System.Reflection.Assembly assembly, string moduleFile, bool encrypted)
        {
            //System.Windows.Forms.MessageBox.Show("AssemblyUpdated");

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(moduleFile);
            Microsoft.Win32.RegistryKey assemblyKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Assembly\\" + assembly.GetName().Name + "\\" + assembly.GetName().Version + "\\", true);
            if (assemblyKey == null)
                assemblyKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("Assembly\\" + assembly.GetName().Name + "\\" + assembly.GetName().Version);
            assemblyKey.SetValue("AssemblyFullName", assembly.GetName().FullName);
            assemblyKey.SetValue("AssemblyDllFile", moduleFile);
            assemblyKey.SetValue("Encrypted", encrypted.ToString());
            assemblyKey.SetValue("Date", fileInfo.LastWriteTime.ToString());
            assemblyKey.SetValue("ImageRuntimeVersion", assembly.ImageRuntimeVersion);

            assemblyKey.Close();

        }

        /// <MetaDataID>{90FC54F2-8B54-427E-A205-E8795D8FCC23}</MetaDataID>
        public static void EncryptandPublish(string moduleFile, string version, string outputFile)
        {

            if (!IsAssemblyUpdated(moduleFile, version))
                return;

            if (System.Reflection.AssemblyName.GetAssemblyName(moduleFile).ProcessorArchitecture != ProcessorArchitecture.MSIL)
                return;

            AssemblyNativeCode.AssemblyLoader.MakeNadive(moduleFile);
            Publish(moduleFile, version, outputFile, true, false);
        }



        /// <MetaDataID>{2B0862DE-A314-4540-8243-FDB4B50B1227}</MetaDataID>
        public static void Publish(string ModuleFile, string Version, string OutputFile, bool setup)
        {

            // System.Windows.Forms.MessageBox.Show("Stop");

            Publish(ModuleFile, Version, OutputFile, false, setup);
        }
        /// <MetaDataID>{0075BACB-9E7B-4ACE-9468-A04886C08C6B}</MetaDataID>
        public static void Remove(string moduleFile, string version, bool encrypted)
        {


            //System.Windows.Forms.MessageBox.Show("Stop");
            System.Reflection.Assembly assembly = null;
            if (encrypted)
                assembly = AssemblyNativeCode.AssemblyLoader.LoadAssembly(moduleFile);
            else
                assembly = LoadFrom(moduleFile);

            if (assembly != null && ClassesAssemblies.ContainsKey(assembly.FullName))
                ClassesAssemblies.Remove(assembly.FullName);
            string KeyPath = null;
            System.Type[] AssemblyTypes = assembly.GetTypes();
            foreach (System.Type type in AssemblyTypes)
            {
                if ((!type.IsAbstract) && type.IsClass)
                {
                    KeyPath = "Namespaces\\" + type.Namespace + "\\" + type.Name;
                    try
                    {
                        Microsoft.Win32.Registry.ClassesRoot.DeleteSubKey(KeyPath);
                        KeyPath = "Namespaces\\" + type.Namespace;
                        Microsoft.Win32.RegistryKey NameSpaceKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(KeyPath);
                        if (NameSpaceKey.GetSubKeyNames().Length == 0)
                        {
                            NameSpaceKey.Close();
                            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKey(KeyPath);
                        }
                        else
                            NameSpaceKey.Close();
                    }
                    catch (System.Exception)
                    {
                    }
                }
            }
            try
            {
                KeyPath = "Assembly\\";
                KeyPath += assembly.GetName().Name + "\\";
                KeyPath += assembly.GetName().Version;
                Microsoft.Win32.Registry.ClassesRoot.DeleteSubKey(KeyPath);

            }
            catch (Exception error)
            {
            }
            if (!encrypted)
                RemoveFromGAC(assembly.FullName);
        }
        /// <MetaDataID>{ACA2C2A9-6DA4-4E7A-ABB3-A7EE22BA87EB}</MetaDataID>
        public static void Publish(string moduleFile, string Version, string outputFile, bool Encrypted, bool setup)
        {
            //System.Windows.Forms.MessageBox.Show("Stop");
            if (System.IO.File.Exists(moduleFile) && !setup)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(moduleFile);
                if (!System.IO.Directory.Exists(fileInfo.Directory.FullName + @"\PerformanceTest"))
                    System.IO.Directory.CreateDirectory(fileInfo.Directory.FullName + @"\PerformanceTest");
                if (System.IO.File.Exists(fileInfo.Directory.FullName + @"\PerformanceTest\" + fileInfo.Name))
                    System.IO.File.Delete(fileInfo.Directory.FullName + @"\PerformanceTest\" + fileInfo.Name);
                System.IO.File.Copy(moduleFile, fileInfo.Directory.FullName + @"\PerformanceTest\" + fileInfo.Name, true);

            }


            moduleFile = moduleFile.Trim();

            if (!IsAssemblyUpdated(moduleFile, Version))
            {
                if (!Encrypted)
                    AddToGAC(moduleFile);
                return;
            }
            System.Reflection.Assembly assembly = null;
            if (!Encrypted)
            {
                if (System.Reflection.AssemblyName.GetAssemblyName(moduleFile).ProcessorArchitecture != ProcessorArchitecture.MSIL)
                    return;
                RemoveFromGAC(moduleFile);
                assembly = LoadFrom(moduleFile);
                AddToGAC(moduleFile);
            }
            else
                assembly = AssemblyNativeCode.AssemblyLoader.LoadAssembly(moduleFile);
            System.Reflection.AssemblyName mscorlib = null;
#if !DeviceDotNet
            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                if (assemblyName.Name == "mscorlib")
                {
                    mscorlib = assemblyName;
                    ulong token = BitConverter.ToUInt64(mscorlib.GetPublicKeyToken(), 0);
                    if (token == 0xac22333d05b89d96)
                        return;
                }
            }
#endif


            if (!ErrorCheck(assembly, outputFile))
                AssemblyUpdated(assembly, moduleFile, Encrypted);


            foreach (System.Type type in assembly.GetTypes())
            {
                if (type.Name.IndexOf("<PrivateImplementationDetails>") != 0 &&
                    type.Name.IndexOf("<>f__AnonymousType") != 0 &&
                    ((!type.IsAbstract) && type.IsClass || (type.IsValueType && !type.IsPrimitive)))//&&mType.IsPublic)
                {
                    if (type.Name.IndexOf("<PrivateImplementationDetails>") == 0)
                        continue;
                    string keyPath = "Namespaces\\" + type.Namespace + "\\" + type.Name + "\\" + assembly.GetName().FullName;
                    Microsoft.Win32.RegistryKey classKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(keyPath, true);
                    if (classKey == null)
                        classKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(keyPath);
                    if (classKey != null)
                    {
                        classKey.SetValue("AssemblyDllFile", moduleFile);
                        classKey.SetValue("AssemblyName", assembly.GetName().Name);
                        classKey.SetValue("AssemblyFullName", assembly.GetName().FullName);
#if !DeviceDotNet
                        classKey.SetValue("GlobalAssemblyCache", assembly.GlobalAssemblyCache.ToString());
#else
                        classKey.SetValue("GlobalAssemblyCache", false.ToString());
#endif
                        if (Encrypted)
                            classKey.SetValue("Encrypted", true.ToString());
                        else
                            classKey.SetValue("Encrypted", false.ToString());
                        classKey.SetValue("ImageRuntimeVersion", assembly.ImageRuntimeVersion);
                        classKey.Close();
                    }
                }

            }
        }

        private static bool ErrorCheck(System.Reflection.Assembly assembly, string outputFile)
        {
            bool hasError = false;
            try
            {
                if (outputFile != null && outputFile.Trim().Length > 0)
                {
                    foreach (System.Attribute attribute in assembly.GetCustomAttributes(false))
                    {
                        if (attribute.GetType().FullName == "OOAdvantech.MetaDataRepository.BuildAssemblyMetadata")
                        {
                            System.Xml.XmlDocument xmlDocument = null;

                            try
                            {
                                object component = null;
                                //#if Net4
                                //                                component = CreateInstance("OOAdvantech.DotNetMetaDataRepository.Assembly", "DotNetMetaDataRepository, Version=4.0.0.0, Culture=neutral, PublicKeyToken=00a88b51a86dbd3c", assembly, new Type[1] { typeof(System.Reflection.Assembly) });
                                //#else
                                component = CreateInstance("OOAdvantech.DotNetMetaDataRepository.Assembly", "DotNetMetaDataRepository, Version=1.0.2.0, Culture=neutral, PublicKeyToken=11a79ce55c18c4e7", assembly, new Type[1] { typeof(System.Reflection.Assembly) });
                                //#endif

                                Type type = component.GetType().BaseType.Assembly.GetType("OOAdvantech.MetaDataRepository.MetaObject");
                                System.Reflection.MethodInfo method = type.GetMethod("ErrorCheck");
                                Type errorsCollectionType = typeof(System.Collections.Generic.List<>).MakeGenericType(new Type[1] { GetType("OOAdvantech.MetaDataRepository.MetaObject", "").GetNestedType("MetaDataError") });
                                Object[] args = new Object[1] { Activator.CreateInstance(errorsCollectionType) };
                                hasError = (bool)method.Invoke(component, args);

                                System.Collections.IList errors = args[0] as System.Collections.IList;

                                //object embeddedOQLParser = CreateInstance("OOAdvantech.CSharpOQLParser.EmbeddedOQLParser", "");
                                //method = embeddedOQLParser.GetType().GetMethod("ParseAssembly");

                                //args = new Object[3] { new System.Collections.ArrayList(), assembly, new System.Collections.ArrayList() };

                                //System.Collections.ArrayList files = args[0] as System.Collections.ArrayList;
                                //try
                                //{
                                //    xmlDocument = new System.Xml.XmlDocument();
                                //    xmlDocument.Load(outputFile);
                                //    foreach (System.Xml.XmlElement fileElement in xmlDocument.DocumentElement.ChildNodes)
                                //        files.Add(fileElement.GetAttribute("LocalPath"));
                                //}
                                //catch (System.Exception error)
                                //{

                                //}


                                //if (files.Count > 0)
                                //    hasError |= (bool)method.Invoke(embeddedOQLParser, args);

                                //  System.Collections.ArrayList oqlErrors = args[2] as System.Collections.ArrayList;




                                xmlDocument = new System.Xml.XmlDocument();
                                System.Xml.XmlElement errorsElement = xmlDocument.CreateElement("Errors");
                                xmlDocument.AppendChild(errorsElement);
                                System.Collections.IList UIerrors = null;
                                try
                                {

                                    method = GetType("ConnectableControls.AssemblyManager", "").GetMethod("ErrorCheck");
                                    args = new Object[2] { component, Activator.CreateInstance(errorsCollectionType) };
                                    hasError = (bool)method.Invoke(null, args);
                                    UIerrors = args[1] as System.Collections.IList;


                                    //UIerrors = new System.Collections.ArrayList();
                                }
                                catch (System.Exception error)
                                {
                                    UIerrors = new System.Collections.ArrayList();
                                    //UIerrors.Add("UI Error: " + error.Message + " \n" + error.StackTrace);


                                }
                                foreach (object error in UIerrors)
                                {
                                    hasError = true;
                                    System.Xml.XmlElement errorElement = xmlDocument.CreateElement("Error");
                                    errorsElement.AppendChild(errorElement);
                                    errorElement.InnerText = error.GetType().GetField("ErrorMessage").GetValue(error) as string; ;
                                    errorElement.SetAttribute("FileName", error.GetType().GetField("ErrorPath").GetValue(error) as string);
                                    errorElement.SetAttribute("LineNumber", ((int)1).ToString());
                                }



                                foreach (object error in errors)
                                {
                                    hasError = true;
                                    System.Xml.XmlElement errorElement = xmlDocument.CreateElement("Error");
                                    errorsElement.AppendChild(errorElement);
                                    errorElement.InnerText = error.GetType().GetField("ErrorMessage").GetValue(error) as string; ;
                                    errorElement.SetAttribute("FileName", error.GetType().GetField("ErrorPath").GetValue(error) as string);
                                    errorElement.SetAttribute("LineNumber", ((int)1).ToString());
                                }

                                //foreach (object OQLerror in oqlErrors)
                                //{

                                //    System.Xml.XmlElement errorElement = xmlDocument.CreateElement("Error");
                                //    errorsElement.AppendChild(errorElement);
                                //    errorElement.InnerText = OQLerror.GetType().GetField("ErrorMessage").GetValue(OQLerror) as string;
                                //    errorElement.SetAttribute("FileName", OQLerror.GetType().GetField("File").GetValue(OQLerror) as string);
                                //    errorElement.SetAttribute("LineNumber", ((int)OQLerror.GetType().GetField("Line").GetValue(OQLerror)).ToString());
                                //}
                            }
                            catch (System.Exception Error)
                            {
                                while (Error.InnerException != null)
                                    Error = Error.InnerException;
                                xmlDocument = new System.Xml.XmlDocument();
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
                                hasError = true;

                            }

                            xmlDocument.Save(outputFile);

                            string KeyPath = "Assembly\\";
                            KeyPath += assembly.GetName().Name + "\\";
                            KeyPath += assembly.GetName().Version;
                            //Microsoft.Win32.RegistryKey ClassesRootKey = Microsoft.Win32.Registry.ClassesRoot;
                            //Microsoft.Win32.RegistryKey ClassRegistryKey = ClassesRootKey.OpenSubKey(KeyPath, true);
                            //if (ClassRegistryKey != null)
                            //{
                            //    if (!hasError)
                            //    {
                            //        DateTime now = DateTime.Now;
                            //        now += new TimeSpan(0, 0, 1);
                            //        ClassRegistryKey.SetValue("Date", now.ToString());
                            //    }
                            //    ClassRegistryKey.Close();
                            //}




                            //type.InvokeMember("ErrorCheck",System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.InvokeMethod,null,component,new Object[1] {""});
                            int tt = 0;

                        }
                    }
                }
            }
            catch (System.Exception error)
            {
                int erteee = 0;

            }
            return hasError;

        }
#endif
        public static void Init()
        {

        }
    }


    /// <MetaDataID>{7dfe83da-2e0f-45dd-ac26-845af811299b}</MetaDataID>
    [System.AttributeUsage(System.AttributeTargets.Assembly)]
    public class AddGAC : System.Attribute
    {
        public AddGAC()
        {
        }
    }



}
