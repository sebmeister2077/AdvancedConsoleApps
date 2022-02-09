using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaver
{
    public static class FileManager
    {
        public static void MoveHiddenBytesToFile(string pathSource, string pathDestination)
        {
            var saver = new DataStream();
            var data = saver.GetWrittenBytes(pathSource);
            File.WriteAllBytes(pathDestination, data);
        }

        public static void AppendBytesToSpecialFile(string bytePathSource, string bytePathDestination)
        {
            var saver = new DataStream();
            var dataToBeAdded = File.ReadAllBytes(bytePathSource);
            saver.AppendBytesToFile(bytePathDestination, dataToBeAdded);
        }

    }
}
