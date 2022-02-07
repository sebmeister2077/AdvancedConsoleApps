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
            double x = double.MaxValue;
            int s = (int)x;
            string dirPath = @"D:/test";

            string filepath = $"{dirPath}/{"test"}.{"png"}";
            string filepath2 = $"{dirPath}/{"download"}.{"png"}";

            //File.Create(filepath).Close();
            StrangeDataSaver saver = new StrangeDataSaver();
            byte[] dat = new byte[10000];
            dat.Initialize();

            for (int i = 0; i < 50; i++)
            {
                saver.AppendBytesToFile(filepath, dat);
            }

            var info = new FileInfo(filepath);
            FileStream fs = info.OpenRead();
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            //var r = saver.GetWrittenBytes(filepath);
            int d = 0;
        }
    }


}
