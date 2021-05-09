using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.RDBMSMetaDataPersistenceRunTime
{


    /// <MetaDataID>{73c22348-c6a3-413e-bfac-3e827b764afe}</MetaDataID>
    public interface IDataBaseConnection
    {

        void CreateDataBase();
        void Close();

        ConnectionState State { get;  }

        void Open();

        IDataBaseCommand CreateCommand();

        void EnlistTransaction(System.Transactions.Transaction transaction);

        object NativeConnaction { get;}

        string ConnectionString { get; set; }

        void BeginTransaction(PersistenceLayerRunTime.TransactionContext theTransaction);

        void AbordTransaction(PersistenceLayerRunTime.TransactionContext theTransaction);

        void CommitTransaction(PersistenceLayerRunTime.TransactionContext theTransaction);
    }

    /// <MetaDataID>{486f792f-35b1-4c60-a71a-97192bee2d95}</MetaDataID>
    public interface IDataBaseCommand
    {

        string CommandText { get; set; }

        IDataBaseParameter CreateParameter();

        IDataBaseParameterCollection Parameters { get; }

        int ExecuteNonQuery();

        bool DesignTimeVisible { get; set; }

        OOAdvantech.MetaDataRepository.ObjectQueryLanguage.IDataReader ExecuteReader();

        object ExecuteScalar();
    }

    /// <MetaDataID>{a08a7409-c8b2-48dc-8dc1-4d22a1c45975}</MetaDataID>
    public interface IDataBaseParameterCollection:System.Collections.IEnumerable
    {

        void Add(IDataBaseParameter parameter);
        IDataBaseParameter this[string parameterName] { get; set; }
    }

    /// <MetaDataID>{10936605-5742-4b74-abb8-a82044179c99}</MetaDataID>
    public interface IDataBaseParameter
    {

        string ParameterName { get; set; }

        DbType DbType { get; set; }

        object Value { get; set; }
        ParameterDirection Direction { get; set; }

        int Size { get; set; }
    }



    // Summary:
    //     Describes the current state of the connection to a data source.
    /// <MetaDataID>{25d34d10-df59-4500-9cfb-d04d49fe2afa}</MetaDataID>
    [Flags]
    public enum ConnectionState
    {
        // Summary:
        //     The connection is closed.
        Closed = 0,
        //
        // Summary:
        //     The connection is open.
        Open = 1,
        //
        // Summary:
        //     The connection object is connecting to the data source. (This value is reserved
        //     for future versions of the product.)
        Connecting = 2,
        //
        // Summary:
        //     The connection object is executing a command. (This value is reserved for
        //     future versions of the product.)
        Executing = 4,
        //
        // Summary:
        //     The connection object is retrieving data. (This value is reserved for future
        //     versions of the product.)
        Fetching = 8,
        //
        // Summary:
        //     The connection to the data source is broken. This can occur only after the
        //     connection has been opened. A connection in this state may be closed and
        //     then re-opened. (This value is reserved for future versions of the product.)
        Broken = 16,
    }

    // Summary:
    //     Specifies the data type of a field, a property, or a Parameter object of
    //     a .NET Framework data provider.
    /// <MetaDataID>{6bb8924a-0f7b-443f-bb79-5a2f18b53590}</MetaDataID>
    public enum DbType
    {
        // Summary:
        //     A variable-length stream of non-Unicode characters ranging between 1 and
        //     8,000 characters.
        AnsiString = 0,
        //
        // Summary:
        //     A variable-length stream of binary data ranging between 1 and 8,000 bytes.
        Binary = 1,
        //
        // Summary:
        //     An 8-bit unsigned integer ranging in value from 0 to 255.
        Byte = 2,
        //
        // Summary:
        //     A simple type representing Boolean values of true or false.
        Boolean = 3,
        //
        // Summary:
        //     A currency value ranging from -2 63 (or -922,337,203,685,477.5808) to 2 63
        //     -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of
        //     a currency unit.
        Currency = 4,
        //
        // Summary:
        //     A type representing a date value.
        Date = 5,
        //
        // Summary:
        //     A type representing a date and time value.
        DateTime = 6,
        //
        // Summary:
        //     A simple type representing values ranging from 1.0 x 10 -28 to approximately
        //     7.9 x 10 28 with 28-29 significant digits.
        Decimal = 7,
        //
        // Summary:
        //     A floating point type representing values ranging from approximately 5.0
        //     x 10 -324 to 1.7 x 10 308 with a precision of 15-16 digits.
        Double = 8,
        //
        // Summary:
        //     A globally unique identifier (or GUID).
        Guid = 9,
        //
        // Summary:
        //     An integral type representing signed 16-bit integers with values between
        //     -32768 and 32767.
        Int16 = 10,
        //
        // Summary:
        //     An integral type representing signed 32-bit integers with values between
        //     -2147483648 and 2147483647.
        Int32 = 11,
        //
        // Summary:
        //     An integral type representing signed 64-bit integers with values between
        //     -9223372036854775808 and 9223372036854775807.
        Int64 = 12,
        //
        // Summary:
        //     A general type representing any reference or value type not explicitly represented
        //     by another DbType value.
        Object = 13,
        //
        // Summary:
        //     An integral type representing signed 8-bit integers with values between -128
        //     and 127.
        SByte = 14,
        //
        // Summary:
        //     A floating point type representing values ranging from approximately 1.5
        //     x 10 -45 to 3.4 x 10 38 with a precision of 7 digits.
        Single = 15,
        //
        // Summary:
        //     A type representing Unicode character strings.
        String = 16,
        //
        // Summary:
        //     A type representing a SQL Server DateTime value. If you want to use a SQL
        //     Server time value, use System.Data.SqlDbType.Time.
        Time = 17,
        //
        // Summary:
        //     An integral type representing unsigned 16-bit integers with values between
        //     0 and 65535.
        UInt16 = 18,
        //
        // Summary:
        //     An integral type representing unsigned 32-bit integers with values between
        //     0 and 4294967295.
        UInt32 = 19,
        //
        // Summary:
        //     An integral type representing unsigned 64-bit integers with values between
        //     0 and 18446744073709551615.
        UInt64 = 20,
        //
        // Summary:
        //     A variable-length numeric value.
        VarNumeric = 21,
        //
        // Summary:
        //     A fixed-length stream of non-Unicode characters.
        AnsiStringFixedLength = 22,
        //
        // Summary:
        //     A fixed-length string of Unicode characters.
        StringFixedLength = 23,
        //
        // Summary:
        //     A parsed representation of an XML document or fragment.
        Xml = 25,
        //
        // Summary:
        //     Date and time data. Date value range is from January 1,1 AD through December
        //     31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an
        //     accuracy of 100 nanoseconds.
        DateTime2 = 26,
        //
        // Summary:
        //     Date and time data with time zone awareness. Date value range is from January
        //     1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through
        //     23:59:59.9999999 with an accuracy of 100 nanoseconds. Time zone value range
        //     is -14:00 through +14:00.
        DateTimeOffset = 27,
    }

    // Summary:
    //     Specifies the type of a parameter within a query relative to the System.Data.DataSet.
    /// <MetaDataID>{bf992be9-d59c-4e7f-8e35-2a0e118b2a85}</MetaDataID>
    public enum ParameterDirection
    {
        // Summary:
        //     The parameter is an input parameter.
        Input = 1,
        //
        // Summary:
        //     The parameter is an output parameter.
        Output = 2,
        //
        // Summary:
        //     The parameter is capable of both input and output.
        InputOutput = 3,
        //
        // Summary:
        //     The parameter represents a return value from an operation such as a stored
        //     procedure, built-in function, or user-defined function.
        ReturnValue = 6,
    }

}
