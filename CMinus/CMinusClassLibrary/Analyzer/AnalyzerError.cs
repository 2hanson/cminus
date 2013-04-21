using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Analyzer
{
    public class AnalyzerError : ErrorBase.Error
    {
        public AnalyzerErrorType Analyzererrortype
        {
            get;
            set;
        }

        public String ErrorInfo
        {
            get;
            set;
        }

        public AnalyzerError(AnalyzerErrorType analyzererrortype, int lineNum, int columnNum, string errorInfo)
            : base(lineNum, columnNum)
        {
            ErrorInfo = errorInfo;
            Analyzererrortype = analyzererrortype;
        }
    }
}
