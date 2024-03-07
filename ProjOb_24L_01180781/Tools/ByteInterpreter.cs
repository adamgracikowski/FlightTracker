using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Tools
{
    /// <summary>
    /// Responsible for converting byte[] to different data types.
    /// Endianess can be specified.
    /// </summary>
    public class ByteInterpreter
    {
        public static bool IsLittleEndian { get; set; }
        public ByteInterpreter(bool isLittleEndian = true)
        {
            IsLittleEndian = isLittleEndian;
        }
        public UInt64 GetUInt64(byte[] bytes, ref int offset)
        {
            if (IsLittleEndian != BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, offset, sizeof(UInt64));
            }
            var result = BitConverter.ToUInt64(bytes, offset);
            offset += sizeof(UInt64);
            return result;
        }
        public UInt64[] GetUInt64(byte[] bytes, ref int offset, int count)
        {
            var result = new UInt64[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = GetUInt64(bytes, ref offset);
            }
            return result;
        }
        public Int64 GetInt64(byte[] bytes, ref int offset)
        {
            if (IsLittleEndian != BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, offset, sizeof(Int64));
            }
            var result = BitConverter.ToInt64(bytes, offset);
            offset += sizeof(Int64);
            return result;
        }
        public UInt32 GetUInt32(byte[] bytes, ref int offset)
        {
            if (IsLittleEndian != BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, offset, sizeof(UInt32));
            }
            var result = BitConverter.ToUInt32(bytes, offset);
            offset += sizeof(UInt32);
            return result;
        }
        public UInt16 GetUInt16(byte[] bytes, ref int offset)
        {
            if (IsLittleEndian != BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, offset, sizeof(UInt16));
            }
            var result = BitConverter.ToUInt16(bytes, offset);
            offset += sizeof(UInt16);
            return result;
        }
        public Single GetSingle(byte[] bytes, ref int offset)
        {
            if (IsLittleEndian != BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, offset, sizeof(Single));
            }
            var result = BitConverter.ToSingle(bytes, offset);
            offset += sizeof(Single);
            return result;
        }
        public string GetString(byte[] bytes, ref int offset, int count)
        {
            var result = Encoding.ASCII.GetString(bytes, offset, count);
            offset += count;
            return result;
        }
    }
}
