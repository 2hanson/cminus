using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.ErrorBase
{
    public class Error
    {
        public int LineNum { get; set; }
        public int ColumnNum { get; set; }

        public Error(int lineNum, int columnNum)
        {
            LineNum = lineNum;
            ColumnNum = columnNum;
        }

    }
}
