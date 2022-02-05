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
            var removalCondition = new Func<uint, uint, bool>((uint a, uint b) => a > b);

            var res = manager.GetBiggerFilesInDirectory(folderDir, minMbB);
        }
    }

}
