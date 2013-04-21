using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Lex
{
    /// <summary>
    /// 数字的词法基本单元
    /// </summary>
    public class Number:Token
    {

        private int value;

        public Number(int v, int ln, int cn) : base((int)TokenType.NUMBER, ln, cn) { value = v; }

        public String Lexeme
        {
            get
            {
                return value.ToString();
            }
        }
    }
}
