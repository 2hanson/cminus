using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.ErrorBase
{
    public static class ErrorManager
    {
        private static List<Error> errorList = new List<Error>();
        private static string curstr;
        /// <summary>
        /// 
        /// </summary>
        public static List<Error> ErrorList
        {
            get {
                return errorList;
            }
        }

        public static void ClearError()
        {
            errorList.Clear();
        }

        public static void AddLexError(Error e)
        {
             
            LexError le = e as LexError;
            switch (le.LexErrorType)
            {
                case LexErrorType.Comment1Error://“/*”后面没有跟着 “*/”
                    le.ErrorInfo = "Lexcial error: " + "Invalid expression '/*', '*/' expected";
                    break;
                case LexErrorType.Comment2Error://出错 “/**”后面没有跟着 “/”
                    le.ErrorInfo = "Lexcial error: " + "Invalid expression '/**', is '/' missed!";
                    break;
                case LexErrorType.EqualError://等于号后面是其他错误符号
                    curstr = le.ErrorInfo;
                    le.ErrorInfo = "Lexcial error: " + "Invalid token '" + curstr + "' after '='";
                    break;
                case LexErrorType.ExclamationError://感叹号后面是其他错误符号
                    curstr = le.ErrorInfo;
                    le.ErrorInfo = "Lexcial error: " + "Invalid token '" + curstr + "' after '!'";
                    break;
                case LexErrorType.GreaterLessError://大于，小于号后面是其他错误符号
                    curstr = le.ErrorInfo;
                    if (curstr[0] == '>')
                        le.ErrorInfo = "Lexcial error: " + "Invalid token '" + curstr[1] + "' after '>'";
                    else
                        le.ErrorInfo = "Lexcial error: " + "Invalid token '" + curstr[1] + "' after '<'";
                    break;
                case LexErrorType.IllegalCharacters: //非法字符 {
                    curstr = le.ErrorInfo;
                    le.ErrorInfo = "Lexcial error: " + "Invalid token '" + curstr + "' occured";
                    break;
                case LexErrorType.SlashError://除号后面是其他错误符号
                    curstr = le.ErrorInfo;
                    le.ErrorInfo = "Lexcial error: " + "Invalid token '" + curstr + "' after '/'";
                    break;
                default:
                    break;
            }
            errorList.Add(e);
        }

        public static void AddParseError(Error e)
        {
            ParseRecursive.ParseError pe = e as ParseRecursive.ParseError;
            switch (pe.ParseErrorType)
            {
                case ParseRecursive.ParseErrorType.InvalidType:
                    pe.ErrorInfo = "Syntax error: Invalid Type"+"'"+pe.ErrorInfo+"'";
                    break;
                case ParseRecursive.ParseErrorType.ReservedToken:
                    pe.ErrorInfo =  "Syntax error: "+ pe.ErrorInfo +" is a reserved token";
                    break;
                case ParseRecursive.ParseErrorType.MissingSemi:
                    pe.ErrorInfo = "Syntax error:  missing \';\' after indentifer " + pe.ErrorInfo;
                    break;
                case ParseRecursive.ParseErrorType.MissingRParenthesisInFunction:
                    pe.ErrorInfo =  "Syntax error: missing \')\' in function"+ pe.ErrorInfo+"(...) declaration";
                    break;
                case ParseRecursive.ParseErrorType.MissingArraySize:
                    pe.ErrorInfo = "Syntax error: missing array size in declaration of array" + pe.ErrorInfo + "[]";
                    break;
                case ParseRecursive.ParseErrorType.MissingRBracketInArray:
                    pe.ErrorInfo = "Syntax error: missing \']\' in declaration of array" +pe.ErrorInfo+"[]";
                    break;
                case ParseRecursive.ParseErrorType.MissingSemiAfterDeclarationSequence:
                    pe.ErrorInfo = "Syntax error: missing \';\' after declaration sequence";
                    break;
                case ParseRecursive.ParseErrorType.MissingArrayParameter:
                    pe.ErrorInfo = "Syntax error: arrar parameter missing \']\'";
                    break;
                case ParseRecursive.ParseErrorType.UnpairedRBrace:
                    pe.ErrorInfo = "Syntax error: unpaired \'}\'";
                    break;
                case ParseRecursive.ParseErrorType.MissingRBraceBeforeEOF:
                    pe.ErrorInfo = "Syntax error: missing \'}\' before file end";
                    break;
                case ParseRecursive.ParseErrorType.UnpairedElseStatement:
                    pe.ErrorInfo = "Syntax error: unpaired \'else\' statement";
                    break;
                case ParseRecursive.ParseErrorType.UndefinedSymbol:
                    pe.ErrorInfo = @"Syntax error: undefined symbol \"+pe.ErrorInfo+@"\";
                    break;
                case ParseRecursive.ParseErrorType.MissingLParenthesisInIf:
                    pe.ErrorInfo = "Syntax error: missing \'(\' in \"if\" statement";
                    break;
                case ParseRecursive.ParseErrorType.MissingRParenthesisInIf:
                    pe.ErrorInfo = "Syntax error: missing \')\' in \"if\" statement";
                    break;
                case ParseRecursive.ParseErrorType.MissingRParenthesisInWhile:
                    pe.ErrorInfo = "Syntax error: missing \')\' in \"while\" statement";
                    break;
                case ParseRecursive.ParseErrorType.MissingLParenthesisInWhile:
                    pe.ErrorInfo = "Syntax error: missing \'(\' in \"while\" statement";
                    break;
                case ParseRecursive.ParseErrorType.MissingSemiOnly:
                    pe.ErrorInfo = "Syntax error: missing \';\'";
                    break;
                case ParseRecursive.ParseErrorType.MissingSemiInReturn:
                    pe.ErrorInfo = "Syntax error: missing \';\' in \"return\" stamtement";
                    break;
                case ParseRecursive.ParseErrorType.LeftID:
                    pe.ErrorInfo = "Syntax error: left of \'=\' should be a ID";
                    break;
                case ParseRecursive.ParseErrorType.MissingRBracket:
                    pe.ErrorInfo = "Syntax error: missing \']\'";
                    break;
                case ParseRecursive.ParseErrorType.MissingRParenthesis:
                    pe.ErrorInfo = "Syntax Error: missing \')\'";
                    break;
                case ParseRecursive.ParseErrorType.ExpressionIsUnexpected:
                    pe.ErrorInfo = "Syntax error: "+pe.ErrorInfo+" expression is unexpected!";
                    break;
                case ParseRecursive.ParseErrorType.MissingLParenthesis:
                    pe.ErrorInfo = "Syntax error: missing \'(\'";
                    break;
                case ParseRecursive.ParseErrorType.BadArgument:
                    pe.ErrorInfo = "Syntax error:"+ pe.ErrorInfo +" is a bad argument";
                    break;
                case ParseRecursive.ParseErrorType.MissingSemiAfterFunCall:
                    pe.ErrorInfo = "Syntax error: missing \';\' after fun call";
                    break;

            }
            errorList.Add(e);

        }

        public static void AddAnalyzerError(Error e)
        {
            Analyzer.AnalyzerError ae = e as Analyzer.AnalyzerError;
            switch (ae.Analyzererrortype)
            {
                case Analyzer.AnalyzerErrorType.VariableRedefinition:
                    ae.ErrorInfo ="Semantic error: "+ ae.ErrorInfo + " is Redefinition";
                    break;
                case Analyzer.AnalyzerErrorType.IdentifierUndeclared:
                    ae.ErrorInfo = "Semantic error: " + ae.ErrorInfo + " is Undeclared";
                    break;
                case Analyzer.AnalyzerErrorType.MaybeCompilerBug:
                    ae.ErrorInfo = "Semantic error: " + "maybe compiler bug!";
                    break;
                default:
                    ae.ErrorInfo = "Semantic error: " + "maybe compiler bug!";
                    break;
            }
            errorList.Add(e);
        }

        public static void ShowError()
        {
        }
    }
}
