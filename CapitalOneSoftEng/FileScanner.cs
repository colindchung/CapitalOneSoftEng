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
        

        public FileScanner(string filePath)
        {
            _fileName = filePath;
            _fileData = File.ReadAllText(_fileName);
            _fileType = (int)FileTypes.FileType.Default;
        }

        public FileScanner(string filePath, int fileType)
        {
            _fileName = filePath;
            _fileData = File.ReadAllText(_fileName);
            _fileType = fileType;
        }

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


        public void LineCount()
        {
            _lineCount = _fileData.Split('\n').Length;
        }

        public void BlockCommentCount()
        {
            string p = "", toCount = "";

            if ((_fileType & 30) != 0)
            {
                p = @"/\*([^\*]|[\n]|(\*+([^\*/])))*\*/";
                toCount = "*";
            }
            else if ((_fileType & 96) != 0)
            {
                p = @"/\*([^\*]|[\n]|(\*+([^\*/])))*\*/";
                toCount = "\n";
            }
            else if ((_fileType & 1) != 0)
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
            _linesInBlockCount += ((_fileType & 96) != 0) ? _blockCommentCount : 0;


        }

        public void SingleCommentCount()
        {
            string p1 = "", p2 = "";

            if((_fileType & 126) != 0)
            {
                p1 = "//";
                p2 = "\"([^\n]*)//([^\n]*)\"";
            }
            else if((_fileType & 1) != 0)
            {
                p1 = "#";
                p2 = "\"([^\n]*)#([^\n]*)\"";
            }

            Regex r1 = new Regex(p1);
            /*
             * //:     Double Slash 
             * 
             */
            Regex r2 = new Regex(p2);
            /*
             * \":        Opening quote
             * ([^\n]*):  Code block without new line
             * //:        Double Slash
             * ([^\n]*):  Codie block without new line
             * \":        Closing quote
             * 
             */

            _singleCommentCount = r1.Matches(_fileData).Count - r2.Matches(_fileData).Count;

        }

        public void TodoCount()
        {
            Regex r = new Regex(@"(#|(//)|\*)([^\n]*)[Tt][Oo][Dd][Oo]");
            /*
             * (#|(//)|\*):        # OR // OR * 
             * ([^\n]*):           Code block without new line
             * [Tt][Oo][Dd][Oo]:   Todo, case insensitive
             * 
             */

            _todoCount = r.Matches(_fileData).Count;
        }
    }
}
