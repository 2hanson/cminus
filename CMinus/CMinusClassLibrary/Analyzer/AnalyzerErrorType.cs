using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Analyzer
{
    public enum AnalyzerErrorType
    {
        VariableRedefinition,
        IdentifierUndeclared,
        MaybeCompilerBug
    }
}
