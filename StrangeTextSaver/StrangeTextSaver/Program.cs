using System;
using System.IO;

namespace DataSaver
{
    internal class Program
    {
        //save text in a png/jpeg or other types of files
        static void Main(string[] args)
        {
            string dirPath = @"D:/test";
            var saver = new StrangeDataSaver();

            string filepath = $"{dirPath}/{"helloworld"}.{"in"}";
            FileInfo file = new FileInfo(filepath);
            var stream = file.OpenWrite();
            //stream.Seek(0, SeekOrigin.Begin);
            stream.WriteByte(2);
            stream.Flush();
            stream.Close();
        }
    }

    public class StrangeDataSaver
    {

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

    }
}
