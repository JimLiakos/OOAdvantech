using System;
using System.Collections.Generic;
using System.Text;

/// <MetaDataID>{8be8506c-16c4-4e65-a069-0b08c1bdfd0b}</MetaDataID>
namespace OOAdvantech.BinaryFormatter
{
    /// <MetaDataID>{4755119c-15c2-4c9f-b2b9-16441ee91b03}</MetaDataID>
    public class BinaryFormatter
    {

        public static void Serialize(System.Int32 value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = BitConverter.GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = BitConverter.GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }


        }

        public static byte[] GetBytes(decimal dec)
        {

            //Load four 32 bit integers from the Decimal.GetBits function
            Int32[] bits = decimal.GetBits(dec);
            //Create a temporary list to hold the bytes
            List<byte> bytes = new List<byte>();
            //iterate each 32 bit integer
            foreach (Int32 i in bits)
            {
                //add the bytes of the current 32bit integer
                //to the bytes list
                bytes.AddRange(BitConverter.GetBytes(i));
            }
            //return the bytes list as an array
            return bytes.ToArray();

            //byte[] buffer = new byte[sizeof(decimal)];
            //fixed (byte* numRef = buffer)
            //{
            //    //fixed(decimal* valueRef=&value)
            //    {
            //        *((decimal*)numRef) = value;// *valueRef;
            //    }
            //}
            //return buffer;
        }
        public static decimal ToDecimal(byte[] bytes)
        {

            if (bytes.Length != 16)
                throw new Exception("A decimal must be created from exactly 16 bytes");
            //make an array to convert back to int32's
            Int32[] bits = new Int32[4];
            for (int i = 0; i <= 15; i += 4)
            {
                //convert every 4 bytes into an int32
                bits[i / 4] = BitConverter.ToInt32(bytes, i);
            }
            //Use the decimal's new constructor to
            //create an instance of decimal
            return new decimal(bits);
        }
        static byte[] GetBytes(DateTime value)
        {
            return BitConverter.GetBytes(value.Ticks);

            //byte[] buffer = new byte[sizeof(decimal)];
            //fixed (byte* numRef = buffer)
            //{
            //    *((DateTime*)numRef) = value;
            //}
            //return buffer;
        }


