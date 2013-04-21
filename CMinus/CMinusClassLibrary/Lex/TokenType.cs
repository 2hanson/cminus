using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Lex
{
    /// <summary>
    /// 枚举扩充编码
    /// </summary>
     public enum TokenType
    {
         ELSE  = 256, //else
         EQUAL    = 257, //== 
         GREATERQUAL    = 258,  //>=
         ID    = 259, //变量 
         IF    = 260, //if
         INT = 261,  //int
         LEFTCOMMON =262, // /*
         LESSEQUAL    = 263,  //<=
         MINUS = 264, // 负号   
         NOTEQUAL = 265,  //!=
         NUMBER  = 266, //数字
         RETURN = 267, //return
         RIGHTCOMMON =268, // */
         WHILE = 269,  //while
         VOID = 270,//void
         EOF = 271,//文件尾
         NONE = 272,
         READ = 273,/* read */
         WRITE = 274,/* write */
         ERROR = 275
    }
}
