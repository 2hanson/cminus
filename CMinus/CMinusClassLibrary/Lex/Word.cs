using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Lex
{
    /// <summary>
    /// 扩充的Token（除数字和ASCII码符号外所有词法单元）
    /// </summary>
    public class Word : Token
    {
        private String lexeme = "";

        public Word(String s, int tag, int ln, int cn) : base(tag, ln, cn) { lexeme = s; }

        public String Lexeme
        {
            get
            {
                return lexeme;
            }
        }
    }
}
