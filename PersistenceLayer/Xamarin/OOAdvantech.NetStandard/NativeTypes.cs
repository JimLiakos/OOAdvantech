
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OOAdvantech.Remoting;
using OOAdvantech.Remoting.RestApi;
//using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;

namespace System
{

#if PORTABLE
    /// <MetaDataID>{36d6576d-c3d2-4139-ba63-113b1582435e}</MetaDataID>
    public sealed class DBNull
    {
        public static readonly DBNull Value;
        static DBNull()
        {
            Value = new DBNull();
        }

        public string ToString(IFormatProvider provider)
        {
            return string.Empty;
        }
    }



    /// <MetaDataID>{f477a588-2582-48f6-bc7f-84f0f44996ce}</MetaDataID>
    public sealed class SerializableAttribute : Attribute
    {
        public SerializableAttribute()
        {
        }
    }

    /// <MetaDataID>{bb531a10-d412-40a1-a1f3-4132253b3020}</MetaDataID>
    public sealed class NonSerializedAttribute : Attribute
    {
        public NonSerializedAttribute()
        {

        }
    }

#endif


    /// <MetaDataID>{f3f0b138-86a2-4980-a4c2-fd921d195762}</MetaDataID>
    public class SystemException : Exception
    {
        public SystemException()
        {

        }

        public SystemException(string message) : base(message)
        {

        }


        public SystemException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

}

#if PORTABLE
namespace System.Transactions
{

    /// <MetaDataID>{9ef7f95b-3779-45e6-9b9d-1bc65e0fc5de}</MetaDataID>
    public class Transaction
    {
        public void Rollback()
        {

        }
        public void Rollback(object obj)
        {
        }
    }

    /// <MetaDataID>{b114db1d-c55d-4cd3-8aef-6c5adbe0b668}</MetaDataID>
    public class CommittableTransaction : Transaction
    {
        public void Commit()
        {
        }

    }
    /// <MetaDataID>{99c12a52-5d5e-4c22-8c0b-ea633ab23fcf}</MetaDataID>
    internal class TransactionScope : IDisposable
    {
        public TransactionScope()
        {

        }
        public TransactionScope(Transaction transaction)
        {

        }



#region IDisposable Members

