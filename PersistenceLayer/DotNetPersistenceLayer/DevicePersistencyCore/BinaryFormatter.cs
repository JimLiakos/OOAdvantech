using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

/// <MetaDataID>{8be8506c-16c4-4e65-a069-0b08c1bdfd0b}</MetaDataID>
namespace OOAdvantech.BinaryFormatter
{
    /// <MetaDataID>{d7cb60b8-c52a-42ad-b53c-dc53776412a4}</MetaDataID>
    public class BinaryFormatter
    {

        /// <MetaDataID>{4e78b6b0-36c2-4fbe-963d-0f61e9f9410e}</MetaDataID>
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




        /// <MetaDataID>{17911d48-6171-45a5-a525-b05824f19810}</MetaDataID>
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
        /// <MetaDataID>{e48b8a94-e0eb-4b7e-bfb6-9284afbedce6}</MetaDataID>
        public static void Serialize(System.Guid value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {


            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = value.ToByteArray();
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
                byte[] stream = value.ToByteArray();
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }

        }
        /// <MetaDataID>{d2b6acf3-b13b-4f85-9720-124e96fd7e40}</MetaDataID>
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
        /// <MetaDataID>{202bd83b-e6b6-4781-9acc-505e5c0bdc3a}</MetaDataID>
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
        /// <MetaDataID>{2c554a26-d56e-4d18-a8ab-6e5f6b288fa3}</MetaDataID>
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
        /// <MetaDataID>{0716800d-1bf6-4ad7-85de-ba7ea7c74646}</MetaDataID>
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
        /// <MetaDataID>{284bedd6-5c75-4f6a-a449-61553808f24f}</MetaDataID>
        public static void Serialize(System.Byte value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = new byte[1];// BitConverter.GetBytes(value);
                stream[0] = value;
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
                byte[] stream = new byte[1];// BitConverter.GetBytes(value);
                stream[0] = value;
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }
        }
        /// <MetaDataID>{438d709e-68f0-416d-a6ca-dae07c0ebab5}</MetaDataID>
        public static decimal ToDecimal(byte[] bytes)
        {
            int[] bits = new int[4];
            bits[0] = ((bytes[0] | (bytes[1] << 8)) | (bytes[2] << 0x10)) | (bytes[3] << 0x18); //lo      
            bits[1] = ((bytes[4] | (bytes[5] << 8)) | (bytes[6] << 0x10)) | (bytes[7] << 0x18); //mid      
            bits[2] = ((bytes[8] | (bytes[9] << 8)) | (bytes[10] << 0x10)) | (bytes[11] << 0x18); //hi      
            bits[3] = ((bytes[12] | (bytes[13] << 8)) | (bytes[14] << 0x10)) | (bytes[15] << 0x18); //flags      
            return new decimal(bits);
        }
        /// <MetaDataID>{e9f4aec7-f185-46a5-a971-b3a8933512b5}</MetaDataID>
        public static byte[] GetBytes(decimal d)
        {
            byte[] bytes = new byte[16];
            int[] bits = decimal.GetBits(d);
            int lo = bits[0];
            int mid = bits[1];
            int hi = bits[2];
            int flags = bits[3];
            bytes[0] = (byte)lo;
            bytes[1] = (byte)(lo >> 8);
            bytes[2] = (byte)(lo >> 0x10);
            bytes[3] = (byte)(lo >> 0x18);
            bytes[4] = (byte)mid;
            bytes[5] = (byte)(mid >> 8);
            bytes[6] = (byte)(mid >> 0x10);
            bytes[7] = (byte)(mid >> 0x18);
            bytes[8] = (byte)hi;
            bytes[9] = (byte)(hi >> 8);
            bytes[10] = (byte)(hi >> 0x10);
            bytes[11] = (byte)(hi >> 0x18);
            bytes[12] = (byte)flags;
            bytes[13] = (byte)(flags >> 8);
            bytes[14] = (byte)(flags >> 0x10);
            bytes[15] = (byte)(flags >> 0x18);
            return bytes;
        }

