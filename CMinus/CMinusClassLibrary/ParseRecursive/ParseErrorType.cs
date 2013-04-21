using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.ParseRecursive
{
    public enum ParseErrorType
    {
        InvalidType,
        InvalidParameter,
        ReservedToken,
        MissingSemi,
        MissingSemiOnly,
        MissingSemiInReturn,
        MissingSemiAfterFunCall,
        MissingRParenthesis,
        MissingRParenthesisInFunction,
        MissingArraySize,
        MissingRBracketInArray,
        MissingRBracket,
        MissingSemiAfterDeclarationSequence,
        MissingArrayParameter,
        MissingRBraceBeforeEOF,
        MissingLParenthesis,
        MissingLParenthesisInIf,
        MissingRParenthesisInIf,
        MissingLParenthesisInWhile,
        MissingRParenthesisInWhile,
        UnpairedRBrace,
        UnpairedElseStatement,
        UndefinedSymbol,
        LeftID,
        ExpressionIsUnexpected,
        BadArgument
    }
}
