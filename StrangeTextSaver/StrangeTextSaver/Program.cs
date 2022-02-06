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
            byte b = 9;
            var c = b.ToBit();
            int d = 0;
        }
    }


}
