using System;
using System.Reflection;

namespace OOAdvantech.Synchronization
{
    /// <MetaDataID>{663f1f8c-a651-4f22-af15-fad7200513fe}</MetaDataID>
    public struct LockCookie
    {
        //System.wThreading.LockCookies
        /// <MetaDataID>{29a0be3e-56dd-43af-892e-d8943b3200f5}</MetaDataID>
        internal InternalLockCookie SystemLockCookie;
    }


    /// <summary></summary>
    /// <MetaDataID>{255f0fbf-3c03-43d7-aa67-fb990a1bf66d}</MetaDataID>
    public class ReaderWriterLock
    {


        /// <MetaDataID>{32f6f2f4-a623-49ff-a6d2-887a23ab163e}</MetaDataID>
        public ReaderWriterLock()
        {
            SystemReaderWriterLock = new InternalReaderWriterLock(false);
        }
        /// <MetaDataID>{b99e7ec4-ced7-4a19-ba84-cd2dbaa2d20d}</MetaDataID>
        public ReaderWriterLock(bool debugMode)
        {
            SystemReaderWriterLock = new InternalReaderWriterLock(debugMode);
        }


        //θα πρέπει να αντικαθασταθεί από την ReaderWriterLockSlim στο .net 3.5
        //System.Threading.ReaderWriterLock SystemReaderWriterLock=new System.Threading.ReaderWriterLock();
        /// <MetaDataID>{6c729959-231e-431a-a529-ee2d8bf4148f}</MetaDataID>
        InternalReaderWriterLock SystemReaderWriterLock;
        /// <MetaDataID>{281aac70-c473-4092-9369-0fa040c1b5af}</MetaDataID>
        public void AcquireReaderLock(int millisecondsTimeout)
        {

            SystemReaderWriterLock.AcquireReaderLock(millisecondsTimeout);
       }
        /// <MetaDataID>{5140431a-8962-4cd4-bc9c-56a729f40f9b}</MetaDataID>
        public void AcquireReaderLock(System.TimeSpan timeout)
        {

            SystemReaderWriterLock.AcquireReaderLock(timeout.TotalMilliseconds);
        }
        /// <MetaDataID>{5adae6b5-ee71-4523-a2f4-5e2083a9ae71}</MetaDataID>
        public void AcquireWriterLock(int millisecondsTimeout)
        {

            SystemReaderWriterLock.AcquireWriterLock(millisecondsTimeout);

        }
        /// <MetaDataID>{9779c006-849b-4831-9c8d-77b70fa52c50}</MetaDataID>
        public void AcquireWriterLock(System.TimeSpan timeout)
        {


            SystemReaderWriterLock.AcquireWriterLock(timeout.TotalMilliseconds);

        }
        /// <MetaDataID>{277a83ce-eb7c-4d72-be32-632eaf6ba355}</MetaDataID>
        public LockCookie UpgradeToWriterLock(int millisecondsTimeout)
        {

            LockCookie lockCookie;
            lockCookie.SystemLockCookie = SystemReaderWriterLock.UpgradeToWriterLock(millisecondsTimeout);
            return lockCookie;

        }


        /// <MetaDataID>{5fa52236-d896-4221-b127-ef5faa32b649}</MetaDataID>
        public LockCookie UpgradeToWriterLock(System.TimeSpan timeout)
        {

            LockCookie lockCookie;
            lockCookie.SystemLockCookie = SystemReaderWriterLock.UpgradeToWriterLock(timeout.TotalMilliseconds);
            return lockCookie;
        }

