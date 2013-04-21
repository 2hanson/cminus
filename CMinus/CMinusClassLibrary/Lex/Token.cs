using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Lex
{
    /// <summary>
    /// 词法分析基本单元，包括行号列号，类型包括ASCII码及扩充编码
    /// </summary>
    public class Token
    {
        public int TokenTypes{get; set;}

        public String TokenInfo
        {
            get 
            {
                if(TokenTypes<=255)
                    return ((char)TokenTypes).ToString();
                else
                {
                    switch(TokenTypes)
                    {
                        case 256:
                            return "ELSE";
                        case 257:
                            return "EQUAL";
                        case 258:
                            return "GREATERQUAL";
                        case 259:
                            return "ID";
                        case 260:
                            return "IF";
                        case 261:
                            return "INT";
                        case 262:
                            return "LEFTCOMMON";
                        case 263:
                            return "LESSEQUAL";
                        case 264:
                            return "MINUS";
                        case 265:
                            return "NOTEQUAL";
                        case 266:
                            return "NUMBER";
                        case 267:
                            return "RETURN";
                        case 268:
                            return "RIGHTCOMMON";
                        case 269:
                            return "WHILE";
                        case 270:
                            return "VOID";
                        case 271:
                            return  "EOF";
                        case 272:
                            return  "NONE";
                        case 273:
                            return  "READ";
                        case 274:
                            return  "WRITE";

                    }
                    return null;
                }
            }
            
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber {get; set;}
        /// <summary>
        /// 列号
        /// </summary>
        public int ColumnNumber { get; set; } 

        public Token(int t, int ln, int cn) 
        { 
            TokenTypes = t;
            LineNumber = ln;
            ColumnNumber = cn;
        }

        //public String ToString() { return "" + (char)tokenType; }
    }
}
