using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Synchronization
{
    /// <MetaDataID>{663f1f8c-a651-4f22-af15-fad7200513fe}</MetaDataID>
    public struct LockCookie
    {
        //System.wThreading.LockCookies
        /// <MetaDataID>{29a0be3e-56dd-43af-892e-d8943b3200f5}</MetaDataID>
        internal object SystemLockCookie;
    }


    /// <summary></summary>
    /// <MetaDataID>{255f0fbf-3c03-43d7-aa67-fb990a1bf66d}</MetaDataID>
    public class ReaderWriterLock
    {


        /// <MetaDataID>{32f6f2f4-a623-49ff-a6d2-887a23ab163e}</MetaDataID>
        public ReaderWriterLock()
        {
            SystemReaderWriterLock = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion); 
        }
        /// <MetaDataID>{b99e7ec4-ced7-4a19-ba84-cd2dbaa2d20d}</MetaDataID>
        public ReaderWriterLock(bool debugMode)
        {
            SystemReaderWriterLock = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);
        }


        //θα πρέπει να αντικαθασταθεί από την ReaderWriterLockSlim στο .net 3.5
        //System.Threading.ReaderWriterLock SystemReaderWriterLock=new System.Threading.ReaderWriterLock();
        /// <MetaDataID>{6c729959-231e-431a-a529-ee2d8bf4148f}</MetaDataID>
        System.Threading.ReaderWriterLockSlim SystemReaderWriterLock;
        /// <MetaDataID>{281aac70-c473-4092-9369-0fa040c1b5af}</MetaDataID>
        public void AcquireReaderLock(int millisecondsTimeout)
        {

            //SystemReaderWriterLock.EnterUpgradeableReadLock();
        }
        /// <MetaDataID>{5140431a-8962-4cd4-bc9c-56a729f40f9b}</MetaDataID>
        public void AcquireReaderLock(System.TimeSpan timeout)
        {

            //SystemReaderWriterLock.TryEnterUpgradeableReadLock(timeout);
        }
        /// <MetaDataID>{5adae6b5-ee71-4523-a2f4-5e2083a9ae71}</MetaDataID>
        public void AcquireWriterLock(int millisecondsTimeout)
        {

           // SystemReaderWriterLock.TryEnterWriteLock(millisecondsTimeout);

        }
        /// <MetaDataID>{9779c006-849b-4831-9c8d-77b70fa52c50}</MetaDataID>
        public void AcquireWriterLock(System.TimeSpan timeout)
        {


           // SystemReaderWriterLock.TryEnterWriteLock(timeout);

        }
        /// <MetaDataID>{277a83ce-eb7c-4d72-be32-632eaf6ba355}</MetaDataID>
        public LockCookie UpgradeToWriterLock(int millisecondsTimeout)
        {
            LockCookie lockCookie;
            
            lockCookie.SystemLockCookie = null;
            return lockCookie;
            if (SystemReaderWriterLock.TryEnterWriteLock(millisecondsTimeout))
                return lockCookie;
            else
                throw new Exception("timeout expires before the lock request is granted.");

        }


        /// <MetaDataID>{5fa52236-d896-4221-b127-ef5faa32b649}</MetaDataID>
        public LockCookie UpgradeToWriterLock(System.TimeSpan timeout)
        {

            LockCookie lockCookie;
            lockCookie.SystemLockCookie = null;
            return lockCookie;

            if (SystemReaderWriterLock.TryEnterWriteLock(timeout))
                return lockCookie;
            else
                throw new Exception("timeout expires before the lock request is granted.");
        }

        /// <MetaDataID>{31fb357a-273d-4d2d-8232-2c025d94c59b}</MetaDataID>
        public void ReleaseReaderLock()
        {
            //TODO τι γίνεται αν καλεστεί πάνω από δύο φορές για ένα readlock
           // SystemReaderWriterLock.ExitReadLock();

        }
        /// <MetaDataID>{682c27a6-17ad-4026-8a0d-ab235cb97a41}</MetaDataID>
        public void ReleaseWriterLock()
        {
            //SystemReaderWriterLock.ExitWriteLock();
        }
        //public bool AnyWritersSince(int seqNum)
        //{
        //    return SystemReaderWriterLock.AnyWritersSince( seqNum);
        //}
        /// <MetaDataID>{b3ef014a-6d15-4694-bed3-352ee28cc2ce}</MetaDataID>
        public void DowngradeFromWriterLock(ref LockCookie lockCookie)
        {
           // SystemReaderWriterLock.ExitWriteLock();

        }
        /// <MetaDataID>{2bdbae6e-be0a-4090-8c69-6b80b1d01d35}</MetaDataID>
        public LockCookie ReleaseLock()
        {
            LockCookie lockCookie;
            lockCookie.SystemLockCookie = null;
            //SystemReaderWriterLock.ExitReadLock();
            return lockCookie;
        }
    }
}
