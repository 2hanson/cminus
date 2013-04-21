using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Analyzer
{
        public class LineList {	// LineListRec
	    public int lineno;	// Remember the line NO. in sourcefile
	    public LineList next;
    	
	    public LineList()
        {
            lineno= 0;
        }
    };
}
