using System;
using System.Collections.Generic;
using System.IO;

namespace FolderSizes
{
    internal class Program
    {

        public class MyFile
        {
            public int SizeMB { get; set; }

            public string Name { get; set; }

            public string FullPath { get; set; }

            public MyFile() { }

            public MyFile(int size, string name, string fullpath)
            {
                SizeMB = size;
                Name = name;
                FullPath = fullpath;
            }
        }
        static void Main(string[] args)
        {
            string folderDir = @"C:\Users";
            int minMbB = 900;

            var r = GetBiggerFilesInDirectory(folderDir, minMbB);
            int x = 0;
        }

        static public List<MyFile> GetBiggerFilesInDirectory(string directoryName, int minMB)
        {
            List<MyFile> files = new List<MyFile>();
            Queue<DirectoryInfo> dirQueue = new Queue<DirectoryInfo>();
            Queue<FileInfo> fileQueue = new Queue<FileInfo>();

            dirQueue.Enqueue(new DirectoryInfo(directoryName));

            while(dirQueue.Count > 0 || fileQueue.Count > 0)
            {
                while(fileQueue.Count > 0)
                {
                    FileInfo file = fileQueue.Dequeue();
                    int size = file.Length.ToMB();
                    if (size > minMB)
                        files.Add(new MyFile(size, file.Name, file.FullName));
                }

                if(dirQueue.Count > 0)
                {
                    DirectoryInfo dirInfo = dirQueue.Dequeue();
                    DirectoryInfo[] newDirInfos = dirInfo.GetDirectories();
                    FileInfo[] newFiles = dirInfo.GetFiles();

                    foreach(var info in newDirInfos)
                        dirQueue.Enqueue(info);
                    foreach(var info in newFiles)
                        fileQueue.Enqueue(info);
                }
            }

            return files;
        }
        
    }
    public static class Extensions
    {
        public static int ToMB(this long number) => (int)(number / (1024 * 1024));
    }

}