        /// <MetaDataID>{31fb357a-273d-4d2d-8232-2c025d94c59b}</MetaDataID>
        public void ReleaseReaderLock()
        {
            //TODO τι γίνεται αν καλεστεί πάνω από δύο φορές για ένα readlock
            SystemReaderWriterLock.ReleaseReaderLock();

        }
        /// <MetaDataID>{682c27a6-17ad-4026-8a0d-ab235cb97a41}</MetaDataID>
        public void ReleaseWriterLock()
        {
            SystemReaderWriterLock.ReleaseWriterLock();
        }
        //public bool AnyWritersSince(int seqNum)
        //{
        //    return SystemReaderWriterLock.AnyWritersSince( seqNum);
        //}
        /// <MetaDataID>{b3ef014a-6d15-4694-bed3-352ee28cc2ce}</MetaDataID>
        public void DowngradeFromWriterLock(ref LockCookie lockCookie)
        {
            SystemReaderWriterLock.DowngradeFromWriterLock(ref lockCookie.SystemLockCookie);

        }
        /// <MetaDataID>{2bdbae6e-be0a-4090-8c69-6b80b1d01d35}</MetaDataID>
        public LockCookie ReleaseLock()
        {
            LockCookie lockCookie;
            lockCookie.SystemLockCookie = SystemReaderWriterLock.ReleaseLock();
            return lockCookie;
        }
    }






    /// <MetaDataID>{e502381f-091b-423b-96df-9b9fdc89e467}</MetaDataID>
    public class InternalReaderWriterLock
    {
        class Lock
        {
            int _RefCount;
            public int RefCount
            {
                get
                {
                    return _RefCount;
                }
                set
                {

                    if (DeadLockDebug && InternalReaderWriterLock.AllowDeadLockDebug && value > _RefCount)
                    {
                        if (Calls == null)
                            Calls = new System.Collections.Generic.Stack<CallData>();
                        CallData callData = new CallData();
#if !DeviceDotNet 
                        callData.StackTrace = new System.Diagnostics.StackTrace(1, true);
#endif
                        callData.ThreadID = ThreadID;
                        Calls.Push(callData);
                    }
                    if (Calls != null && Calls.Count > 0 && DeadLockDebug && InternalReaderWriterLock.AllowDeadLockDebug && value < _RefCount)
                        Calls.Pop();

                    _RefCount = value;


                }
            }
            public int ThreadID;
            #if DEBUG
            public int NativeThread;
            #endif


            public bool Passive;
            public System.Collections.Generic.Stack<CallData> Calls;
            public readonly bool DeadLockDebug;
            public static int GetNativeThreadId(System.Threading.Thread thread)
            {
                var f = typeof(System.Threading.Thread).GetField("DONT_USE_InternalThread",
                    BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);

                var pInternalThread = (IntPtr)f.GetValue(thread);
                var nativeId = System.Runtime.InteropServices.Marshal.ReadInt32(pInternalThread, (IntPtr.Size == 8) ? 0x022C : 0x0160); // found by analyzing the memory
                return nativeId;
            }
            public Lock(bool deadLockDebug)
            {
                DeadLockDebug = deadLockDebug;
                ThreadID = GetNativeThreadId(System.Threading.Thread.CurrentThread);
                
#if DEBUG&&!DeviceDotNet 
                NativeThread=AppDomain.GetCurrentThreadId();

#endif
#if DEBUG&&DeviceDotNet 
                NativeThread=0;
#endif




                if (DeadLockDebug && InternalReaderWriterLock.AllowDeadLockDebug)
                    Calls = new System.Collections.Generic.Stack<CallData>();
                else
                    Calls = null;
                _RefCount = 0;
                Passive = false;
            }
        }
        
        public struct CallData
        {
#if !DeviceDotNet 
            public System.Diagnostics.StackTrace StackTrace;
#endif
            public int ThreadID;
        }

        /// <MetaDataID>{047c48aa-e46b-4709-968a-7f29d9c412ce}</MetaDataID>
        Lock LastAccessedReadLock;

