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
            string filepath = $"D:/test/{"testpng"}.{"png"}";

            StrangeDataSaver saver = new StrangeDataSaver();
            byte[] dat = new byte[1000000];
            dat.Initialize();


            var info = new FileInfo(filepath);
            FileStream fs = info.OpenRead();
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            //var r = saver.GetWrittenBytes(filepath);
            int d = 0;
        }
    }


}
