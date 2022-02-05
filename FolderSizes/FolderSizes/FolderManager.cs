using System;
using System.Collections.Generic;
using System.IO;

namespace FolderManagerProj
{
    public class FolderManager
    {
        public List<MyFile> GetBiggerFilesInDirectory(string directoryName, uint minMB)
            => GetBiggerFilesInDirectory(directoryName, minMB, out List<string> dummyList);

        public List<MyFile> GetBiggerFilesInDirectory(string directoryName, uint minMB, out List<string> unauthorizedAccessFolders)
        {
            List<MyFile> files = new List<MyFile>();
            Queue<DirectoryInfo> dirQueue = new Queue<DirectoryInfo>();
            Queue<FileInfo> fileQueue = new Queue<FileInfo>();
            unauthorizedAccessFolders = new List<string>();

            dirQueue.Enqueue(new DirectoryInfo(directoryName));

            while (dirQueue.Count > 0 || fileQueue.Count > 0)
            {
                while (fileQueue.Count > 0)
                {
                    FileInfo file = fileQueue.Dequeue();
                    uint size = file.Length.ToMB();
                    if (size > minMB)
                        files.Add(new MyFile(size, file.Name, file.FullName));
                }

                if (dirQueue.Count > 0)
                {
                    DirectoryInfo dirInfo = dirQueue.Dequeue();
                    try
                    {
                        DirectoryInfo[] newDirInfos = dirInfo.GetDirectories();
                        FileInfo[] newFiles = dirInfo.GetFiles();

                        foreach (var info in newDirInfos)
                            dirQueue.Enqueue(info);
                        foreach (var info in newFiles)
                            fileQueue.Enqueue(info);

                    }
                    catch (Exception ex)
                    {
                        unauthorizedAccessFolders.Add(dirInfo.FullName);
                    }
                }
            }

            return files;
        }

        //returns true if all folders have been succesfully deleted, else false and outputs a list of failed deletes
        public bool RemoveFilesAndFolders(string basePath, ByteSize maxSize)
            => RemoveFilesAndFolders(basePath, maxSize, out List<string> dummy);

        public bool RemoveFilesAndFolders(string basePath, ByteSize maxSize, out List<string> failedDeletes)
        {

        }

        private void IterateThroughFoldersTree()
        {
            //use delegates as params for code reusability
        }
    }
    public class MyFile
    {
        public uint SizeMB { get; set; }

        public string Name { get; set; }

        public string FullPath { get; set; }

        public MyFile() { }

        public MyFile(uint size, string name, string fullpath)
        {
            SizeMB = size;
            Name = name;
            FullPath = fullpath;
        }
    }

    public class ByteSize
    {
        public uint Size { get; set; }

        public SizeType Type { get; set; }

        public ByteSize() { }

        public ByteSize(uint size, SizeType type)
        {
            Type = type;
            Size = size;
        }

        public void ChangeType(SizeType newType)
        {
            if (newType == Type)
                return;
            if (newType < Type)
            {
                Size = Size.IncrementType();
            }

        }
    }

    public enum SizeType
    {
        Byte = 1,
        KiloByte = 2,
        MegaByte = 3,
        GigaByte = 4,
    }

    public static class Extensions
    {
        public static uint DecrementType(this uint number) => number * 1024;

        public static uint IncrementType(this uint number) => number / 1024;

        public static uint ToMB(this long number) => (uint)(number / (1024 * 1024));
    }
}
