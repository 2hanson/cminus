using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMinusClassLibrary.Lex
{
    public class Lexer
    {
        #region 私有变量
        //保留字表
        HashSet<String> reserve = new HashSet<String>();
        //Token链表
        List<Token> tokenList = new List<Token>();
        //当前所处于的状态
        StateType currentState = new StateType();
        //读取指针
        int readLineNum, readColNum;
        CMinusFile file = new CMinusFile();
        ArrayList buffer = new ArrayList();

        //错误信息
        String currentErrorString = "";
        //当前字符变量
        char curChar;
        #endregion

        #region public 方法
        public Lexer(String filePath)
        {
            buffer = file.ReadFile(filePath);
            //读取指针从（0,0）开始.
            readLineNum = 0;
            readColNum = -1;//每次调用GetNextCharFromBuffer需要自增1；
            currentState = StateType.START;
        }
        public List<Token> TokenList
        {
            get
            {
                return tokenList;
            }
        }
        #endregion

        #region private 方法
        /// <summary>
        /// 把token加到token链表
        /// </summary>
        /// <param name="token">token类型</param>
        private void AddToTokenList(Token token)
        {
            token.LineNumber += 1;
            token.ColumnNumber += 1;
            tokenList.Add(token);
        }
        private void AddToTokenList(Number num)
        {
            num.LineNumber += 1;
            tokenList.Add(num);
        }
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="word"></param>
        private void AddToTokenList(Word word)
        {
            word.LineNumber += 1;
            tokenList.Add(word);
        }
        /// <summary>
        /// 读取下一个字符
        /// </summary>
        private int GetNextFromBuffer()
        {
            if (readLineNum >= buffer.Count)
                return -1;
            ++readColNum;
            int ch = 0;
            //从第readLineNum行读取
            String line = buffer[readLineNum].ToString();
            if(readColNum < line.Length) {
               ch = (int)line[readColNum];
            }
            else {
                readColNum = 0;

                do
                {
                    ++readLineNum;
                    if (readLineNum >= buffer.Count)
                        return -1;
                    line = buffer[readLineNum].ToString();
                    //该新行有没有符号
                }
                while (0 >= line.Length);
                //{
                //    ++readLineNum;
                //    if (readLineNum >= buffer.Count)
                //        return -1;
                //    line = buffer[readLineNum].ToString();
                //    //读完所有内容
                
                //}
                ch = (int)line[readColNum];
            }
            //if (readcolNum < )
            //for (int i = 0; i <= buffer.Count; i++)
            //{
            //    String line = buffer[i].ToString();
            //    for (int j = 0; j < line.Length; j++)
            //    {

            //    }
            //}
            //test
            char test = (char)ch;
            return ch;
        }

        /// <summary>
        /// 多读取字符，回退该字符
        /// </summary>
        private void RollBack()
        {
            --readColNum;
            if (readColNum < 0) {
                --readLineNum;
                String line = buffer[readLineNum].ToString();
                readColNum = line.Length-1;
            }
        }

        /// <summary>
        ///  
        /// </summary>
       private void ErrorRecord(LexErrorType lexErrorType, int lineNum, int columnNum)
        {
            currentErrorString += curChar;
            ErrorBase.ErrorManager.AddLexError(new LexError(lexErrorType, lineNum+1, columnNum+1, currentErrorString));
            currentErrorString = "";
        }

        /// <summary>
        /// 扫描获取Token
        /// </summary>
        public void Scan()
        {
            //清空错误列表
            ErrorBase.ErrorManager.ClearError();
            
            int cur;

            //获取下一个字符
            while ((cur = GetNextFromBuffer()) != -1)
            {
                curChar = (char)cur;
                currentState = StateType.START;
                while (currentState != StateType.DONE/*未到终止状态*/)
                {

                    switch (currentState)
                    {
                        case StateType.START:
                            if (char.IsLetter(curChar))
                            {
                                currentState = StateType.ID;//进入标识符状态
                            }
                            else if (char.IsDigit(curChar))
                            {
                                currentState = StateType.NUMBER;//进入数字状态
                            }
                            else if (curChar == '/')
                            {
                                currentState = StateType.NIUBISTATE;//遇到'/',它可能是除号，也可能是注释，所以暂定为进入牛×状态。嘿嘿
                            }
                            else if (curChar == '=')
                            {
                                currentState = StateType.ASSIGN;//进入赋值状态
                            }
                            else if (curChar == '!')
                            {
                                currentState = StateType.NOT;// != 
                            }
                            else if (curChar == '>' || curChar == '<')
                            {
                                currentState = StateType.GREATERORLESS;//
                            }
                            else
                            {
                                switch (curChar)
                                {
                                    case '+':
                                        AddToTokenList(new Token('+', readLineNum, readColNum));
                                        break;
                                    case '-':
                                        AddToTokenList(new Token('-', readLineNum, readColNum));
                                        break;
                                    case '*':
                                        AddToTokenList(new Token('*', readLineNum, readColNum));
                                        break;
                                    case '(':
                                        AddToTokenList(new Token('(', readLineNum, readColNum));
                                        break;
                                    case ')':
                                        AddToTokenList(new Token(')', readLineNum, readColNum));
                                        break;
                                    case ';':
                                        AddToTokenList(new Token(';', readLineNum, readColNum));
                                        break;
                                    case '[':
                                        AddToTokenList(new Token('[', readLineNum, readColNum));
                                        break;
                                    case ']':
                                        AddToTokenList(new Token(']', readLineNum, readColNum));
                                        break;
                                    case '{':
                                        AddToTokenList(new Token('{', readLineNum, readColNum));
                                        break;
                                    case '}':
                                        AddToTokenList(new Token('}', readLineNum, readColNum));
                                        break;
                                    case ',':
                                        AddToTokenList(new Token(',', readLineNum, readColNum));
                                        break;
                                    case '\n'://换行字符，过滤，行号加1；
                                        
                                        break;
                                    case ' '://空格格式字符，过滤
                                        break;
                                    case '\t'://tab
                                        break;
                                    default://出错 其它非法字符
                                        ErrorRecord(LexErrorType.IllegalCharacters,readLineNum,readColNum);
                                        break;
                                }
                                currentState = StateType.DONE;//当前进入结束状态
                            }
                            break;
                        case StateType.ID://结束时多读取了一个字符，应该有回退处理机制
                            String lexeme = "";
                            //生成字符串
                            lexeme += curChar;
                            while ( (cur = GetNextFromBuffer()) != -1/*结束*/)
                            {
                                curChar = (char)cur;
                                if (Char.IsLetter(curChar) == false)
                                {
                                    break;
                                }
                                lexeme += curChar;
                            }
                            //回退
                            RollBack();
                            switch (lexeme)
                            {
                                case "else":
                                    AddToTokenList(new Word(lexeme, (int)TokenType.ELSE, readLineNum, (2 + readColNum - lexeme.Length)));
                                    break;
                                case "if":
                                    AddToTokenList(new Word(lexeme, (int)TokenType.IF, readLineNum, (2 + readColNum - lexeme.Length)));
                                    break;
                                case "int":
                                    AddToTokenList(new Word(lexeme, (int)TokenType.INT, readLineNum, (2 + readColNum - lexeme.Length)));
                                    break;
                                case "return":
                                    AddToTokenList(new Word(lexeme, (int)TokenType.RETURN, readLineNum, (2 + readColNum - lexeme.Length)));
                                    break;
                                case "while":
                                    AddToTokenList(new Word(lexeme, (int)TokenType.WHILE, readLineNum, (2 + readColNum - lexeme.Length)));
                                    break;
                                case "void":
                                    AddToTokenList(new Word(lexeme, (int)TokenType.VOID, readLineNum, (2 + readColNum - lexeme.Length)));
                                    break;
                                case "read":
                                    AddToTokenList(new Word(lexeme, (int)TokenType.READ, readLineNum, (2 + readColNum - lexeme.Length)));
                                    break;
                                case "write":
                                    AddToTokenList(new Word(lexeme, (int)TokenType.WRITE, readLineNum, (2 + readColNum - lexeme.Length)));
                                    break;
                                default://出错 其它非法字符
                                    AddToTokenList(new Word(lexeme, (int)TokenType.ID, readLineNum, (2 + readColNum - lexeme.Length)));
                                    break;
                            }
                            currentState = StateType.DONE;

                            break;
                        case StateType.NUMBER://结束时多读取了一个字符，应该有回退处理机制
                            //记录数字长度
                            int i = 1;
                            Int32 value = 0;
                            //转为数字
                            value = Convert.ToInt32(curChar.ToString());

                            while ((cur = GetNextFromBuffer()) != -1/*结束*/)
                            {
                                curChar = (char)cur;
                                if (Char.IsDigit(curChar) == false)
                                {
                                    break;
                                }
                                ++i;
                                value *= 10;
                                value += Convert.ToInt32(curChar.ToString());
                            }
                            //回退
                            RollBack();
                            AddToTokenList(new Number(value, readLineNum,(2+ readColNum-i)));
                            currentState = StateType.DONE;
                            break;
                        case StateType.NIUBISTATE:
                            cur = GetNextFromBuffer();
                            if (cur != -1)
                            {
                                curChar = (char)cur;
                            }
                            else {//结束   除号后面没其他东西 
                                AddToTokenList(new Token('/', readLineNum, readColNum));//除号
                                currentState = StateType.DONE;
                                return ;
                            }
                            if (curChar == '*')//刚好处理完，没多读取字符
                            {
                                currentState = StateType.COMMENT1;//进入注释状态，已经读取了符号 "/*"
                            }
                            else if (Char.IsLetter(curChar) || Char.IsDigit(curChar) || curChar == ' ')// “/”号后面是数字，字母，空格
                            {//结束时多读取了一个字符，应该有回退处理机制
                                RollBack();
                                AddToTokenList(new Token('/', readLineNum, readColNum));//除号
                                currentState = StateType.DONE;
                            }
                            else
                            {//出错 除号后面是其他错误符号
                                ErrorRecord(LexErrorType.SlashError,readLineNum, readColNum);
                                currentState = StateType.DONE;

                            }
                            break;
                        case StateType.ASSIGN:
                            cur = GetNextFromBuffer();
                            if (cur != -1)
                            {
                                curChar = (char)cur;
                            }
                            else
                            {//结束   等于号后面没其他东西 
                                AddToTokenList(new Token('=', readLineNum, readColNum));
                                //
                                currentState = StateType.DONE;
                                return;
                            }
                            if (curChar == '=')
                            {//刚好处理完，没多读取字符
                                AddToTokenList(new Word("==", (int)TokenType.EQUAL, readLineNum, readColNum));
                                //
                                currentState = StateType.DONE;
                               // return;
                            }
                            else if (Char.IsLetter(curChar) || Char.IsDigit(curChar) || curChar == ' ')// “=”号后面是数字，字母，空格
                            { //结束时多读取了一个字符，应该有回退处理机制
                                RollBack();
                                AddToTokenList(new Token('=', readLineNum, readColNum));
                                //
                                currentState = StateType.DONE;
                            }
                            else //出错 等于号后面是其他错误符号
                            {
                                ErrorRecord(LexErrorType.EqualError,readLineNum,readColNum);
                                currentState = StateType.DONE;
                            }
                            break;
                        case StateType.NOT:
                            cur = GetNextFromBuffer();
                            if (cur != -1)
                            {
                                curChar = (char)cur;
                            }
                            else
                            {//错误   感叹号后面是其他错误符号
                                ErrorRecord(LexErrorType.ExclamationError,readLineNum,readColNum);
                                currentState = StateType.DONE;
                            }
                            if (curChar == '=')
                            {
                                AddToTokenList(new Word("!=", (int)TokenType.NOTEQUAL, readLineNum, readColNum));
                                currentState = StateType.DONE;
                            }
                            else
                            {//出错 感叹号后面是其他错误符号
                                ErrorRecord(LexErrorType.ExclamationError, readLineNum, readColNum);
                                currentState = StateType.DONE;
                            }
                            break;
                        case StateType.GREATERORLESS:
                            lexeme = "";
                            lexeme += curChar;
                            cur = GetNextFromBuffer();
                            if (cur != -1)
                            {
                                curChar = (char)cur;
                            }
                            else
                            {
                            //结束   大于，小于号后面没其他东西 
                                AddToTokenList(new Token(lexeme[0], readLineNum, readColNum));
                                currentState = StateType.DONE;
                                return;
                            }
                            if (curChar == '=')
                            {
                                lexeme += curChar;
                                if (lexeme == ">=")
                                    AddToTokenList(new Word(lexeme, (int)TokenType.GREATERQUAL, readLineNum, readColNum));
                                else
                                    AddToTokenList(new Word(lexeme, (int)TokenType.LESSEQUAL, readLineNum, readColNum));
                                currentState = StateType.DONE;
                            }
                            else if (Char.IsLetter(curChar) || Char.IsDigit(curChar) || curChar == ' ')// “=”号后面是数字，字母，空格
                            { //结束时多读取了一个字符，应该有回退处理机制
                                RollBack();
                                AddToTokenList(new Token(lexeme[0], readLineNum, readColNum));
                                currentState = StateType.DONE;
                            }
                            else
                            {//出错 大于，小于号后面是其他错误符号
                                currentErrorString =""+ lexeme[0];
                                ErrorRecord(LexErrorType.GreaterLessError,readLineNum,readColNum);
                                currentState = StateType.DONE;
                            }
                            break;
                        case StateType.COMMENT1:
                            while ((cur = GetNextFromBuffer()) != -1/*结束*/)
                            {
                                curChar = (char)cur;
                                if (curChar == '*')
                                {
                                    break;
                                }
                            }
                            if (cur == -1) { //出错 “/*”后面没有跟着 “*/”
                                ErrorRecord(LexErrorType.Comment1Error,readLineNum,readColNum);
                                currentState = StateType.DONE;
                            }
                            //是*号；
                            currentState = StateType.COMMENT2;
                            break;
                        case StateType.COMMENT2:
                           
                            if ((cur = GetNextFromBuffer()) != -1/*结束*/)
                            {
                                curChar = (char)cur;
                                if (curChar == '/')
                                {
                                    currentState = StateType.DONE;
                                }
                                else if (curChar == '*')
                                    currentState = StateType.COMMENT2;
                                else
                                    currentState = StateType.COMMENT1;
                            }
                            if (cur == -1)
                            { //出错 “/**”后面没有跟着 “/”
                                ErrorRecord(LexErrorType.Comment2Error, readLineNum, readColNum);
                                currentState = StateType.DONE;
                            }
                            break;
                        default:
                            //出错 其它非法字符
                            ErrorRecord(LexErrorType.IllegalCharacters, readLineNum, readColNum);
                            currentState = StateType.DONE;
                            break;
                    }
                }
            }

            AddToTokenList(new Word("EOF", (int)TokenType.EOF, readLineNum, readColNum));
        }
        #endregion
    }
}
