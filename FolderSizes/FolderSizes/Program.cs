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
            string folderDir = @"C:\Users\Sebas";
            int minMbB = 900;

            var res = GetBiggerFilesInDirectory(folderDir, minMbB);
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
                    try
                    {
                        
                        DirectoryInfo dirInfo = dirQueue.Dequeue();
                        DirectoryInfo[] newDirInfos = dirInfo.GetDirectories();
                        FileInfo[] newFiles = dirInfo.GetFiles();

                        foreach(var info in newDirInfos)
                            dirQueue.Enqueue(info);
                        foreach(var info in newFiles)
                            fileQueue.Enqueue(info);

                    }catch(Exception ex) { }
                }
            }

            return files;
        }

        public bool IsUserAdministrator()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }

    }
    public static class Extensions
    {
        public static int ToMB(this long number) => (int)(number / (1024 * 1024));
    }

}
