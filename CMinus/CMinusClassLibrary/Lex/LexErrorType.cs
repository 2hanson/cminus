using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Lex
{
    public enum LexErrorType
    {
        IllegalCharacters = 1,//非法字符
        SlashError,//除号后面是其他错误符号
        EqualError,//等于号后面是其他错误符号
        ExclamationError,//感叹号后面是其他错误符号
        GreaterLessError,//大于，小于号后面是其他错误符号
        Comment1Error,//“/*”后面没有跟着 “*/”
        Comment2Error,//出错 “/**”后面没有跟着 “/”
    }
}
