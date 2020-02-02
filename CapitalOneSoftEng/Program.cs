using System;
using System.IO;
using System.Collections.Generic;

namespace CapitalOneSoftEng
{
    class Program
    {
        static void Main(string[] args)
        {
            List<FileScanner> fileQueue = new List<FileScanner>();

            foreach(var arg in args){
                
                string extension = Path.GetExtension(arg);

                switch (extension)
                {
                    case ".py":
                        fileQueue.Add(new FileScanner(arg, (int)FileTypes.FileType.Python));
                        break;

                    case ".java":
                        fileQueue.Add(new FileScanner(arg, (int)FileTypes.FileType.Java));
                        break;

                    case ".cs":
                        fileQueue.Add(new FileScanner(arg, (int)FileTypes.FileType.CSharp));
                        break;

                    case ".c":
                        fileQueue.Add(new FileScanner(arg, (int)FileTypes.FileType.C));
                        break;

                    case ".cpp":
                        fileQueue.Add(new FileScanner(arg, (int)FileTypes.FileType.Cpp));
                        break;

                    case ".js":
                        fileQueue.Add(new FileScanner(arg, (int)FileTypes.FileType.JavaScript));
                        break;

                    case ".ts":
                        fileQueue.Add(new FileScanner(arg, (int)FileTypes.FileType.TypeScript));
                        break;

                    case ".sql":
                        fileQueue.Add(new FileScanner(arg, (int)FileTypes.FileType.Sql));
                        break;

                    default:
                        fileQueue.Add(new FileScanner(arg));
                        break;
                }

            }

            foreach (FileScanner file in fileQueue)
            {
                file.ScanAndPrint();
            }

        }


    }
}
