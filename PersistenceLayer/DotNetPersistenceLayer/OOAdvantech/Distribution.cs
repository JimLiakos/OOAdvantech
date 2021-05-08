using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech
{
    /// <MetaDataID>{b7882097-1e43-4210-8316-62bf7528a857}</MetaDataID>
    public class Distribution
    {
        /// <MetaDataID>{e7d8efb5-efbf-4372-a2a3-8ead10458a45}</MetaDataID>
        public static bool ReferenceEquals(object left,object right)
        {     
             
            if(left ==right)
                return true;
            if (left == null || right == null)
                return false;
            if (Remoting.RemotingServices.IsOutOfProcess(left as MarshalByRefObject) && Remoting.RemotingServices.IsOutOfProcess(right as MarshalByRefObject))
            {  
                bool equals= left.GetHashCode() == right.GetHashCode();
                if (equals)
                    System.Diagnostics.Debug.WriteLine("Proxy doublication");
                return equals;
            }
           return  left == right;
        }
    }
}