        public static void Serialize(System.Int16 value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = BitConverter.GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = BitConverter.GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }



        }
        public static void Serialize(System.Guid value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = value.ToByteArray();
            nextAvailablePos = offset + stream.Length;
            for (short i = 0; i < stream.Length; i++)
                byteStream[offset + i] = stream[i];
        }

        public static void Serialize(System.Guid? value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            byteStream[offset] = 0;
            if (value != null)
            {
                byteStream[offset] = 16;
                byte[] stream = value.Value.ToByteArray();
                nextAvailablePos = offset + stream.Length + 1;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + 1 + i] = stream[i];
            }
            else
                nextAvailablePos = offset + 1;

        }
        public static void Serialize(System.Double value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = BitConverter.GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = BitConverter.GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }


        }

        public static void Serialize(System.Double? value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            int count = 255;
            int countOffset = offset;
            nextAvailablePos = offset + 1;
            if (value != null)
            {
                count = 0;
                byte[] stream = BitConverter.GetBytes(value.Value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                countOffset = offset;
                nextAvailablePos = offset + count + 1;
            }
            byteStream[countOffset] = (byte)count;
        }


        public static void Serialize(System.Int32? value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            int count = 255;
            int countOffset = offset;
            nextAvailablePos = offset + 1;
            if (value != null)
            {
                count = 0;
                byte[] stream = BitConverter.GetBytes(value.Value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                countOffset = offset;
                nextAvailablePos = offset + count + 1;
            }
            byteStream[countOffset] = (byte)count;
        }
        public static void Serialize(System.Int64? value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            int count = 255;
            int countOffset = offset;
            nextAvailablePos = offset + 1;
            if (value != null)
            {
                count = 0;
                byte[] stream = BitConverter.GetBytes(value.Value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                countOffset = offset;
                nextAvailablePos = offset + count + 1;
            }
            byteStream[countOffset] = (byte)count;
        }

        public static void Serialize(System.Int64 value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = BitConverter.GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = BitConverter.GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }


        }
        public static void Serialize(System.Single value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = BitConverter.GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = BitConverter.GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }


        }
        public static void Serialize(System.Boolean value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {

            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = BitConverter.GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = BitConverter.GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }

        }


        public static void Serialize(System.Boolean? value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {

            int count = 0;
            byte[] stream = null;

            if (value != null)
            {
                stream = BitConverter.GetBytes(value.Value);
                count = stream.Length;

                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
            }

            int countOffset = offset;
            nextAvailablePos = offset + count + 1;
            count = (byte)count << 1;
            byteStream[countOffset] = (byte)count;

        }

        public static void Serialize(System.Byte value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;

                byteStream[offset + 1] = value;
                if (value == 0)
                    count = 0;
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                nextAvailablePos = offset + 1;
                byteStream[offset] = value;
            }
        }
        public static void Serialize(System.Decimal value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }



        }
        public static void Serialize(System.DateTime value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            //byte[] stream = GetBytes(value.ToOADate());
#if DeviceDotNet
            byte[] stream = BitConverter.GetBytes(value.ToBinary());
            nextAvailablePos = offset + stream.Length;
            for (short i = 0; i < stream.Length; i++)
                byteStream[offset + i] = stream[i];
#else
            byte[] stream = BitConverter.GetBytes(value.ToOADate());
            nextAvailablePos = offset + stream.Length;
            for (short i = 0; i < stream.Length; i++)
                byteStream[offset + i] = stream[i];

#endif
        }

        public static void Serialize(System.DateTime? value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            //byte[] stream = GetBytes(value.ToOADate());
            //#if DeviceDotNet
            //            byte[] stream = BitConverter.GetBytes(value.ToBinary());
            //            nextAvailablePos = offset + stream.Length;
            //            for (short i = 0; i < stream.Length; i++)
            //                byteStream[offset + i] = stream[i];
            //#else
            //            byte[] stream = BitConverter.GetBytes(value.ToOADate());
            //            nextAvailablePos = offset + stream.Length;
            //            for (short i = 0; i < stream.Length; i++)
            //                byteStream[offset + i] = stream[i];

            //#endif


            long? dateAsLong = null;
            if (value != null)
                dateAsLong = value.Value.ToBinary();
            Serialize(dateAsLong, byteStream, offset, ref nextAvailablePos);

        }


        public static void Serialize(System.UInt32 value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = BitConverter.GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = BitConverter.GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }


        }
        public static void Serialize(System.UInt16 value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = BitConverter.GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = BitConverter.GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }


        }
        public static void Serialize(System.UInt64 value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = BitConverter.GetBytes(value);
                for (int i = stream.Length - 1; i >= 0; i--)
                {
                    if (stream[i] != 0)
                    {
                        count = i + 1;
                        break;
                    }
                }
                for (short i = 0; i < count; i++)
                    byteStream[offset + i + 1] = stream[i];
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                byte[] stream = BitConverter.GetBytes(value);
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }


        }
        public static void Serialize(System.SByte value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;

                byteStream[offset + 1] = (byte)value;
                if (value == 0)
                    count = 0;
                int countOffset = offset;
                nextAvailablePos = offset + count + 1;
                count = (byte)count << 1;
                byteStream[countOffset] = (byte)count;
            }
            else
            {
                nextAvailablePos = offset + 1;
                byteStream[offset] = (byte)value;
            }
        }
        public static void Serialize(System.String value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            if (string.IsNullOrEmpty(value))
            {
                byteStream[offset] = 0;
                byteStream[offset + 1] = 0;
                nextAvailablePos = offset + 2;

                return;
            }
            else
            {
                nextAvailablePos = (value.Length * 2) + 2 + offset;
                System.Byte[] stringAsByteArray = System.Text.Encoding.Unicode.GetBytes(value);
                short i = 0;
                for (i = 0; i < stringAsByteArray.Length; i++)
                    byteStream[offset + i] = stringAsByteArray[i];
                byteStream[offset + i] = 0;
                byteStream[offset + i + 1] = 0;
            }
        }

        public static void Serialize(byte[] value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {

            if (value != null)
            {
                Serialize(value.Length, byteStream, offset, ref offset, true);
                nextAvailablePos = offset + value.Length;
                for (int i = 0; i != value.Length; i++)
                    byteStream[offset + i] = value[i];
            }
            else
            {
                int length = 0;
                Serialize(length, byteStream, offset, ref offset, true);
                nextAvailablePos = offset;
            }

            //if (value == null)
            //{
            //    byteStream[offset] = 0;
            //    byteStream[offset + 1] = 0;
            //    nextAvailablePos = offset + 2;
            //    return;
            //}
            //else
            //{
            //    nextAvailablePos = (value.Length * 2) + 2 + offset;
            //    System.Byte[] stringAsByteArray = value;
            //    short i = 0;
            //    for (i = 0; i < stringAsByteArray.Length; i++)
            //        byteStream[offset + i] = stringAsByteArray[i];
            //    byteStream[offset + i] = 0;
            //    byteStream[offset + i + 1] = 0;
            //}
        }



        public static void Serialize(System.Byte[] value, System.Int32 valueOffset, System.Int32 count, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            if (value != null && count != 0)
            {
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(count, byteStream, offset, ref offset, true);
                nextAvailablePos = offset + count;
                for (int i = 0; i != count; i++)
                    byteStream[offset + i] = value[valueOffset + i];
            }
            else
            {
                int length = 0;
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(length, byteStream, offset, ref offset, true);
                nextAvailablePos = offset;
            }
        }


        public static void Serialize(System.Byte[] value, ByteStreamType byteStreamType, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            if (value != null)
            {
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize((long)value.Length, byteStream, offset, ref offset, true);
                nextAvailablePos = offset + value.Length;
                for (int i = 0; i !=  value.Length; i++)
                    byteStream[offset + i] = value[i];
            }
            else
            {
                long length = -1;
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(length, byteStream, offset, ref offset, true);
                nextAvailablePos = offset;
            }
        }





        public System.Object ToObject(System.Type valueType, System.Byte[] byteStream, System.Int32 offset, System.Int32 nextAvailablePos)
        {
            if (valueType == typeof(System.Int16))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToInt16(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.Int32))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToInt32(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.Int64))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToInt64(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.UInt16))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToUInt16(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.UInt32))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToUInt32(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.UInt64))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToUInt64(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.Byte))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToByte(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.SByte))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToSByte(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.Double))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToDouble(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.Single))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToSingle(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.DateTime))
            {
                return (ToDateTime(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.Decimal))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToDecimal(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.Boolean))
            {
                if (byteStream[offset] == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return (ToBoolean(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.Guid))
            {
                return (ToGuid(byteStream, offset, ref nextAvailablePos, false));
            }
            else if (valueType == typeof(System.String))
            {
                return ToString(byteStream, offset, ref nextAvailablePos);
            }
            else if (valueType == typeof(System.Byte[]))
            {
                return ToBytes(byteStream, offset, ref nextAvailablePos);
            }
            throw new System.Exception("System can't read this type");

        }










        public static void Serialize(System.Object value, System.Type valueType, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            if (valueType == typeof(System.Int16))
            {

                if (value == null)
                {
                    System.Int16 Int16 = 0;
                    Serialize(Int16, byteStream, offset, ref nextAvailablePos, false);
                    return;
                }
                Serialize((System.Int16)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Int32))
            {
                if (value == null)
                {
                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Int32)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Int64))
            {
                if (value == null)
                {

                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Int64)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt16))
            {
                if (value == null)
                {


                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.UInt16)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt32))
            {
                if (value == null)
                {

                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.UInt32)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt64))
            {
                if (value == null)
                {

                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.UInt64)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Byte))
            {
                if (value == null)
                {


                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Byte)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.SByte))
            {
                if (value == null)
                {


                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.SByte)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Double))
            {
                if (value == null)
                {

                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Double)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Single))
            {
                if (value == null)
                {


                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Single)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.DateTime))
            {
                System.DateTime nullDateTime = System.DateTime.MinValue;
                if (value == null)
                    Serialize(nullDateTime, byteStream, offset, ref nextAvailablePos, false);
                else
                    Serialize((System.DateTime)(value), byteStream, offset, ref nextAvailablePos, false);

            }
            else if (valueType == typeof(System.Decimal))
            {
                if (value == null)
                {

                    byteStream[offset] = 1;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Decimal)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Boolean))
            {
                if (value == null)
                {

                    byteStream[offset] = 2;
                    nextAvailablePos = offset + 1;
                }
                else
                    Serialize((System.Boolean)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Guid))
            {
                if (value == null)
                    Serialize(System.Guid.Empty, byteStream, offset, ref nextAvailablePos, false);
                else
                    Serialize((System.Guid)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.String))
            {
                Serialize((System.String)(value), byteStream, offset, ref nextAvailablePos);
            }
            else if (valueType == typeof(System.Byte[]))
            {
                Serialize((System.Byte[])(value), byteStream, offset, ref nextAvailablePos);
            }
            else
                throw new System.Exception("System can't write this type");

        }

        public static bool IsSerializeable(System.Type valueType)
        {
            if (valueType == typeof(System.Int16))
            {
                return true;
            }
            else if (valueType == typeof(System.Int32))
            {
                return true;
            }
            else if (valueType == typeof(System.Int64))
            {
                return true;
            }
            else if (valueType == typeof(System.UInt16))
            {
                return true;
            }
            else if (valueType == typeof(System.UInt32))
            {
                return true;
            }
            else if (valueType == typeof(System.UInt64))
            {
                return true;
            }
            else if (valueType == typeof(System.Byte))
            {
                return true;
            }
            else if (valueType == typeof(System.SByte))
            {
                return true;
            }
            else if (valueType == typeof(System.Double))
            {
                return true;
            }
            else if (valueType == typeof(System.Single))
            {
                return true;
            }
            else if (valueType == typeof(System.DateTime))
            {
                return true;
            }
            else if (valueType == typeof(System.Decimal))
            {
                return true;
            }
            else if (valueType == typeof(System.Boolean))
            {
                return true;
            }
            else if (valueType == typeof(System.Guid))
            {
                return true;
            }
            else if (valueType == typeof(System.String))
            {
                return true;
            }
            else if (valueType == typeof(System.Byte[]))
            {
                return true;
            }
            else
                return false;
        }
        public static int Size(System.Object value, System.Type valueType)
        {
            if (valueType == typeof(System.Int16))
            {
                return sizeof(System.Int16);
            }
            else if (valueType == typeof(System.Int32))
            {
                return sizeof(System.Int32);
            }
            else if (valueType == typeof(System.Int64))
            {
                return sizeof(System.Int64);
            }
            else if (valueType == typeof(System.UInt16))
            {
                return sizeof(System.UInt16);
            }
            else if (valueType == typeof(System.UInt32))
            {
                return sizeof(System.UInt32);
            }
            else if (valueType == typeof(System.UInt64))
            {
                return sizeof(System.UInt64);
            }
            else if (valueType == typeof(System.Byte))
            {
                return sizeof(System.Byte);
            }
            else if (valueType == typeof(System.SByte))
            {
                return sizeof(System.SByte);
            }
            else if (valueType == typeof(System.Double))
            {
                return sizeof(System.Double);
            }
            else if (valueType == typeof(System.Single))
            {
                return sizeof(System.Single);
            }
            else if (valueType == typeof(System.DateTime))
            {
                return 8;
            }
            else if (valueType == typeof(System.Decimal))
            {
                return sizeof(System.Decimal);
            }
            else if (valueType == typeof(System.Boolean))
            {
                return sizeof(System.Boolean);
            }
            else if (valueType == typeof(System.Guid))
            {
                return 16;
            }
            else if (valueType == typeof(System.String))
            {
                if (value == null || value is DBNull || ((System.String)(value)).Length == 0)
                    return 2;
                else
                    return (((System.String)(value)).Length * 2) + 2;
            }
            else if (valueType == typeof(System.Byte[]))
            {
                if (value == null)
                    return 4;
                else
                    return ((System.Byte[])(value)).Length + sizeof(int);
            }
            else
                throw new System.Exception("System can't write this type");
        }


        public static System.Int32 ToInt32(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            int typeSize = sizeof(System.Int32);
            byte[] stream = new byte[typeSize];
            if (fixedLength)
            {
                nextAvailablePos = offset + typeSize;
                for (short i = 0; i < typeSize; i++)
                    stream[i] = byteStream[offset + i];
                return BitConverter.ToInt32(stream, 0);
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                return BitConverter.ToInt32(stream, 0);
            }
        }
        public static System.Int16 ToInt16(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            int typeSize = sizeof(System.Int16);
            byte[] stream = new byte[typeSize];
            if (fixedLength)
            {
                nextAvailablePos = offset + typeSize;
                for (short i = 0; i < typeSize; i++)
                    stream[i] = byteStream[offset + i];
                return BitConverter.ToInt16(stream, 0);
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                return BitConverter.ToInt16(stream, 0);
            }
        }
        public static System.Double ToDouble(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            int typeSize = sizeof(System.Double);
            byte[] stream = new byte[typeSize];
            if (fixedLength)
            {
                nextAvailablePos = offset + typeSize;
                for (short i = 0; i < typeSize; i++)
                    stream[i] = byteStream[offset + i];
                return BitConverter.ToDouble(stream, 0);
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                return BitConverter.ToDouble(stream, 0);
            }
        }
        public static void ToDouble(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, out double? value)
        {
            int typeSize = sizeof(System.Double);
            byte[] stream = new byte[typeSize];
            value = null;
            int count = byteStream[offset];
            nextAvailablePos = offset + 1;
            if (count != 255)
            {
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                value = BitConverter.ToDouble(stream, 0);
            }
        }

        public static void ToInt32(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, out Int32? value)
        {
            int typeSize = sizeof(System.Int32);
            byte[] stream = new byte[typeSize];
            value = null;
            int count = byteStream[offset];
            nextAvailablePos = offset + 1;
            if (count != 255)
            {
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                value = BitConverter.ToInt32(stream, 0);
            }
        }


        public static void ToInt64(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, out Int64? value)
        {
            int typeSize = sizeof(System.Int64);
            byte[] stream = new byte[typeSize];
            value = null;
            int count = byteStream[offset];
            nextAvailablePos = offset + 1;
            if (count != 255)
            {
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                value = BitConverter.ToInt64(stream, 0);
            }
        }

        public static System.Int64 ToInt64(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            int typeSize = sizeof(System.Int64);
            byte[] stream = new byte[typeSize];
            if (fixedLength)
            {
                nextAvailablePos = offset + typeSize;
                for (short i = 0; i < typeSize; i++)
                    stream[i] = byteStream[offset + i];
                return BitConverter.ToInt64(stream, 0);
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                return BitConverter.ToInt64(stream, 0);
            }
        }
        public static System.Guid ToGuid(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            nextAvailablePos = offset + 16;
            System.Byte[] guidStream = new System.Byte[16];
            for (short i = 0; i < 16; i++)
                guidStream[i] = byteStream[offset + i];
            return new System.Guid(guidStream);
        }

        public static void ToGuid(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, out System.Guid? guidValue)
        {
            nextAvailablePos = offset + 1;
            int size = (int)byteStream[offset];
            guidValue = null;
            if (size == 0)
                return;
            nextAvailablePos = offset + 17;
            System.Byte[] guidStream = new System.Byte[16];
            for (short i = 0; i < 16; i++)
                guidStream[i] = byteStream[offset + 1 + i];

            guidValue = new System.Guid(guidStream);
        }
        public static System.Single ToSingle(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            int typeSize = sizeof(System.Single);
            byte[] stream = new byte[typeSize];
            if (fixedLength)
            {
                nextAvailablePos = offset + typeSize;
                for (short i = 0; i < typeSize; i++)
                    stream[i] = byteStream[offset + i];
                return BitConverter.ToSingle(stream, 0);
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                return BitConverter.ToSingle(stream, 0);
            }

        }


        public static void ToBoolean(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, out bool? value)
        {
            value = null;
            int count = byteStream[offset];
            count = count >> 1;
            nextAvailablePos = offset + count + 1;
            if (count != 0)
                value = BitConverter.ToBoolean(byteStream, offset + 1);

        }

        public static System.Boolean ToBoolean(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (fixedLength)
            {
                nextAvailablePos = offset + 1;
                if (byteStream[offset] == 1)
                    return true;
                else
                    return false;
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                if (count == 0)
                    return false;
                else
                    return BitConverter.ToBoolean(byteStream, offset + 1);
            }
        }
        public static System.Byte ToByte(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (fixedLength)
            {
                nextAvailablePos = offset + 1;
                return byteStream[offset];
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                if (count == 0)
                    return 0;
                else
                    return byteStream[offset + 1];
            }
        }
        public static System.Decimal ToDecimal(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            int typeSize = sizeof(System.Decimal);
            byte[] stream = new byte[typeSize];
            if (fixedLength)
            {
                nextAvailablePos = offset + typeSize;
                for (short i = 0; i < typeSize; i++)
                    stream[i] = byteStream[offset + i];
                return ToDecimal(stream);
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                return ToDecimal(stream);
            }
        }

        public static System.DateTime ToDateTime(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
#if DeviceDotNet
            nextAvailablePos = offset + 8;
            byte[] stream = new byte[8];
            for (short i = 0; i < 8; i++)
                stream[i] = byteStream[offset + i];
            return System.DateTime.FromBinary(BitConverter.ToInt64(stream, 0));
#else
            nextAvailablePos = offset + 8;
            byte[] stream = new byte[8];
            for (short i = 0; i < 8; i++)
                stream[i] = byteStream[offset + i];
            return System.DateTime.FromOADate(BitConverter.ToDouble(stream, 0));
#endif
        }


        public static void ToDateTime(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, out DateTime? value)
        {
            long? dateTimeAsLong = null;
            value = null;
            ToInt64(byteStream, offset, ref nextAvailablePos, out dateTimeAsLong);
            if (dateTimeAsLong != null)
                value = System.DateTime.FromBinary(dateTimeAsLong.Value);

            //#if DeviceDotNet
            //            nextAvailablePos = offset + 8;
            //            byte[] stream = new byte[8];
            //            for (short i = 0; i < 8; i++)
            //                stream[i] = byteStream[offset + i];
            //            return System.DateTime.FromBinary(BitConverter.ToInt64(stream, 0));
            //#else
            //            nextAvailablePos = offset + 8;
            //            byte[] stream = new byte[8];
            //            for (short i = 0; i < 8; i++)
            //                stream[i] = byteStream[offset + i];
            //            return System.DateTime.FromOADate(BitConverter.ToDouble(stream, 0));
            //#endif


        }
        public static System.UInt32 ToUInt32(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            int typeSize = sizeof(System.UInt32);
            byte[] stream = new byte[typeSize];
            if (fixedLength)
            {
                nextAvailablePos = offset + typeSize;
                for (short i = 0; i < typeSize; i++)
                    stream[i] = byteStream[offset + i];
                return BitConverter.ToUInt32(stream, 0);
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                return BitConverter.ToUInt32(stream, 0);
            }

        }
        public static System.UInt16 ToUInt16(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            int typeSize = sizeof(System.UInt16);
            byte[] stream = new byte[typeSize];
            if (fixedLength)
            {
                nextAvailablePos = offset + typeSize;
                for (short i = 0; i < typeSize; i++)
                    stream[i] = byteStream[offset + i];
                return BitConverter.ToUInt16(stream, 0);
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                return BitConverter.ToUInt16(stream, 0);
            }
        }
        public static System.UInt64 ToUInt64(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            int typeSize = sizeof(System.UInt64);
            byte[] stream = new byte[typeSize];
            if (fixedLength)
            {
                nextAvailablePos = offset + typeSize;
                for (short i = 0; i < typeSize; i++)
                    stream[i] = byteStream[offset + i];
                return BitConverter.ToUInt64(stream, 0);
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
                return BitConverter.ToUInt64(stream, 0);
            }

        }
        public static System.SByte ToSByte(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (fixedLength)
            {
                nextAvailablePos = offset + 1;
                return (sbyte)byteStream[offset];
            }
            else
            {
                int count = byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                if (count == 0)
                    return 0;
                else
                    return (sbyte)byteStream[offset + 1];
            }
        }

        public static System.String ToString(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            int pos = offset;
            while (true)
            {
                if (byteStream[pos] == 0 && byteStream[pos + 1] == 0)
                    break;
                pos += 2;
            }
            if (pos == offset)
            {
                nextAvailablePos = offset + 2;
                return "";
            }
            string tempString = System.Text.Encoding.Unicode.GetString(byteStream, offset, pos - offset);
            nextAvailablePos = offset + (tempString.Length * 2) + 2;
            return tempString;
        }
        public static System.Byte[] ToBytes(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            int size = ToInt32(byteStream, offset, ref offset, true);
            nextAvailablePos = offset;
            if (size == 0)
                return new byte[0];
            System.Byte[] returnByteStream = new System.Byte[size];
            for (int i = 0; i != size; i++)
                returnByteStream[i] = byteStream[offset + i];
            nextAvailablePos = offset + size;
            return returnByteStream;
        }

        public static void ToBytes(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, out byte[] outByteStream)
        {
            long size = ToInt64(byteStream, offset, ref offset, true);
            nextAvailablePos = offset;
            if (size == -1)
            {
                outByteStream = null;
                return;
            }
            if (size == 0)
                outByteStream = new byte[0];
            System.Byte[] returnByteStream = new System.Byte[size];
            for (int i = 0; i != size; i++)
                returnByteStream[i] = byteStream[offset + i];
            nextAvailablePos = offset + (int)size;
            outByteStream = returnByteStream;
        }


        public static System.Object ToObject(System.Type valueType, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {

            if (valueType == typeof(System.Int16))
            {
                return ToInt16(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Int32))
            {
                return ToInt32(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Int64))
            {
                return ToInt64(byteStream, offset, ref nextAvailablePos, false);

            }
            else if (valueType == typeof(System.UInt16))
            {
                return ToUInt16(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt32))
            {
                return ToUInt32(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt64))
            {
                return ToUInt64(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Byte))
            {
                return ToByte(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.SByte))
            {
                return ToSByte(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Double))
            {
                return ToDouble(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Single))
            {
                return ToSingle(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.DateTime))
            {
                return ToDateTime(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Decimal))
            {
                return ToDecimal(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Boolean))
            {
                return ToBoolean(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Guid))
            {
                return ToGuid(byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.String))
            {
                return ToString(byteStream, offset, ref nextAvailablePos);
            }
            else if (valueType == typeof(System.Byte[]))
            {
                return ToBytes(byteStream, offset, ref nextAvailablePos);
            }
            return 0;

        }



    }

    /// <MetaDataID>{708a3fb1-c34b-4c33-a799-f2531d5eb957}</MetaDataID>
    public enum ByteStreamType
    {
        Short,
        Medium,
        Long
    }


}

