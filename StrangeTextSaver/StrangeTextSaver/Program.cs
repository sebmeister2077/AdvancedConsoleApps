using System;
using System.IO;
using System.Text;

namespace DataSaver
{
    public class Program
    {
        //save text in a png/jpg/jpeg/gif/mp4 or other types of files without corrupting/changing that previous data
        static void Main(string[] args)
        {
            string filepath = $"D:/test/{"test"}.{"mp4"}";

            StrangeDataSaver saver = new StrangeDataSaver();
            byte[] dat = new byte[1000000];
            dat.Initialize();
            for (int i = 0; i < 500; i++)
            {
                saver.AppendBytesToFile(filepath, dat);

            }

        }
    }


}
