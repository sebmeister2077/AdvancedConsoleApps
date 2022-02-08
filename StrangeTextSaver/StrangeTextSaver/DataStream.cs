using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaver
{

    /// <summary>
    /// You can add hidden data inside .png, .jpg, .jpeg, .gif, .mp4 files without changing anything visible.<br></br>
    /// Best you use your own special signature (recommend at least length of 15)
    /// </summary>
    public class DataStream
    {
        //signature represents a file that has been written to with extra data
        //signature is always at the end
        readonly byte[] signature = new byte[20] { 13, 25, 0, 19, 20, 18, 1, 14, 7, 5, 0, 19, 9, 7, 14, 1, 20, 21, 5, 0 };
        readonly int signatureLength = 20;

        //the next 4 bytes will tell how many extra bytes have been added (2^(8+8+8+8) ~= 34GB (i think its enough))
        const int byteLengthData = 4;

        // improve performance
        readonly long[] exponentsOfTwo = new long[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576, 2097152, 4194304, 8388608, 16777216, 33554432, 67108864, 134217728, 268435456, 536870912, 1073741824, 2147483648 };

        public DataStream() { }
        public DataStream(byte[] customSignature)
        {
            signature = customSignature;
            signatureLength = customSignature.Length;
        }

        #region Writes

        /// <summary>
        /// Appends bytes to a given file, adds signature and byte length at the end.
        /// If signature was found, the byte lengths get added together
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public void AppendBytesToFile(string filePath, byte[] data) => AppendBytesToFile(new FileInfo(filePath), data);

        public void AppendBytesToFile(FileInfo file, byte[] data)
        {
            if (!file.Exists)
                throw new FileNotFoundException();

            long previousDataLength = GetWrittenByteLength(file);
            long fileLength = file.Length;
            bool hasSignature = HasSignature(file);

            using FileStream writer = file.OpenWrite();
            writer.Position = fileLength;
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
        /// Returns the hidden data inside the file if it has a signature
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        public byte[] GetWrittenBytes(FileInfo file)
        {
            if (!HasSignature(file))
                return null;

            using FileStream reader = file.OpenRead();
            long totalWrittenBytes = GetWrittenByteLength(reader);
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
        /// Returns the hidden data inside the file if it has a signature
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        public byte[] GetWrittenBytes(string filePath) => GetWrittenBytes(new FileInfo(filePath));

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
            if (!reader.CanRead)
                return 0;

            long fileLength = reader.Length;
            byte[] dataLength = new byte[4];
            long readPosition = fileLength - signatureLength - byteLengthData;
            reader.Position = readPosition;
            reader.Read(dataLength, 0, byteLengthData);

            return CalculateLength(dataLength);
        }
        #endregion

        /// <summary>
        /// Returns true if the file has the corresponding signature
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

            long fileLength = reader.Length;
            if (fileLength < 20)
                return false;

            byte[] fileSignature = new byte[20];
            long readPosition = fileLength - signatureLength;
            reader.Position = readPosition;
            reader.Read(fileSignature, 0, signatureLength);

            reader.Dispose();
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
                    length += exponentsOfTwo[31 - i];

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

    #endregion
}
