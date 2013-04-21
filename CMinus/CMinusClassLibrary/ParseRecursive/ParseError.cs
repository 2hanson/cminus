using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.ParseRecursive
{
    public class ParseError:ErrorBase.Error
    {
         public ParseErrorType ParseErrorType
        {
            get;
            set;
        }
        
        public String ErrorInfo
        {
            get;
            set;
        }

        public ParseError(ParseErrorType parseErrorType, int lineNum, int columnNum, Lex.Token errorInfo)
            : base(lineNum, columnNum)
        {
            if (errorInfo is Lex.Word)
            {
                Lex.Word word = errorInfo as Lex.Word;
                ErrorInfo = word.Lexeme;
            }
            else
            {
                ErrorInfo = errorInfo.TokenInfo;
            }
            ParseErrorType = parseErrorType;
        }
    }
}
