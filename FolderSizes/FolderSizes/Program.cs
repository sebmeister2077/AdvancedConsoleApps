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
            string folderDir = @"D:\empty";
            var manager = new FolderManager();
            var removalCondition = new Func<uint, ByteSize, bool>((uint a, ByteSize b) => a < b.Size); // delete files which are less than the given ByteSize

            var res = manager.GetBiggerFilesInDirectory(folderDir, new ByteSize(700, SizeType.MegaByte));
            var success = manager.RemoveFilesAndFolders(folderDir, new ByteSize(100, SizeType.Byte), removalCondition);
        }
    }

}
