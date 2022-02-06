using System;
using System.IO;
using System.Text;

namespace DataSaver
{
    public class Program
    {
        //save text in a png/jpeg or other types of files
        static void Main(string[] args)
        {
            string dirPath = @"D:/test";
            var saver = new StrangeDataSaver();

            string filepath = $"{dirPath}/{"test"}.{"png"}";
            string filepath2 = $"{dirPath}/{"download"}.{"png"}";
            /*FileInfo file = new FileInfo(filepath);
            using FileStream streamWrite = file.OpenWrite();
            FileInfo file2 = new FileInfo(filepath2);
            using FileStream streamRead = file2.OpenRead();
            var length = streamRead.Length;
            byte[] readbuffer = new byte[length];
            streamRead.Read(readbuffer, 0, (int)length);

            //byte[] pngChunk = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };
            //var r = streamRead.Read(buffer, 0, 1024);
            streamWrite.Write(readbuffer, 0, (int)length);
            streamWrite.Write(new byte[2] { 140, 120 }, 0, 2);
            streamWrite.Dispose();
            using FileStream streamReadAgain = file.OpenRead();
            streamReadAgain.Read(new byte[length], 0, (int)length);
            byte[] myBytes = new byte[2];
            streamReadAgain.Read(myBytes, 0, 2);
            int x = 0;*/
            byte[] signature1 = new byte[20] { 13, 25, 0, 19, 20, 18, 1, 14, 7, 5, 0, 19, 9, 7, 14, 1, 20, 21, 5, 0 };
            byte[] signature2 = new byte[20] { 13, 25, 0, 19, 20, 18, 1, 14, 7, 5, 0, 19, 9, 7, 14, 1, 20, 21, 5, 0 };
        }
    }

    public class StrangeDataSaver
    {
        //if signature matches , the value/values before indicate how many bytes have been added manually, 255 meaning you have to check the byte before too and so on
        byte[] signature = new byte[20] { 13, 25, 0, 19, 20, 18, 1, 14, 7, 5, 0, 19, 9, 7, 14, 1, 20, 21, 5, 0 };
        int signatureLength = 20;

        public FileStream CreateFile(string dirPath, string fileName, string extension)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            FileInfo file = new FileInfo($"{dirPath}/{fileName}.{extension}");
            if (file.Exists)
                throw new InvalidOperationException();

            return File.Create($"{dirPath}/{fileName}.{extension}");
        }

        public void AppendTestToStream(FileStream fs, string text)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(text);
        }
        public void AppendBytesToFile(FileInfo file, byte[] data)
        {

            using FileStream stream = file.OpenWrite();
            //fs.Write(data, 0, data.Length);
        }

        public byte[] GetWrittenBytes(FileInfo file)
        {
            using FileStream reader = file.OpenRead();
            byte[] fileSignature = new byte[20];
            long fileLength = reader.Length;
            long readPosition = fileLength - signatureLength;
            reader.Position = readPosition;
            reader.Read(fileSignature, 0, signatureLength);

            if (IsSignature(fileSignature))
        }

        public long GetWrittenByteLength(FileInfo file)
        {
            if (!HasSignature(file))
                return 0;

            using FileStream reader = file.OpenRead();
            byte[] fileSignature = new byte[20];
            long fileLength = reader.Length;
            long readPosition = fileLength - signatureLength;
            reader.Position = readPosition;
            reader.Read(fileSignature, 0, signatureLength);

        }

        public bool HasSignature(FileInfo file)
        {
            if (file == null || !file.Exists)
                return false;

            using FileStream reader = file.OpenRead();
            byte[] fileSignature = new byte[20];
            long fileLength = reader.Length;
            long readPosition = fileLength - signatureLength;
            reader.Position = readPosition;
            reader.Read(fileSignature, 0, signatureLength);

            return IsSignature(fileSignature);
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
}