        /// <MetaDataID>{85df5b3f-a9fe-4c0f-a4be-d15a1e75c691}</MetaDataID>
        Lock WriteLock;
        /// <MetaDataID>{d36b2018-e84e-4d9d-9900-855f9bdd5c10}</MetaDataID>
        bool DeadLockDebug;
        /// <MetaDataID>{2a0e9742-4441-4e64-81c1-48a9cb7ad635}</MetaDataID>
        public InternalReaderWriterLock(bool deadLockDebug)
        {
            DeadLockDebug = deadLockDebug;
            LastAccessedReadLock= new Lock(deadLockDebug);
            LastAccessedReadLock.RefCount = 0;
            LastAccessedReadLock.ThreadID = -1;
            LastAccessedReadLock.Passive = false;
            WriteLock = new Lock(deadLockDebug);
            WriteLock.RefCount = 0;
            WriteLock.ThreadID = -1;
            WriteLock.Passive = false;
            LastAccessedReadLock = WriteLock;

        }


        /// <MetaDataID>{2857cf99-c2b7-4103-b3f1-fac293cd4424}</MetaDataID>
        System.Collections.Generic.Dictionary<int, Lock> ReadLocks;

        /// <MetaDataID>{5211af70-23da-4704-aec3-e78c36283273}</MetaDataID>
        public void AcquireReaderLock(double millisecondsTimeout)
        {
            //TODO Δεν δουλεύει καλά ο timeout xρονος
            int whileCount = 0;
            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            bool wait = false;
            do
            {
                wait = false;
                CallData lastCallDebugData;
#if !DeviceDotNet 
                lastCallDebugData.StackTrace = null;
#endif

                lock (this)
                {
                    if (WriteLock.ThreadID == -1 || WriteLock.ThreadID == threadID)
                    {
                        if (LastAccessedReadLock.ThreadID == threadID)
                        {
                            LastAccessedReadLock.RefCount++;
                        }
                        else if (LastAccessedReadLock.ThreadID == -1)
                        {
                            LastAccessedReadLock.ThreadID = threadID;
#if DEBUG&&!DeviceDotNet 
                           // LastAccessedReadLock.NativeThread = AppDomain.GetCurrentThreadId();
#endif
                            LastAccessedReadLock.RefCount++;

                        }
                        else
                        {
                            if (ReadLocks == null)
                            {
                                ReadLocks = new System.Collections.Generic.Dictionary<int, Lock>();
                                ReadLocks.Add(LastAccessedReadLock.ThreadID, LastAccessedReadLock);
                                LastAccessedReadLock = new Lock(DeadLockDebug);
                                LastAccessedReadLock.ThreadID = threadID;
#if DEBUG&&!DeviceDotNet 
                                LastAccessedReadLock.NativeThread = AppDomain.GetCurrentThreadId();
#endif
                                LastAccessedReadLock.Passive = false;
                                LastAccessedReadLock.RefCount++;
                                ReadLocks.Add(threadID, LastAccessedReadLock);
                            }
                            else if (!ReadLocks.ContainsKey(threadID))
                            {
                                if (LastAccessedReadLock.ThreadID != -1 && !ReadLocks.ContainsKey(LastAccessedReadLock.ThreadID))
                                    ReadLocks[LastAccessedReadLock.ThreadID] = LastAccessedReadLock;

                                LastAccessedReadLock = new Lock(DeadLockDebug);
                                LastAccessedReadLock.ThreadID = threadID;
#if DEBUG&&!DeviceDotNet 
                                LastAccessedReadLock.NativeThread = AppDomain.GetCurrentThreadId();
#endif

                                LastAccessedReadLock.Passive = false;
                                LastAccessedReadLock.RefCount++;
                                ReadLocks.Add(threadID, LastAccessedReadLock);
                            }
                            else
                            {
                                LastAccessedReadLock = ReadLocks[threadID];
                                LastAccessedReadLock.RefCount++;
                                //ReadLocks[threadID] = LastAccessedReadLock;
                                //if (ReadLocks[threadID].RefCount != LastAccessedReadLock.RefCount)
                                //{

                                //}
                            }
                        }
                    }
                    else
                    {
                        wait = true;
                    
                        if (WriteLock.DeadLockDebug && AllowDeadLockDebug && WriteLock.Calls != null && WriteLock.Calls.Count > 0)
                            lastCallDebugData = WriteLock.Calls.Peek();
                    }
                }
                if (wait)
                {
                    if (whileCount > 10)
                    {

                    }
                    whileCount++;
                    System.Threading.Thread.Sleep(1);
                    millisecondsTimeout--;
                    if (millisecondsTimeout < 0)
                    {
#if !DeviceDotNet 
                        if (lastCallDebugData.StackTrace != null)
                        {

                            int i = 0;
                            System.Diagnostics.StackFrame stackFrame = null;
                            System.Reflection.MethodBase method = null;
                            do
                            {
                                stackFrame = lastCallDebugData.StackTrace.GetFrame(i++);
                                method = stackFrame.GetMethod();
                            }
                            while (method.DeclaringType.Namespace == typeof(InternalReaderWriterLock).Namespace);

                            string fileName = stackFrame.GetFileName();
                            int fileLineNumber = stackFrame.GetFileLineNumber();
                            string errorMessage = "DeadLock time out. Lock code at " + method.DeclaringType.FullName + "." + method.Name + " File :" + fileName + " Line :" + fileLineNumber.ToString();
                            System.Diagnostics.Debug.WriteLine(errorMessage);

                            //TODO γεμισει με message το log file τοτε παράγει exception
                            if (!System.Diagnostics.EventLog.SourceExists("Syncronization", "."))
                                System.Diagnostics.EventLog.CreateEventSource("Syncronization", "OOAdvance");

                            System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                            myLog.Source = "Syncronization";
                            System.Diagnostics.Debug.WriteLine(errorMessage);
                            throw new System.Exception(errorMessage);
                        }
                        else
#endif
                            throw new System.Exception("Time out expired");
                    }
                }

            } while (wait);

        }
        /// <MetaDataID>{4817c798-e51f-4c47-8e7b-7eea384f6d11}</MetaDataID>
        public void AcquireWriterLock(double millisecondsTimeout)
        {
            int whileCount = 0;
           //TODO Δεν δουλεύει καλά ο timeout xρονος
            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            bool wait = false;
            do
            {
                CallData lastCallDebugData;
#if !DeviceDotNet 
                lastCallDebugData.StackTrace = null;
#endif
                lock (this)
                {
                    wait = false;

                    if ((WriteLock.ThreadID == -1 || WriteLock.ThreadID == threadID) &&
                        (LastAccessedReadLock.ThreadID == -1 ||
                        (LastAccessedReadLock.ThreadID == threadID && (ReadLocks == null || ReadLocks.Count <= 1))))
                    {
                        if (WriteLock.ThreadID == threadID)
                            WriteLock.RefCount++;
                        else
                        {
                            WriteLock.ThreadID = threadID;
#if DEBUG&&!DeviceDotNet 
                            WriteLock.NativeThread = AppDomain.GetCurrentThreadId();
#endif

                            WriteLock.RefCount++;
                        }

                    }
                    else
                    {
                        if ((WriteLock.ThreadID != -1 && WriteLock.ThreadID != threadID))
                        {
                            wait = true;
                            if (WriteLock.DeadLockDebug && AllowDeadLockDebug && WriteLock.Calls != null && WriteLock.Calls.Count > 0)
                                lastCallDebugData = WriteLock.Calls.Peek();

                        }
                        else if (ReadLocks != null && ReadLocks.Count > 0)
                        {
                            foreach (Lock readLock in ReadLocks.Values)
                            {
                                if (readLock.ThreadID != threadID && !readLock.Passive)
                                {
                                    wait = true;
                                    if (readLock.DeadLockDebug && AllowDeadLockDebug && readLock.Calls != null && readLock.Calls.Count > 0)
                                        lastCallDebugData = readLock.Calls.Peek();

                                    break;
                                }
                            }
                            if (!wait)
                            {

                                if (WriteLock.ThreadID == threadID)
                                    WriteLock.RefCount++;
                                else
                                {
                                    WriteLock.ThreadID = threadID;
#if DEBUG&&!DeviceDotNet 
                                    WriteLock.NativeThread = AppDomain.GetCurrentThreadId();
#endif

                                    WriteLock.RefCount++;
                                }

                            }

                        }
                        else
                        {
                            wait = true;
                            if (WriteLock.DeadLockDebug && AllowDeadLockDebug && WriteLock.Calls != null && WriteLock.Calls.Count > 0)
                                lastCallDebugData = WriteLock.Calls.Peek();
                        }
                    }
              
                }
                if (wait)
                {
                    if(whileCount>10)
                    {

                    }
                    whileCount++;
                    System.Threading.Thread.Sleep(1);
                    millisecondsTimeout--;
                    if (millisecondsTimeout < 0)
                    {
#if !DeviceDotNet                         
                        if (lastCallDebugData.StackTrace != null)
                        {
                            int i = 0;
                            System.Diagnostics.StackFrame stackFrame =null;
                            System.Reflection.MethodBase method=null;
                            do
                            {
                                stackFrame = lastCallDebugData.StackTrace.GetFrame(i++);
                                method = stackFrame.GetMethod();
                            }
                            while (method.DeclaringType.Namespace == typeof(InternalReaderWriterLock).Namespace);
                            string fileName = stackFrame.GetFileName();
                            int fileLineNumber = stackFrame.GetFileLineNumber();
                            string errorMessage = "DeadLock time out. Lock code at " + method.DeclaringType.FullName + "." + method.Name + " File :" + fileName + " Line :" + fileLineNumber.ToString();
                            System.Diagnostics.Debug.WriteLine(errorMessage);

                            //TODO γεμισει με message το log file τοτε παράγει exception
                            if (!System.Diagnostics.EventLog.SourceExists("Syncronization", "."))
                                System.Diagnostics.EventLog.CreateEventSource("Syncronization", "OOAdvance");

                            System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                            myLog.Source = "Syncronization";
                            System.Diagnostics.Debug.WriteLine(errorMessage);
                            throw new System.Exception(errorMessage);
                        }
                        else
#endif
                            throw new System.Exception("Read write lock. Time out expired");
                    }
                }

            } while (wait);



        }
        /// <MetaDataID>{64010815-8719-428b-b641-42900390cc10}</MetaDataID>
        public InternalLockCookie UpgradeToWriterLock(double millisecondsTimeout)
        {
            InternalLockCookie lockCookie;
            lock (this)
            {

                int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

                lockCookie.ThreadID = threadID;
                lockCookie.UpgradeReadLock = false;

                if (LastAccessedReadLock.ThreadID == threadID)
                {
                    LastAccessedReadLock.Passive = true;
                    lockCookie.UpgradeReadLock = true;
                    if (ReadLocks != null && ReadLocks.Count > 0)
                        ReadLocks[threadID] = LastAccessedReadLock;
                }
                else if (ReadLocks != null && ReadLocks.ContainsKey(threadID))
                {
                    lockCookie.UpgradeReadLock = true;
                    Lock readLock = ReadLocks[threadID];
                    readLock.Passive = true;
                    ReadLocks[threadID] = readLock;
                }
            }

            AcquireWriterLock(millisecondsTimeout);

            return lockCookie;
        }
        /// <MetaDataID>{c6e7d9bb-4203-4ea2-a54c-b6bdf7457cd2}</MetaDataID>
        public void DowngradeFromWriterLock(ref InternalLockCookie lockCookie)
        {
            lock (this)
            {
                if (lockCookie.UpgradeReadLock)
                {
                    if (LastAccessedReadLock.ThreadID == lockCookie.ThreadID)
                        LastAccessedReadLock.Passive = false;
                    if (ReadLocks != null && ReadLocks.ContainsKey(lockCookie.ThreadID))
                    {
                        Lock readLock = ReadLocks[lockCookie.ThreadID];
                        readLock.Passive = false;

                        ReadLocks[lockCookie.ThreadID] = readLock;
                    }
                }
            }
            ReleaseWriterLock();

        }
        /// <MetaDataID>{6ce25492-d077-44e2-a755-8cf289be7f56}</MetaDataID>
        public void ReleaseReaderLock()
        {
            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            lock (this)
            {
                if (LastAccessedReadLock.ThreadID == threadID)
                {
                    LastAccessedReadLock.RefCount--;
                    if (LastAccessedReadLock.RefCount == 0)
                    {
                        LastAccessedReadLock.ThreadID = -1;
                        if (ReadLocks != null && ReadLocks.ContainsKey(threadID))
                            ReadLocks.Remove(threadID);
                        if (ReadLocks != null)
                        {
                            foreach (Lock readLock in ReadLocks.Values)
                            {
                                LastAccessedReadLock = readLock;
                                break;
                            }
                        }

                    }
                }
                else if (ReadLocks != null && ReadLocks.ContainsKey(threadID))
                {
                    Lock readLock = ReadLocks[threadID];
                    readLock.RefCount--;
                    if (readLock.RefCount == 0)
                    {
                        readLock.ThreadID = -1;
                        ReadLocks.Remove(threadID);
                    }
                    else
                    {
                        //if (ReadLocks[threadID].RefCount != readLock.RefCount)
                        //{

                        //}
                    }
                }

            }

        }
        /// <MetaDataID>{2b7e5f2f-1f9a-4434-85ee-c2d9212fbd69}</MetaDataID>
        public void ReleaseWriterLock()
        {
            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            lock (this)
            {
                if (WriteLock.ThreadID == threadID)
                {
                    WriteLock.RefCount--;
                    if (WriteLock.RefCount == 0)
                        WriteLock.ThreadID = -1;

                }
            }


        }
        /// <MetaDataID>{6508a4b8-bc93-4905-9097-d238d6496813}</MetaDataID>
        public InternalLockCookie ReleaseLock()
        {
            InternalLockCookie lockCookie;
            lockCookie.ThreadID = 0;
            lockCookie.UpgradeReadLock = false;

            return lockCookie;
        }


