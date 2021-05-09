using System;

namespace OOAdvantech.Synchronization
{
    public struct LockCookie
    {
        //System.wThreading.LockCookies
        //   internal System.Threading.LockCookie SystemLockCookie;
        int SystemLockCookie;
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReaderWriterLock
    {

        static string Lock = "";
        static int _DebugMode = -1;
        static bool DebugMode
        {
            get
            {
                lock (Lock)
                {
                    if (_DebugMode == -1)
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                        {

                            _DebugMode = 0;
                            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\OOAdvantech");
                            if (key != null)
                            {
                                try
                                {
                                    _DebugMode = (int)key.GetValue("DeadLockDebug");
                                }
                                catch
                                {
                                }
                                key.Close();
                            }
                        }
                        else
                            _DebugMode = 0;
                    }
                    if (_DebugMode == 1)
                        return true;
                    else
                        return false;
                }

            }
        }
        public bool IsWriterLockHeld
        {
            get
            {
                return false;
            }
        }
        public bool IsReaderLockHeld
        {
            get
            {
                return true;
            }
        }

        //System.Diagnostics.StackTrace LastCallStackTrace = null;
        //System.Threading.ReaderWriterLock SystemReaderWriterLock = new System.Threading.ReaderWriterLock();
        public void AcquireReaderLock(int millisecondsTimeout)
        {
            //if (!DebugMode)
            //{
            //    SystemReaderWriterLock.AcquireReaderLock(millisecondsTimeout);
            //}
            //else
            //{
            //    try
            //    {
            //        SystemReaderWriterLock.AcquireReaderLock(millisecondsTimeout);
            //        if (DebugMode)
            //            LastCallStackTrace = new System.Diagnostics.StackTrace(1, true);
            //    }
            //    catch (System.Exception Error)
            //    {
            //        if (LastCallStackTrace != null)
            //        {
            //            System.Diagnostics.StackFrame stackFrame = LastCallStackTrace.GetFrame(0);
            //            System.Reflection.MethodBase method = stackFrame.GetMethod();
            //            string fileName = LastCallStackTrace.GetFrame(0).GetFileName();
            //            int fileLineNumber = LastCallStackTrace.GetFrame(0).GetFileLineNumber();
            //            string errorMessage = "DeadLock time out. Lock code at " + method.DeclaringType.FullName + "." + method.Name + " File :" + fileName + " Line :" + fileLineNumber.ToString();
            //            System.Diagnostics.Debug.WriteLine(errorMessage);
            //            throw new System.Exception(errorMessage, Error);

            //        }
            //        throw new System.Exception(Error.Message, Error);
            //    }
            //}
        }
        public void AcquireReaderLock(System.TimeSpan timeout)
        {
            //if (!DebugMode)
            //{
            //    SystemReaderWriterLock.AcquireReaderLock(timeout);
            //}
            //else
            //{
            //    try
            //    {
            //        SystemReaderWriterLock.AcquireReaderLock(timeout);

            //        if (DebugMode)
            //            LastCallStackTrace = new System.Diagnostics.StackTrace(1, true);
            //    }
            //    catch (System.Exception Error)
            //    {
            //        if (LastCallStackTrace != null)
            //        {
            //            System.Diagnostics.StackFrame stackFrame = LastCallStackTrace.GetFrame(0);
            //            System.Reflection.MethodBase method = stackFrame.GetMethod();
            //            string fileName = LastCallStackTrace.GetFrame(0).GetFileName();
            //            int fileLineNumber = LastCallStackTrace.GetFrame(0).GetFileLineNumber();
            //            string errorMessage = "DeadLock time out. Lock code at " + method.DeclaringType.FullName + "." + method.Name + " File :" + fileName + " Line :" + fileLineNumber.ToString();
            //            System.Diagnostics.Debug.WriteLine(errorMessage);
            //            throw new System.Exception(errorMessage, Error);
            //        }
            //        throw new System.Exception(Error.Message, Error);
            //    }
            //}

        }
        public void AcquireWriterLock(int millisecondsTimeout)
        {
            //if (!DebugMode)
            //{
            //    SystemReaderWriterLock.AcquireWriterLock(millisecondsTimeout);
            //}
            //else
            //{
            //    try
            //    {
            //        SystemReaderWriterLock.AcquireWriterLock(millisecondsTimeout);

            //        if (DebugMode)
            //            LastCallStackTrace = new System.Diagnostics.StackTrace(1, true);
            //    }
            //    catch (System.Exception Error)
            //    {
            //        if (LastCallStackTrace != null)
            //        {
            //            System.Diagnostics.StackFrame stackFrame = LastCallStackTrace.GetFrame(0);
            //            System.Reflection.MethodBase method = stackFrame.GetMethod();
            //            string fileName = LastCallStackTrace.GetFrame(0).GetFileName();
            //            int fileLineNumber = LastCallStackTrace.GetFrame(0).GetFileLineNumber();
            //            string errorMessage = "DeadLock time out. Lock code at " + method.DeclaringType.FullName + "." + method.Name + " File :" + fileName + " Line :" + fileLineNumber.ToString();
            //            System.Diagnostics.Debug.WriteLine(errorMessage);
            //            throw new System.Exception(errorMessage, Error);

            //        }
            //        throw new System.Exception(Error.Message, Error);
            //    }
            //}
        }
        public void AcquireWriterLock(System.TimeSpan timeout)
        {

            //if (!DebugMode)
            //{
            //    SystemReaderWriterLock.AcquireWriterLock(timeout);
            //}
            //else
            //{
            //    try
            //    {
            //        SystemReaderWriterLock.AcquireWriterLock(timeout);

            //        if (DebugMode)
            //            LastCallStackTrace = new System.Diagnostics.StackTrace(1, true);
            //    }
            //    catch (System.Exception Error)
            //    {
            //        if (LastCallStackTrace != null)
            //        {
            //            System.Diagnostics.StackFrame stackFrame = LastCallStackTrace.GetFrame(0);
            //            System.Reflection.MethodBase method = stackFrame.GetMethod();
            //            string fileName = LastCallStackTrace.GetFrame(0).GetFileName();
            //            int fileLineNumber = LastCallStackTrace.GetFrame(0).GetFileLineNumber();
            //            string errorMessage = "DeadLock time out. Lock code at " + method.DeclaringType.FullName + "." + method.Name + " File :" + fileName + " Line :" + fileLineNumber.ToString();
            //            System.Diagnostics.Debug.WriteLine(errorMessage);
            //            throw new System.Exception(errorMessage, Error);
            //        }
            //        throw new System.Exception(Error.Message, Error);
            //    }
            //}
        }
        public LockCookie UpgradeToWriterLock(int millisecondsTimeout)
        {
            LockCookie lockCookie=new LockCookie();
            return lockCookie;
            //if (!DebugMode)
            //{
            //    LockCookie lockCookie;
            //    lockCookie.SystemLockCookie = SystemReaderWriterLock.UpgradeToWriterLock(millisecondsTimeout);
            //    return lockCookie;
            //}
            //else
            //{
            //    try
            //    {
            //        LockCookie lockCookie;
            //        lockCookie.SystemLockCookie = SystemReaderWriterLock.UpgradeToWriterLock(millisecondsTimeout);
            //        if (DebugMode)
            //            LastCallStackTrace = new System.Diagnostics.StackTrace(1, true);
            //        return lockCookie;

            //    }
            //    catch (System.Exception Error)
            //    {
            //        if (LastCallStackTrace != null)
            //        {
            //            System.Diagnostics.StackFrame stackFrame = LastCallStackTrace.GetFrame(0);
            //            System.Reflection.MethodBase method = stackFrame.GetMethod();
            //            string fileName = LastCallStackTrace.GetFrame(0).GetFileName();
            //            int fileLineNumber = LastCallStackTrace.GetFrame(0).GetFileLineNumber();
            //            string errorMessage = "DeadLock time out. Lock code at " + method.DeclaringType.FullName + "." + method.Name + " File :" + fileName + " Line :" + fileLineNumber.ToString();
            //            System.Diagnostics.Debug.WriteLine(errorMessage);
            //            throw new System.Exception(errorMessage, Error);

            //        }
            //        throw new System.Exception(Error.Message, Error);
            //    }
            //}
        }
        public LockCookie UpgradeToWriterLock(System.TimeSpan timeout)
        {

            LockCookie lockCookie=new LockCookie();
            return lockCookie;


            //if (!DebugMode)
            //{
            //    LockCookie lockCookie;
            //    lockCookie.SystemLockCookie = SystemReaderWriterLock.UpgradeToWriterLock(timeout);
            //    return lockCookie;
            //}
            //else
            //{
            //    try
            //    {
            //        LockCookie lockCookie;
            //        lockCookie.SystemLockCookie = SystemReaderWriterLock.UpgradeToWriterLock(timeout);
            //        if (DebugMode)
            //            LastCallStackTrace = new System.Diagnostics.StackTrace(1, true);
            //        return lockCookie;

            //    }
            //    catch (System.Exception Error)
            //    {
            //        if (LastCallStackTrace != null)
            //        {
            //            System.Diagnostics.StackFrame stackFrame = LastCallStackTrace.GetFrame(0);
            //            System.Reflection.MethodBase method = stackFrame.GetMethod();
            //            string fileName = LastCallStackTrace.GetFrame(0).GetFileName();
            //            int fileLineNumber = LastCallStackTrace.GetFrame(0).GetFileLineNumber();
            //            string errorMessage = "DeadLock time out. Lock code at " + method.DeclaringType.FullName + "." + method.Name + " File :" + fileName + " Line :" + fileLineNumber.ToString();
            //            System.Diagnostics.Debug.WriteLine(errorMessage);
            //            throw new System.Exception(errorMessage, Error);

            //        }
            //        throw new System.Exception(Error.Message, Error);
            //    }
            //}

        }

        public void ReleaseReaderLock()
        {
            //SystemReaderWriterLock.ReleaseReaderLock();
        }
        public void ReleaseWriterLock()
        {
            // SystemReaderWriterLock.ReleaseWriterLock();
        }
        public bool AnyWritersSince(int seqNum)
        {
            return true;
            //  return SystemReaderWriterLock.AnyWritersSince(seqNum);
        }
        public void DowngradeFromWriterLock(ref LockCookie lockCookie)
        {
            //SystemReaderWriterLock.DowngradeFromWriterLock(ref lockCookie.SystemLockCookie);
        }
        public LockCookie ReleaseLock()
        {
            LockCookie lockCookie=new LockCookie();
            //  lockCookie.SystemLockCookie = SystemReaderWriterLock.ReleaseLock();
            return lockCookie;


        }
        public void RestoreLock(ref LockCookie lockCookie)
        {
            //SystemReaderWriterLock.RestoreLock(ref lockCookie.SystemLockCookie);
        }


    }

}
