using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaver
{
    public static class DataConverter
    {
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

        public static byte ToByte(this bool[] bits)
        {
            byte result = 0;
            for (int i = 7; i >= 0; i--)
                if (bits[i])
                    result += (byte)Math.Pow(2, 7 - i);

            return result;
        }

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
    }
}