        enum DeadLockDebugState
        {
            Uninitialized,
            DeadLockDebug,
            NoDeadLockDebug

        }
        /// <MetaDataID>{b2f4b496-b997-40ae-af8f-6bd0b6b634ed}</MetaDataID>
        static DeadLockDebugState _AllowDeadLockDebug = DeadLockDebugState.Uninitialized;
        /// <MetaDataID>{d177ee1b-9fdf-4251-84de-71b0f37de93e}</MetaDataID>
        static string StaticLock = "";
        /// <summary>
        /// DeadLockDebug is true when the value of DeadLockDebug in registry is one 
        /// and debugger is attached to the process.
        /// </summary>
        /// <MetaDataID>{07603b26-42f2-45a0-99ae-17cc54fd6b93}</MetaDataID>
        static bool AllowDeadLockDebug
        {
            get
            {
#if DEBUG
                return false;
                lock (StaticLock)
                {
                    if (_AllowDeadLockDebug == DeadLockDebugState.Uninitialized)
                    {
                        _AllowDeadLockDebug = DeadLockDebugState.NoDeadLockDebug;
                        Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\OOAdvantech");
                        if (key != null)
                        {
                            try
                            {
                                if ((int)key.GetValue("DeadLockDebug") == 1)
                                    _AllowDeadLockDebug = DeadLockDebugState.DeadLockDebug;
                            }
                            catch
                            {
                            }
                            key.Close();
                        }
                    }
                    if (_AllowDeadLockDebug == DeadLockDebugState.DeadLockDebug && System.Diagnostics.Debugger.IsAttached)
                        return true;
                    else
                        return false;
                }
#else
                return false;
#endif


            }
        }



    }

    //InternalLockCookie
    /// <MetaDataID>{cbd77705-161a-42fe-8eaf-9d4f9ac363d5}</MetaDataID>
    public struct InternalLockCookie
    {
        /// <MetaDataID>{c7c2d842-5736-4f31-ac01-c203b0b74dce}</MetaDataID>
        internal int ThreadID;
        /// <MetaDataID>{43cea134-19a5-4268-8bb8-a04b944ab423}</MetaDataID>
        internal bool UpgradeReadLock;
    }
}

