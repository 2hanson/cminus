using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;
namespace CMinusClassLibrary.Analyzer
{
        public class BucketList 
        {	// BucketListRec
	        public BucketList next;	// the next hash
            public LineList Lines { get; set; }	// reference line
            public string Name { get; set; }
	        public string	Scope  { get; set; }
            public TokenType Type { get; set; }
	        public int	memloc;		// memory location
	        public bool isArr;
            public string appear { get; set; }
            public BucketList()
            {
                appear = "";
                memloc = 0;
            }
    }

}
