using System;
using System.IO;
using System.Text;

namespace DataSaver
{
    public class Program
    {
        //save text in a png/jpg/jpeg or other types of files
        static void Main(string[] args)
        {
            string filepath = $"D:/test/{"processed"}.{"jpeg"}";

            StrangeDataSaver saver = new StrangeDataSaver();
            byte[] dat = new byte[1000000];
            dat.Initialize();

            saver.AppendBytesToFile(filepath, dat);

        }
    }


}