        /// <MetaDataID>{d2e5ac84-f244-4181-821f-73328ab84808}</MetaDataID>
        static unsafe byte[] GetBytes(DateTime value)
        {
            return BitConverter.GetBytes(value.Ticks);
        }
        /// <MetaDataID>{0e078245-b3e2-4498-a569-ab3eb7b4c295}</MetaDataID>
        public static DateTime ToDateTime(byte[] bytes)
        {
            return new DateTime((long)BitConverter.ToInt64(bytes, 0));
        }

        /// <MetaDataID>{01d99c6c-7c42-4461-bbb7-3c15145c07ad}</MetaDataID>
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
        /// <MetaDataID>{c38f92ec-9dd5-4223-b25f-702161aaefdf}</MetaDataID>
        public static void Serialize(System.DateTime value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = GetBytes(value);
            nextAvailablePos = offset + stream.Length;
            for (short i = 0; i < stream.Length; i++)
                byteStream[offset + i] = stream[i];

        }

        /// <MetaDataID>{cafab116-569d-4f3e-adef-27023a776ea1}</MetaDataID>
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
        /// <MetaDataID>{79112237-ce66-47ca-a8e8-634057e85fa7}</MetaDataID>
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
        /// <MetaDataID>{0db19c5a-4a86-4b82-9dee-bad111f5d009}</MetaDataID>
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
        /// <MetaDataID>{0dd301e9-1a71-4e95-9bd1-7e294d710e03}</MetaDataID>
        public static void Serialize(System.SByte value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            if (!fixedLength)
            {
                int count = 1;
                byte[] stream = new byte[1];// BitConverter.GetBytes(value);
                stream[0] = (byte)value;
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
                byte[] stream = new byte[1];// BitConverter.GetBytes(value);
                stream[0] = (byte)value;
                nextAvailablePos = offset + stream.Length;
                for (short i = 0; i < stream.Length; i++)
                    byteStream[offset + i] = stream[i];
            }
        }
        /// <MetaDataID>{9eee461c-8432-4de6-9e76-8d0c9b5e0829}</MetaDataID>
        public static void Serialize(System.String value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            if (string.IsNullOrEmpty(value))
            {

                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                else
                    byteStream[offset] = 1;
                offset++;

                byteStream[offset] = 0;
                byteStream[offset + 1] = 0;
                nextAvailablePos = offset + 2;

                return;
            }
            else
            {
                byteStream[offset] = 1;
                offset++;
                nextAvailablePos = (value.Length * 2) + 2 + offset;
                System.Byte[] stringAsByteArray = System.Text.Encoding.Unicode.GetBytes(value);
                short i = 0;
                for (i = 0; i < stringAsByteArray.Length; i++)
                    byteStream[offset + i] = stringAsByteArray[i];
                byteStream[offset + i] = 0;
                byteStream[offset + i + 1] = 0;
            }
        }

