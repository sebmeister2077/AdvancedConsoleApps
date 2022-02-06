using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaver
{
    public class StrangeDataSaver
    {
        //if signature matches , the value/values before indicate how many bytes have been added manually, 255 meaning you have to check the byte before too and so on
        //signature is always at the end
        protected readonly byte[] signature = new byte[20] { 13, 25, 0, 19, 20, 18, 1, 14, 7, 5, 0, 19, 9, 7, 14, 1, 20, 21, 5, 0 };
        protected readonly int signatureLength = 20;

        //the next 4 bytes will tell how many extra bytes have been added (2^(8+8+8+8) = enough)
        protected const int byteLengthData = 4;

        // improve performance
        readonly long[] exponentsOfTwo = new long[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576, 2097152, 4194304, 8388608, 16777216, 33554432, 67108864, 134217728, 268435456, 536870912, 1073741824, 2147483648 };

        public FileStream CreateFile(string dirPath, string fileName, string extension)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            FileInfo file = new FileInfo($"{dirPath}/{fileName}.{extension}");
            if (file.Exists)
                throw new InvalidOperationException("Cant create an existing file");

            return File.Create($"{dirPath}/{fileName}.{extension}");
        }

        #region Writes
        /*public void AppendTestToStream(FileStream fs, string text)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(text);
        }*/
        public void AppendBytesToFile(FileInfo file, byte[] data)
        {
            if (!file.Exists)
                throw new FileNotFoundException();

            using FileStream writer = file.OpenWrite();
            long fileLength = file.Length;
            long previousDataLength = GetWrittenByteLength(file);
            writer.Position = fileLength;
            bool hasSignature = HasSignature(file);
            if (hasSignature)
                writer.Position -= (signatureLength + byteLengthData);

            long dataLength = data.Length;
            int skipCount = 0;
            byte[] buffer;

            while (dataLength > int.MaxValue)
            {
                buffer = data.Skip(skipCount * int.MaxValue).Take(int.MaxValue).ToArray();
                writer.Write(buffer, 0, int.MaxValue);
                skipCount++;
                dataLength -= int.MaxValue;
            }
            buffer = data.Skip(skipCount * int.MaxValue).Take((int)dataLength).ToArray();
            writer.Write(buffer, 0, (int)dataLength);

            WriteLengthAndSignature(writer, previousDataLength + data.Length);
        }

        private void WriteLengthAndSignature(FileStream writer, long dataLength)
        {
            byte[] bytesLength = dataLength.ConvertToBytes();
            writer.Write(bytesLength, 0, byteLengthData);
            writer.Write(signature, 0, signatureLength);
        }

        #endregion

        #region Reads
        /// <summary>
        /// Returns the hidden data
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        public byte[] GetWrittenBytes(FileInfo file)
        {
            if (!HasSignature(file))
                return null;

            long totalWrittenBytes = GetWrittenByteLength(file);

            using FileStream reader = file.OpenRead();
            byte[] data = new byte[totalWrittenBytes];
            long fileLength = reader.Length;
            long readPosition = fileLength - signatureLength - byteLengthData;
            if (totalWrittenBytes < readPosition)
                throw new InvalidDataException("File data written overpasses the file size");
            readPosition -= totalWrittenBytes;

            reader.Position = readPosition;
            while (totalWrittenBytes > int.MaxValue)
            {
                reader.Read(data, 0, int.MaxValue);
                totalWrittenBytes -= int.MaxValue;
            }
            reader.Read(data, 0, (int)totalWrittenBytes);

            return data;
        }

        /// <summary>
        /// Returns the length of the hidden data
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public long GetWrittenByteLength(FileInfo file)
        {
            if (!HasSignature(file))
                return 0;

            using FileStream reader = file.OpenRead();

            return GetWrittenByteLength(reader);
        }

        public long GetWrittenByteLength(FileStream reader)
        {
            if (!reader.CanRead || !HasSignature(reader))
                return 0;

            long fileLength = reader.Length;
            byte[] dataLength = new byte[4];
            long readPosition = fileLength - signatureLength - byteLengthData;
            reader.Position = readPosition;
            reader.Read(dataLength, 0, signatureLength);

            return CalculateLength(dataLength);
        }
        #endregion

        /// <summary>
        /// Returns true if the file has been written to
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool HasSignature(FileInfo file)
        {
            if (file == null || !file.Exists)
                return false;

            using FileStream reader = file.OpenRead();
            return HasSignature(reader);
        }
        public bool HasSignature(FileStream reader)
        {
            if (!reader.CanRead)
                return false;

            byte[] fileSignature = new byte[20];
            long fileLength = reader.Length;
            long readPosition = fileLength - signatureLength;
            reader.Position = readPosition;
            reader.Read(fileSignature, 0, signatureLength);

            return IsSignature(fileSignature);
        }

        #region Helpers

        private long CalculateLength(byte[] bytes)
        {
            if (bytes.Length != byteLengthData)
                throw new InvalidOperationException($"Data is not valid. Input length should be {byteLengthData} but found to be {bytes.Length}");

            bool[] rawLength = CombineBytesLength(bytes);
            long length = 0;

            for (int i = 0; i < 32; i++)
                if (rawLength[i])
                    length += exponentsOfTwo[i];

            return length;
        }

        private bool[] CombineBytesLength(params byte[] bytes)
        {
            List<bool> length = new List<bool>();

            for (int i = 0; i < bytes.Length; i++)
                length.AddRange(bytes[i].ToBits());

            return length.ToArray();
        }

        private bool IsSignature(byte[] givenSignature)
        {
            if (givenSignature == null)
                return false;
            if (givenSignature.Length != 20)
                return false;

            for (int i = 0; i < 20; i++)
                if (givenSignature[i] != signature[i])
                    return false;

            return true;
        }

    }

    public static class Extensions
    {
        /// <summary>
        /// Converts a byte to its corresponding bits
        /// </summary>
        /// <param name="givenByte"></param>
        /// <returns></returns>
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
    }

    #endregion
}
