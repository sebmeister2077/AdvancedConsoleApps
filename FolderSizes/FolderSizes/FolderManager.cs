using System;
using System.Collections.Generic;
using System.IO;

namespace FolderManagerProj
{
    public class FolderManager
    {
        public List<MyFile> GetBiggerFilesInDirectory(string directoryName, ByteSize byteSize)
            => GetBiggerFilesInDirectory(directoryName, byteSize, out List<string> dummyList);

        public List<MyFile> GetBiggerFilesInDirectory(string directoryName, ByteSize byteSize, out List<string> unauthorizedAccessFolders)
        {
            List<MyFile> files = new List<MyFile>();
            Action<FileInfo> fileAction = (FileInfo file) =>
            {
                uint size = file.Length.ToByteSize(byteSize.Type);
                if (size > byteSize.Size)
                    files.Add(new MyFile(size, file.Name, file.FullName));
            };

            Action<DirectoryInfo> directoryAction = (DirectoryInfo directoryInfo) => { };
            IterateThroughFoldersTree(directoryName, directoryAction, fileAction, out unauthorizedAccessFolders);

            return files;
        }

        //returns true if all folders have been succesfully deleted, else false and outputs a list of failed deletes
        public bool RemoveFilesAndFolders(string basePath, ByteSize byteSize, Func<uint, ByteSize, bool> condition)
            => RemoveFilesAndFolders(basePath, byteSize, condition, out List<string> dummy);

        public bool RemoveFilesAndFolders(string basePath, ByteSize byteSize, Func<uint, ByteSize, bool> condition, out List<string> failedDeletes)
        {
            List<MyFile> files = new List<MyFile>();
            Action<FileInfo> fileAction = (FileInfo file) =>
            {
                uint size = file.Length.ToByteSize(byteSize.Type);
                if (condition(size, byteSize))
                {
                    files.Add(new MyFile(file));
                }
            };
            Action<DirectoryInfo> dirAction = (DirectoryInfo directoryInfo) => { };

            IterateThroughFoldersTree(basePath, dirAction, fileAction, out failedDeletes);
            return failedDeletes.Count > 0;
        }

        private void IterateThroughFoldersTree(string directoryName, Action<DirectoryInfo> directoryAction, Action<FileInfo> fileAction, out List<string> exceptionList)
        {
            Queue<DirectoryInfo> dirQueue = new Queue<DirectoryInfo>();
            Queue<FileInfo> fileQueue = new Queue<FileInfo>();
            dirQueue.Enqueue(new DirectoryInfo(directoryName));
            exceptionList = new List<string>();

            while (dirQueue.Count > 0 || fileQueue.Count > 0)
            {
                while (fileQueue.Count > 0)
                {
                    FileInfo file = fileQueue.Dequeue();
                    fileAction(file);
                }

                if (dirQueue.Count > 0)
                {
                    DirectoryInfo dirInfo = dirQueue.Dequeue();
                    directoryAction(dirInfo);

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
                        exceptionList.Add(dirInfo.FullName);
                    }
                }
            }
        }
    }
    public class MyFile
    {
        public uint SizeMB { get; set; }

        public string Name { get; set; }

        public string FullPath { get; set; }

        public MyFile() { }

        public MyFile(FileInfo file)
        {
            Name = file.Name;
            FullPath = file.FullName;
            SizeMB = file.Length.ToByteSize(SizeType.MegaByte);
        }

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
            if (newType > Type)
            {
                Size = Size.IncrementType();
                Type++;
            }
            else
            {
                Size = Size.DecrementType();
                Type--;
            }

            ChangeType(newType);
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

        public static uint ToByteSize(this long number, SizeType byteSize = SizeType.MegaByte)
        {
            switch (byteSize)
            {
                case SizeType.Byte:
                    return (uint)number;
                case SizeType.KiloByte:
                    return (uint)number / 1024;
                case SizeType.MegaByte:
                    return (uint)(number / (1024 * 1024));
                case SizeType.GigaByte:
                    return (uint)((number / (1024 * 1024 * 1024)));
                default:
                    return (uint)number;
            }
        }
    }
}
