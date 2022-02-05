using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Security.Principal;

namespace FolderManagerProj
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string folderDir = @"C:\Users\Sebas";
            int minMbB = 900;
            var manager = new FolderManager();
            var removalCondition = new Func<uint, ByteSize, bool>((uint a, ByteSize b) => a < b.Size);

            var res = manager.GetBiggerFilesInDirectory(folderDir, new ByteSize(100, SizeType.Byte));
        }
    }

}