        /// <MetaDataID>{96ac128a-578b-47bd-be59-5eb67bda3e15}</MetaDataID>
        public static void Serialize(System.Byte[] value, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {


            if (value != null)
            {
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(value.Length, byteStream, offset, ref offset, true);
                nextAvailablePos = offset + value.Length;
                for (int i = 0; i != value.Length; i++)
                    byteStream[offset + i] = value[i];
            }
            else
            {
                int length = 0;
                OOAdvantech.BinaryFormatter.BinaryFormatter.Serialize(length, byteStream, offset, ref nextAvailablePos, true);
            }

        }

        /// <MetaDataID>{d16f5df4-c6d9-4461-895d-d5f8f4e560c6}</MetaDataID>
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

        /// <MetaDataID>{b74feb3f-76ca-4f28-bbc1-2c2b5bd791a8}</MetaDataID>
        public static void Serialize(System.Object value, System.Type valueType, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {

            if (valueType == typeof(System.Int16))
            {

                if (value == null)
                {

                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Int16)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Int32))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Int32)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Int64))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Int64)(value), byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt16))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.UInt16)(value), byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt32))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.UInt32)(value), byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt64))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.UInt64)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Byte))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Byte)(value), byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.SByte))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.SByte)(value), byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Double))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Double)(value), byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Single))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Single)(value), byteStream, offset, ref   nextAvailablePos, false);
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
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                    return;
                }
                Serialize((System.Decimal)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Boolean))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                }
                else
                    Serialize((System.Boolean)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Guid))
            {
                if (value == null)
                {
                    byteStream[offset] = 0;
                    nextAvailablePos = offset + 1;
                }
                else
                    Serialize((System.Guid)(value), byteStream, offset, ref nextAvailablePos, false);
            }
            else if (valueType == typeof(System.String))
            {
                Serialize((System.String)(value), byteStream, offset, ref nextAvailablePos);
            }
            else if (valueType == typeof(System.Byte[]))
            {
                Serialize((System.Byte[])(value), byteStream, offset, ref  nextAvailablePos);
            }
            else
                throw new System.Exception("System can't write this type");

        }

        /// <MetaDataID>{946eb21f-ee80-4096-aec7-2161ddfb172a}</MetaDataID>
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

        /// <MetaDataID>{3930625d-2050-469f-8ccb-18121dbf5883}</MetaDataID>
        public static System.Int32 ToInt32(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(int)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(int);
                for (short i = 0; i < sizeof(int); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return BitConverter.ToInt32(stream, 0);
        }
        /// <MetaDataID>{807c206c-503b-48cf-8ded-5031b939bd20}</MetaDataID>
        public static System.Int16 ToInt16(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(Int16)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(Int16);
                for (short i = 0; i < sizeof(Int16); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return BitConverter.ToInt16(stream, 0);

        }
        /// <MetaDataID>{e5cc3f0d-c7cc-4138-ba04-0781224c03b9}</MetaDataID>
        public static System.Double ToDouble(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(Double)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(Double);
                for (short i = 0; i < sizeof(Double); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return BitConverter.ToDouble(stream, 0);
        }
        /// <MetaDataID>{be9a3229-d97c-44dd-a0cd-2aef6c2ce6f1}</MetaDataID>
        public static System.Int64 ToInt64(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(Int64)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(Int64);
                for (short i = 0; i < sizeof(Int64); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return BitConverter.ToInt64(stream, 0);
        }
        /// <MetaDataID>{8f3162fa-7a06-4156-bec4-3c9a9676a0b9}</MetaDataID>
        public static System.Guid ToGuid(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {

            byte[] stream = new byte[16];
            if (fixedLength)
            {
                nextAvailablePos = offset + 16;
                for (short i = 0; i < 16; i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return new Guid(stream);
        }
        /// <MetaDataID>{7f4d1c48-36ff-4be1-b436-30cbc0702a47}</MetaDataID>
        public static System.Single ToSingle(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(Single)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(Single);
                for (short i = 0; i < sizeof(Single); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return BitConverter.ToSingle(stream, 0);

        }
        /// <MetaDataID>{2f748cd2-d434-4849-a147-bb71b3b2a3e9}</MetaDataID>
        public static System.Boolean ToBoolean(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(Boolean)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(Boolean);
                for (short i = 0; i < sizeof(Boolean); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return BitConverter.ToBoolean(stream, 0);
        }
        /// <MetaDataID>{f6fb51bd-847d-4a82-b511-02e5fc82c303}</MetaDataID>
        public static System.Byte ToByte(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(Byte)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(Byte);
                for (short i = 0; i < sizeof(Byte); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return stream[0];
        }
        /// <MetaDataID>{45a6e2a0-5537-4c49-b4cd-2901436c6f1b}</MetaDataID>
        public static System.Decimal ToDecimal(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {

            byte[] stream = new byte[sizeof(Decimal)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(Decimal);
                for (short i = 0; i < sizeof(Decimal); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return ToDecimal(stream);
        }
        /// <MetaDataID>{34c0c448-ac12-49de-b40c-151166b5fe5e}</MetaDataID>
        public static System.DateTime ToDateTime(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {

            byte[] stream = new byte[sizeof(long)];
            nextAvailablePos = offset + sizeof(long);
            for (short i = 0; i < sizeof(long); i++)
                stream[i] = byteStream[offset + i];
            return ToDateTime(stream);
        }
        /// <MetaDataID>{24fc153c-3755-49c7-b715-8b7bab7b696e}</MetaDataID>
        public static System.UInt32 ToUInt32(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(UInt32)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(UInt32);
                for (short i = 0; i < sizeof(UInt32); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return BitConverter.ToUInt32(stream, 0);

        }
        /// <MetaDataID>{b233244d-1539-457d-897a-c88150f7c513}</MetaDataID>
        public static System.UInt16 ToUInt16(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(UInt16)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(UInt16);
                for (short i = 0; i < sizeof(UInt16); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return BitConverter.ToUInt16(stream, 0);

        }
        /// <MetaDataID>{ee11e47e-6c11-44ff-bd53-d177b6cddaa2}</MetaDataID>
        public static System.UInt64 ToUInt64(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(UInt64)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(UInt64);
                for (short i = 0; i < sizeof(UInt64); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return BitConverter.ToUInt64(stream, 0);


        }
        /// <MetaDataID>{af882777-3fe3-4f9b-9a80-c7545c0150c5}</MetaDataID>
        public static System.SByte ToSByte(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos, bool fixedLength)
        {
            byte[] stream = new byte[sizeof(SByte)];
            if (fixedLength)
            {
                nextAvailablePos = offset + sizeof(SByte);
                for (short i = 0; i < sizeof(SByte); i++)
                    stream[i] = byteStream[offset + i];
            }
            else
            {
                int count = (int)byteStream[offset];
                count = count >> 1;
                nextAvailablePos = offset + count + 1;
                for (short i = 0; i < count; i++)
                    stream[i] = byteStream[offset + i + 1];
            }
            return (sbyte)stream[0];
        }

        /// <MetaDataID>{4fff360b-96e2-455a-8ebe-49ba618b0e49}</MetaDataID>
        public static System.String ToString(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {

            if (byteStream[offset] == 0)
            {
                nextAvailablePos = offset + 1;
                return null;
            }
            offset++;
            System.Int32 pos = offset;
            while (true)
            {
                if (byteStream[pos] == 0 && byteStream[pos + 1] == 0)
                    break;
                pos++;
            }
            if (pos == offset)
            {
                nextAvailablePos = offset + 2;
                return "";
            }
            string tempString = System.Text.Encoding.Unicode.GetString(byteStream, offset, pos - offset + 1);
            nextAvailablePos = offset + (tempString.Length * 2) + 2;
            return tempString;
        }

        /// <MetaDataID>{08c8b60e-306b-435d-8da3-567c87bfeaeb}</MetaDataID>
        public static System.Byte[] ToBytes(System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            int size = ToInt32(byteStream, offset, ref offset, true);
            nextAvailablePos = offset;
            if (size == 0)
                return null;
            byte[] returnByteStream = new byte[size];
            for (int i = 0; i != size; i++)
                returnByteStream[i] = byteStream[offset + i];
            nextAvailablePos = offset + size;
            return returnByteStream;
        }

        /// <MetaDataID>{5b464eab-1af9-46a7-b9b7-05289dfa28e5}</MetaDataID>
        public static System.Object ToObject(System.Type valueType, System.Byte[] byteStream, System.Int32 offset, ref System.Int32 nextAvailablePos)
        {
            if (valueType == typeof(System.Int16))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToInt16(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Int32))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToInt32(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Int64))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToInt64(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt16))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToUInt16(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt32))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToUInt32(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.UInt64))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToUInt64(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Byte))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToByte(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.SByte))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToSByte(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Double))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToDouble(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Single))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToSingle(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.DateTime))
            {
                return ToDateTime(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Decimal))
            {
                System.Byte count = byteStream[offset];
                if (count == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToDecimal(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Boolean))
            {
                if (byteStream[offset] == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToBoolean(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.Guid))
            {
                if (byteStream[offset] == 0)
                {
                    nextAvailablePos = offset + 1;
                    return null;
                }
                return ToGuid(byteStream, offset, ref  nextAvailablePos, false);
            }
            else if (valueType == typeof(System.String))
            {
                return ToString(byteStream, offset, ref  nextAvailablePos);
            }
            else if (valueType == typeof(System.Byte[]))
            {
                return ToBytes(byteStream, offset, ref  nextAvailablePos);
            }
            throw new System.Exception("System can't read this type");

        }

    }
}
