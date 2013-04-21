using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;
namespace CMinusClassLibrary.Analyzer
{
    public class FunDecListRec {
        public int count;	// record the params count
        public int	lineno;	// record the function defined line;
	    public string name;
	    public TokenType type;
	    public ParamListRec Params;
	    public FunDecListRec next;
	    public FunDecListRec()
        {
            count= 0;
            lineno = 0;
        }
	    public FunDecListRec( string s, TokenType t)
	    {
            name = s;
            type= t;
    	    count = 0;
            lineno = 0;
        }
    }
}