    public void Dispose()
        {

        }
        public void Complete()
        {

        }

#endregion
    }
    /// <MetaDataID>{8c6fc35b-4076-4c57-ab4e-f083f338d428}</MetaDataID>
    public interface IEnlistmentNotification
    {
    }



}
#endif

#if PORTABLE
namespace System.Data
{
    // Summary:
    //     Specifies SQL Server-specific data type of a field, property, for use in
    //     a System.Data.SqlClient.SqlParameter.
    /// <MetaDataID>{8b1dda92-47b7-41e7-a551-615822182178}</MetaDataID>
    public enum SqlDbType
    {
        // Summary:
        //     System.Int64. A 64-bit signed integer.
        BigInt = 0,
        //
        // Summary:
        //     System.Array of type System.Byte. A fixed-length stream of binary data ranging
        //     between 1 and 8,000 bytes.
        Binary = 1,
        //
        // Summary:
        //     System.Boolean. An unsigned numeric value that can be 0, 1, or null.
        Bit = 2,
        //
        // Summary:
        //     System.String. A fixed-length stream of non-Unicode characters ranging between
        //     1 and 8,000 characters.
        Char = 3,
        //
        // Summary:
        //     System.DateTime. Date and time data ranging in value from January 1, 1753
        //     to December 31, 9999 to an accuracy of 3.33 milliseconds.
        DateTime = 4,
        //
        // Summary:
        //     System.Decimal. A fixed precision and scale numeric value between -10 38
        //     -1 and 10 38 -1.
        Decimal = 5,
        //
        // Summary:
        //     System.Double. A floating point number within the range of -1.79E +308 through
        //     1.79E +308.
        Float = 6,
        //
        // Summary:
        //     System.Array of type System.Byte. A variable-length stream of binary data
        //     ranging from 0 to 2 31 -1 (or 2,147,483,647) bytes.
        Image = 7,
        //
        // Summary:
        //     System.Int32. A 32-bit signed integer.
        Int = 8,
        //
        // Summary:
        //     System.Decimal. A currency value ranging from -2 63 (or -9,223,372,036,854,775,808)
        //     to 2 63 -1 (or +9,223,372,036,854,775,807) with an accuracy to a ten-thousandth
        //     of a currency unit.
        Money = 9,
        //
        // Summary:
        //     System.String. A fixed-length stream of Unicode characters ranging between
        //     1 and 4,000 characters.
        NChar = 10,
        //
        // Summary:
        //     System.String. A variable-length stream of Unicode data with a maximum length
        //     of 2 30 - 1 (or 1,073,741,823) characters.
        NText = 11,
        //
        // Summary:
        //     System.String. A variable-length stream of Unicode characters ranging between
        //     1 and 4,000 characters. Implicit conversion fails if the string is greater
        //     than 4,000 characters. Explicitly set the object when working with strings
        //     longer than 4,000 characters.
        NVarChar = 12,
        //
        // Summary:
        //     System.Single. A floating point number within the range of -3.40E +38 through
        //     3.40E +38.
        Real = 13,
        //
        // Summary:
        //     System.Guid. A globally unique identifier (or GUID).
        UniqueIdentifier = 14,
        //
        // Summary:
        //     System.DateTime. Date and time data ranging in value from January 1, 1900
        //     to June 6, 2079 to an accuracy of one minute.
        SmallDateTime = 15,
        //
        // Summary:
        //     System.Int16. A 16-bit signed integer.
        SmallInt = 16,
        //
        // Summary:
        //     System.Decimal. A currency value ranging from -214,748.3648 to +214,748.3647
        //     with an accuracy to a ten-thousandth of a currency unit.
        SmallMoney = 17,
        //
        // Summary:
        //     System.String. A variable-length stream of non-Unicode data with a maximum
        //     length of 2 31 -1 (or 2,147,483,647) characters.
        Text = 18,
        //
        // Summary:
        //     System.Array of type System.Byte. Automatically generated binary numbers,
        //     which are guaranteed to be unique within a database. timestamp is used typically
        //     as a mechanism for version-stamping table rows. The storage size is 8 bytes.
        Timestamp = 19,
        //
        // Summary:
        //     System.Byte. An 8-bit unsigned integer.
        TinyInt = 20,
        //
        // Summary:
        //     System.Array of type System.Byte. A variable-length stream of binary data
        //     ranging between 1 and 8,000 bytes. Implicit conversion fails if the byte
        //     array is greater than 8,000 bytes. Explicitly set the object when working
        //     with byte arrays larger than 8,000 bytes.
        VarBinary = 21,
        //
        // Summary:
        //     System.String. A variable-length stream of non-Unicode characters ranging
        //     between 1 and 8,000 characters.
        VarChar = 22,
        //
        // Summary:
        //     System.Object. A special data type that can contain numeric, string, binary,
        //     or date data as well as the SQL Server values Empty and Null, which is assumed
        //     if no other type is declared.
        Variant = 23,
        //
        // Summary:
        //     An XML value. Obtain the XML as a string using the System.Data.SqlClient.SqlDataReader.GetValue(System.Int32)
        //     method or System.Data.SqlTypes.SqlXml.Value property, or as an System.Xml.XmlReader
        //     by calling the System.Data.SqlTypes.SqlXml.CreateReader() method.
        Xml = 25,
        //
        // Summary:
        //     A SQL Server 2005 user-defined type (UDT).
        Udt = 29,
        //
        // Summary:
        //     A special data type for specifying structured data contained in table-valued
        //     parameters.
        Structured = 30,
        //
        // Summary:
        //     Date data ranging in value from January 1,1 AD through December 31, 9999
        //     AD.
        Date = 31,
        //
        // Summary:
        //     Time data based on a 24-hour clock. Time value range is 00:00:00 through
        //     23:59:59.9999999 with an accuracy of 100 nanoseconds. Corresponds to a SQL
        //     Server time value.
        Time = 32,
        //
        // Summary:
        //     Date and time data. Date value range is from January 1,1 AD through December
        //     31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an
        //     accuracy of 100 nanoseconds.
        DateTime2 = 33,
        //
        // Summary:
        //     Date and time data with time zone awareness. Date value range is from January
        //     1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through
        //     23:59:59.9999999 with an accuracy of 100 nanoseconds. Time zone value range
        //     is -14:00 through +14:00.
        DateTimeOffset = 34,
    }
}
#endif

namespace OOAdvantech.Remoting
{

    /// <MetaDataID>{364d8cfa-4615-4f77-9bca-6f0c07600332}</MetaDataID>
    public class MarshalByRefObject:System.MarshalByRefObject, IExtMarshalByRefObject,ITransparentProxy
    {
        IProxy Proxy;
        public MarshalByRefObject(IProxy proxy)
        {
            Proxy = proxy;
        }
        public MarshalByRefObject()
        {

        }
        internal System.Runtime.Remoting.ObjRef ObjRef;

        public event ProxyRecconectedHandle Reconnected;

        public object GetLifetimeService()
        {
            return null;
        }

