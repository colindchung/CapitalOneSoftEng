using System;
using System.Collections.Generic;
using System.Text;

namespace CapitalOneSoftEng
{
    class FileTypes
    {
        public enum FileType
        {
            Python = 1,
            Java = 2,
            CSharp = 4,
            C = 8,
            Cpp = 16,
            JavaScript = 32,
            TypeScript = 64,
            Sql = 128,
            Default = 255
        }
    }
}
