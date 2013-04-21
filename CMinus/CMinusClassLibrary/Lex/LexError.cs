using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.ErrorBase;

namespace CMinusClassLibrary.Lex
{
    public class LexError : Error
    {
        public LexErrorType LexErrorType
        {
            get;
            set;
        }
        
        public String ErrorInfo
        {
            get;
            set;
        }

        public LexError(LexErrorType lexErrorType, int lineNum, int columnNum,string errorInfo)
            : base(lineNum, columnNum)
        {
            ErrorInfo = errorInfo;
            LexErrorType = lexErrorType;
        }
    }
}
