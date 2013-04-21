using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;
namespace CMinusClassLibrary.Analyzer
{
    public class ParamListRec {
        public TokenType type;
	    public bool	isArr;
	    public ParamListRec next;
	    public ParamListRec()
        {
            isArr= false;
        }
	    public ParamListRec(TokenType t, bool arr) 
        {
            type = t;
            isArr = arr;
        }
    };
}
