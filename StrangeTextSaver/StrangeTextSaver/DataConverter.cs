using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaver
{
    public static class DataConverter
    {
        static Random rand = new Random();


        #region ToBits
        public static bool[] ToBits(this bool b) => new bool[8] { false, false, false, false, false, false, false, b };

        public static bool[] ToBits(this bool[] arr)
        {
            int offset = 8 - arr.Length % 8;
            bool[] res = new bool[arr.Length + offset];
            arr.CopyTo(res, offset);

            return res;
        }
        public static bool[] ToBits(this byte givenByte)
        {
            bool[] result = new bool[8];

            for (int i = 7; i >= 0; i--)
            {
                if (givenByte != 0)
                {
                    result[i] = (givenByte % 2 == 1);
                    givenByte /= 2;
                    continue;
                }
                result[i] = false;
            }

            return result;
        }
        #endregion


        #region ToByte(s)
        public static byte ToByte(this bool[] bits)
        {
            if (bits.Length > 8)
                throw new ArgumentException("Cant convert more than 8 bits into 1 byte. Use ToBytes method for more flexibility");
            if (bits.Length < 8)
                bits = bits.ToBits();

            byte result = 0;
            for (int i = 7; i >= 0; i--)
                if (bits[i])
                    result += (byte)Math.Pow(2, 7 - i);

            return result;
        }

        public static byte[] ToBytes(this bool[] bits)
        {
            if (bits.Length % 8 != 0)
                bits = bits.ToBits();

            byte[] bytes = new byte[8];
            int byteLength = bits.Length / 8;

            for (int i = 0; i < byteLength; i++)
            {
                byte getByte = bits.Skip(i).Take(8).ToArray().ToByte();
                bytes.Append(getByte);
            }

            return bytes;
        }

        public static byte ConvertToByte(this char c) => Convert.ToByte(c);

        public static byte[] ConvertToBytes(this char[] arr) => arr.Select(Convert.ToByte).ToArray();

        public static byte[] ConvertToBytes(this int number) => number >= 0 ? ConvertToBytes(number) : new byte[0];
        public static byte[] ConvertToBytes(this uint number) => ConvertToBytes(number);

        public static byte[] ConvertToBytes(this short number) => number >= 0 ? ConvertToBytes(number) : new byte[0];
        public static byte[] ConvertToBytes(this ushort number) => ConvertToBytes(number);

        public static byte[] ConvertToBytes(this ulong number) => number <= long.MaxValue ? ConvertToBytes(number) : new byte[0];
        public static byte[] ConvertToBytes(this long number)
        {
            byte[] bytes = new byte[4];
            bool[] bits = new bool[32];

            for (int i = 31; i >= 0; i--)
            {
                if (number != 0)
                {
                    bits[i] = number % 2 == 1;
                    number /= 2;
                    continue;
                }
                bits[i] = false;
            }

            for (int i = 3; i >= 0; i--)
                bytes[i] = bits.Skip(i * 8).Take(8).ToArray().ToByte();

            return bytes;
        }

        public static byte[] ConvertToBytes(this string text)
        {
            return new UTF8Encoding(true).GetBytes(text);
        }

        #endregion

        static byte[] GenerateBytes(long n)
        {

            byte[] arr = new byte[n];
            for (int i = 0; i < n; i++)
                arr[i] = (byte)rand.Next(256);

            return arr;
        }

        static bool BytesEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return false;

            return true;
        }
    }
}