        public IProxy GetProxy()
        {
            return Proxy;
        }
    }

}
namespace System.Runtime.Remoting.Proxies
{
    /// <MetaDataID>{a0a0e31c-4c29-4362-b11f-957db630cf59}</MetaDataID>
    public class RealProxy
    {
        private Type type;

        public RealProxy(Type type)
        {
            this.type = type;
        }


    }


}

namespace System.Runtime.Remoting
{
    /// <MetaDataID>{01f96480-b829-4b3e-af64-2ff2111f729e}</MetaDataID>
    public interface IRemotingTypeInfo
    {
    }
    /// <MetaDataID>{e476d245-994c-45d4-9ce3-fac4097f452a}</MetaDataID>
    public class RemotingServices
    {

        static Tracker Tracker;
        static RemotingServices()
        {


            Tracker = new Tracker();
        }

        internal static ObjRef Marshal(MarshalByRefObject marshalByRefObject)
        {
            lock (marshalByRefObject)
            {
                var marshalByRefObjectEx = marshalByRefObject as OOAdvantech.Remoting.MarshalByRefObject;
                if (marshalByRefObjectEx.ObjRef != null)
                    return marshalByRefObjectEx.ObjRef;
                else
                {
                    marshalByRefObjectEx.ObjRef = new ObjRef();
                    marshalByRefObjectEx.ObjRef.URI = Guid.NewGuid().ToString("N");

                    Tracker.MarshaledObject(marshalByRefObjectEx, marshalByRefObjectEx.ObjRef);

                    return marshalByRefObjectEx.ObjRef;
                }
            }

        }

        internal static string GetObjectUri(MarshalByRefObject marshalByRefObject)
        {
            lock (marshalByRefObject)
            {
                if ((marshalByRefObject as OOAdvantech.Remoting.MarshalByRefObject).ObjRef != null)
                    return (marshalByRefObject as OOAdvantech.Remoting.MarshalByRefObject).ObjRef.URI;
                else
                    return null;
            }
        }

        public static Proxy GetRealProxy(object _object)
        {
            if (_object is ITransparentProxy)
                return (_object as ITransparentProxy).GetProxy() as Proxy;
            return null;
        }
        public static ObjRef Marshal(MarshalByRefObject marshalByRefObject, string URI)
        {
            lock (marshalByRefObject)
            {
                var marshalByRefObjectEx = marshalByRefObject as OOAdvantech.Remoting.MarshalByRefObject;
                marshalByRefObjectEx.ObjRef = new ObjRef();
                marshalByRefObjectEx.ObjRef.URI = Guid.NewGuid().ToString("N");
                Tracker.MarshaledObject(marshalByRefObject, marshalByRefObjectEx.ObjRef);
                return marshalByRefObjectEx.ObjRef;
            }
        }
    }
    /// <MetaDataID>{1e1ed207-d7af-4581-a527-9efa133ea4d7}</MetaDataID>
    public class ObjRef
    {
        public string URI { get; set; }
    }
}


namespace System.Runtime.Remoting.Messaging
{
    /// <MetaDataID>{6c279027-7486-4d03-abe2-e161b42a01fd}</MetaDataID>
    public interface IMessage
    {
    }

    /// <MetaDataID>{c36b1525-4aad-471e-83ad-437ed95271c4}</MetaDataID>
    public class CallContext
    {
        static CallContext()
        {
            CallContextData = new ThreadLocal<Dictionary<string, object>>();
            if (!CallContextData.IsValueCreated)
                CallContextData.Value = new Dictionary<string, object>();
        }
        static ThreadLocal<Dictionary<string, object>> CallContextData;
        public static object GetData(string name)
        {
            object value = null;
            if (CallContextData.IsValueCreated)
                CallContextData.Value.TryGetValue(name, out value);
            return value;
        }

        public static void SetData(string name, object data)
        {
            if (!CallContextData.IsValueCreated)
                CallContextData.Value = new Dictionary<string, object>();

            CallContextData.Value[name] = data;
            //     Threading.thre
            //Thread.CurrentThread.GetLogicalCallContext().FreeNamedDataSlot(name);
            //Thread.CurrentThread.GetIllogicalCallContext().SetData(name, data);

        }

        internal static void FreeNamedDataSlot(string name)
        {
            if (CallContextData.IsValueCreated&& CallContextData.Value.ContainsKey(name))
                CallContextData.Value.Remove(name );
        }

        internal static object LogicalGetData(string name)
        {
            return GetData(name);
        }
    }
}
namespace System.Runtime.Remoting.Services
{
    /// <MetaDataID>{12978b70-b0a4-453f-8dd2-5f9ac2ab3c06}</MetaDataID>
    public interface ITrackingHandler
    {

    }
}