using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CapitalOneSoftEng
{
    class FileScanner
    {
        public string _fileName;
        public int _fileType;
        private string _fileData;


        public int  _lineCount = 0,
            _commentCount = 0,
            _blockCommentCount = 0,
            _linesInBlockCount = 0,
            _singleCommentCount = 0,
            _todoCount = 0;
        

        /// <summary>
        /// Create a file scanner when the file type is not known
        /// </summary>
        /// <param name="filePath"></param>
        public FileScanner(string filePath)
        {
            _fileName = filePath;
            _fileData = File.ReadAllText(_fileName);
            _fileType = (int)FileTypes.FileType.Default;
        }

        /// <summary>
        /// Create a file scanner when the file type is known
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileType"></param>
        public FileScanner(string filePath, int fileType)
        {
            _fileName = filePath;
            _fileData = File.ReadAllText(_fileName);
            _fileType = fileType;
        }

        /// <summary>
        /// Make appropriate function calls and print results to console
        /// </summary>
        public void ScanAndPrint()
        {
            Console.WriteLine("Test results for: " + _fileName);

            if (_lineCount == 0)
                LineCount();

            if(_blockCommentCount == 0)
                BlockCommentCount();

            if (_singleCommentCount == 0)
                SingleCommentCount();

            if (_todoCount == 0)
                TodoCount();

            if (_fileType == (int)FileTypes.FileType.Python)
                _singleCommentCount -= _linesInBlockCount;

            _commentCount = _linesInBlockCount + _singleCommentCount;

            Console.WriteLine("Total # of Lines: " + _lineCount);
            Console.WriteLine("Total # of Comment Lines: " + _commentCount);
            Console.WriteLine("Total # of Single Line Comments: " + _singleCommentCount);
            Console.WriteLine("Total # of Comment Lines Within Block Comments: " + _linesInBlockCount);
            Console.WriteLine("Total # of Block Line Comments: " + _blockCommentCount);
            Console.WriteLine("Total # of TODO's: " + _todoCount);
            Console.WriteLine();
        }

        /// <summary>
        /// Count the number of lines in a file
        /// </summary>
        public void LineCount()
        {
            _lineCount = _fileData.Split('\n').Length;
        }

        /// <summary>
        /// Count the number of blocks and lines within the blocks
        /// </summary>
        public void BlockCommentCount()
        {
            string p = "", toCount = "";

            if ((_fileType & 30) != 0) // 30 = Java, C#, C, C++
            {
                p = @"/\*([^\*]|[\n]|(\*+([^\*/])))*\*/";
                toCount = "*";
            }
            else if ((_fileType & 96) != 0) // 96 = JavaScript, TypeScript
            {
                p = @"/\*([^\*]|[\n]|(\*+([^\*/])))*\*/";
                toCount = "\n";
            }
            else if ((_fileType & 128) != 0) // 128 = SQL
            {
                p = @"/\*.*\s*.*\*/";
                toCount = "\n";
            }
            else if ((_fileType & 1) != 0) // 1 = Python
            {
                p = @"#([^\n]*)\n{1}([\t\f\a\e ]*)(#.*\n)+";
                toCount = "#";
            }

            Regex r = new Regex(p);

            _linesInBlockCount = 0;

            foreach(Match m in r.Matches(_fileData))
            {
                _linesInBlockCount += m.Value.Split(toCount).Length - 1;
            }

            _blockCommentCount = r.Matches(_fileData).Count;

            // When counting newlines, negate the (-1) on ln 122
            // 224 = JavaScript, TypeScript, SQL
            _linesInBlockCount += ((_fileType & 224) != 0) ? _blockCommentCount : 0; 

        }

        /// <summary>
        /// Count the number of single lines comments within a file
        /// </summary>
        public void SingleCommentCount()
        {
            string p1 = "", p2 = "";

            if((_fileType & 126) != 0) // 126 = Java, C#, C, C++, JavaScript, TypeScript
            {
                p1 = "//";
                p2 = "\"([^\n]*)//([^\n]*)\"";
            }
            else if((_fileType & 1) != 0) // 1 = Python
            {
                p1 = "#";
                p2 = "\"([^\n]*)#([^\n]*)\"";
            }
            else if ((_fileType & 128) != 0) // 128 = SQL
            {
                p1 = "--";
                p2 = "\"([^\n]*)--([^\n]*)\"";
            }

            Regex r1 = new Regex(p1);
            Regex r2 = new Regex(p2);

            _singleCommentCount = r1.Matches(_fileData).Count - r2.Matches(_fileData).Count;

        }

        /// <summary>
        /// Count the number of Todo's within a file
        /// </summary>
        public void TodoCount()
        {
            Regex r = new Regex(@"(#|(//)|\*)([^\n]*)[Tt][Oo][Dd][Oo]");

            _todoCount = r.Matches(_fileData).Count;
        }
    }
}
