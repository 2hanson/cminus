using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.ParseRecursive
{
    public class Parser
    {
        #region
        // current scope
        string scope;		
        //当前的token
        Token currentToken;
        //typeToken
        Token typeToken;
        bool _good = false;
        //idnameToken
        Token idNameToken;
        TreeNode root;
        public TreeNode __program;
        List<Token> tokenList;
        //当前token的编号
        int currentTokenIndex;
        //错误信息
        String currentErrorString = "";
        //当前字符变量
        char curChar;

        #endregion


        /// <summary>
        /// 构造函数，传入词法的tokenlist
        /// </summary>
        /// <param name="tokenList"></param>
        public Parser(List<Token> ttokenList)
        {
            _good = false;
            this.tokenList = ttokenList;
            root = null;
            currentTokenIndex = -1;
        }

        private void ErrorRecord(ParseErrorType parseErrorType)
        {
            ErrorBase.ErrorManager.AddParseError(new ParseError(parseErrorType, currentToken.LineNumber , currentToken.ColumnNumber, currentToken));
        }

        /// <summary>
        /// 取下一个token
        /// </summary>
        /// <returns></returns>
        private Token GetNextToken()
        {
            ++currentTokenIndex;
            if (currentTokenIndex == tokenList.Count)
            {
                //Token endOfFile = new Token();
                return null;
            }
            return tokenList[currentTokenIndex];
        }

        /// <summary>
        ///  build parse tree
        /// </summary>
        public TreeNode BuildSyntaxTree()
        {
            root = new TreeNode(NodeKind.proK, TokenType.NONE , "Root", 0, "global");
            TreeNode p = null;
            scope = "global";
            p = root.Child[0] = Program();
            if (p != null)
                p.Father = root;
            //
            while (p != null && p.Sibling != null)
            {
                p = p.Sibling;
                p.Father = root;
            }
            return root;
        }

        public bool is_good()
        {
            return _good;
        }

        /// <summary>
        /// 
        /// </summary>
        void PrintTree()
        { 
            
        }

        /// <summary>
        /// 1. program -> delaration_list
        /// </summary>
        /// <returns></returns>
        TreeNode Program()
        {
            return DeclarationList();
        }


        /// <summary>
        /// 2. declaration-list → declaration-list declaration | declaration
        /// </summary>
        /// <returns></returns>
        TreeNode DeclarationList()
        {
            TreeNode program = null;
            TreeNode last = null;
            TreeNode temp = null;
            currentToken = GetNextToken();
            while (!CheckCurrentToken(TokenType.EOF)) { 
                if (!CheckCurrentToken(TokenType.INT)&&
                    !CheckCurrentToken(TokenType.VOID) &&!CheckCurrentToken(';'))
                {
                    ErrorRecord(ParseErrorType.InvalidType);
                    ConsumeUntil(";","}");
                    //错误处理
                    //scan->add_err();
                    //sprintf(msg_temp, "Syntax error: invalid type \'%s\'",
                    //    cToken.str.c_str());
                    //outputMsg(scan->lineno(), msg_temp);
                    //consumeUntil(SEMI, RBRACE); // ';', '}'
                }
                else if ((temp = Declaration()) != null)
                {
                    if (program == null)
                    {
                        program = temp;
                        last = temp.LastSibling();
                    }
                    else
                    {
                        last.Sibling = temp;
                        last = temp.LastSibling();
                    }
                }
                currentToken = GetNextToken();
            }
            return program;
        }

        private void ConsumeUntil(string type1, string type2)
        {
            while (currentToken.TokenInfo != type1 && currentToken.TokenInfo != type2 && currentToken.TokenInfo != "EOF")
                currentToken = GetNextToken();
        }


        /// <summary>
        /// 3. declaration → var-declaration | fun-declaration
        /// </summary>
        /// <returns></returns>
        TreeNode Declaration()
        {
            scope = "global";	// use global fun or var
            TreeNode temp = null;
            typeToken = currentToken;
            currentToken = GetNextToken();
            idNameToken = currentToken;
            
            if (idNameToken.TokenTypes != Convert.ToInt32(TokenType.ID))
            {
                ErrorRecord(ParseErrorType.ReservedToken);
                ConsumeUntil(";", "}");
                //错误处理
                //scan->add_err();
                //sprintf(msg_temp, "Syntax error: %s is a reserved token", iToken.str.c_str());
                //outputMsg(scan->lineno(), msg_temp);
                //consumeUntil(SEMI, RBRACE);
            }
            else {
                currentToken = GetNextToken();// expected a '(', '[' or  ','
                if (CheckCurrentToken('(')) {
                    // '(' is fun declaration
                    temp = FunDeclaration();
                }
                else if (CheckCurrentToken(';')||
                    CheckCurrentToken(',')||
                    CheckCurrentToken('[')) 
                {
                    temp = VarDeclaration();
                }
                else {
                    ErrorRecord(ParseErrorType.MissingSemi);
                    ConsumeUntil(";", "}");
                    //错误处理
                    //scan->add_err();
                    //sprintf(msg_temp, "Syntax error: missing \';\' after indentifer %s",iToken.str.c_str());
                    //outputMsg(scan->lineno(), msg_temp);
                    //consumeUntil(SEMI, RBRACE);
                }
            }
            return temp;  
        }
       
        /// <summary>
        ///  4. var-declaration → type-specifier ID(,...); | type-specifier ID [ NUM ](,...);
        ///  5. type-specifier → int | void
        /// </summary>
        /// <returns></returns>
        TreeNode VarDeclaration()
        {
            // now, cToken.str == ";", "," or "["
            Word iToken = idNameToken as Word;
            TreeNode temp = new TreeNode(NodeKind.varK, (TokenType)typeToken.TokenTypes, iToken.Lexeme, currentToken.LineNumber, scope);
            if (CheckCurrentToken('[')) {
                currentToken = GetNextToken();//must NUMber
                if (!CheckCurrentToken(TokenType.NUMBER)) {
                    ErrorRecord(ParseErrorType.MissingArraySize);
                    ConsumeUntil(";", "}");
                    //error
                    /*scan->add_err();
                    sprintf(msg_temp,
                        "Syntax error: missing array size in declaration of array %s[]",
                        iToken.str.c_str());
                    outputMsg(scan->lineno(), msg_temp);
                    delete temp;
                    consumeUntil(SEMI, RBRACE);	// error recovery in global declaration

                    return NULL;*/
                    return null;
                }
                temp.IsArray = true;
                Number num = currentToken as Number;
                temp.ArraySize = Convert.ToInt32(num.Lexeme);
                if (!Match(Convert.ToInt32(']'))) {
                    ErrorRecord(ParseErrorType.MissingRBracketInArray);
                    --currentTokenIndex;
                    // need ']'
                    /*
                    scan->add_err();
                    sprintf(msg_temp,
                        "Syntax error: missing \']\' in declaration of array %s[]",
                        iToken.str.c_str());
                    scan->push();	
                    */
                }
                currentToken = GetNextToken();//  ";" or ","
            }
            if (CheckCurrentToken(',')) {
                idNameToken = currentToken = GetNextToken();//next id
                if (idNameToken.TokenTypes != Convert.ToInt32( TokenType.ID)) { 
                    //错误处理
                    ErrorRecord(ParseErrorType.ReservedToken);
                    ConsumeUntil(";", "}");
                    return temp;
                }
                currentToken = GetNextToken();//expected ";", ",", "["
                if (CheckCurrentToken(';')||
                    CheckCurrentToken(',') ||
                   CheckCurrentToken('['))
                {
                    temp.Sibling = VarDeclaration();
                }
                else { 
                    //错误处理
                    ErrorRecord(ParseErrorType.MissingSemi);
                    --currentTokenIndex;                  
                    return temp;
                }
            }
            else if ( !CheckCurrentToken(';')) {
                //错误处理
                ErrorRecord(ParseErrorType.MissingSemiAfterDeclarationSequence);
                ConsumeUntil(";", "}");         
            }
            return temp;
        }

        /// <summary>
        ///  6. fun-declaration → type-specifier ID ( params )  compound-stmt
        /// </summary>
        /// <returns></returns>
        TreeNode FunDeclaration()
        {
            Word iToken = idNameToken as Word;
            TreeNode temp = new TreeNode(NodeKind.funK, (TokenType)typeToken.TokenTypes, iToken.Lexeme, currentToken.LineNumber, scope);
            TreeNode p = null;
            scope = iToken.Lexeme;
            p = temp.Child[0] = Params();

            if (p != null)
                p.Father = temp;
            //
            while (p != null && p.Sibling != null) {
                p = p.Sibling;
                p.Father = temp;
            }
            //scan->push();
            //--currentTokenIndex;//调了N久

            if (!Match(Convert.ToInt32(')')))
            {
                ErrorRecord(ParseErrorType.MissingRParenthesisInFunction);
                --currentTokenIndex;
                //error
                //scan->add_err();
                //sprintf(msg_temp,
                //    "Syntax error: missing \')\' in function %s(...) declaration",
                //    iToken.str.c_str());
                //outputMsg(scan->lineno(), msg_temp);
                //scan->push();

            }
            // compound statements
            p = temp.Child[1] = CompoundStmt();
            if (p != null)
                p.Father = temp;
            while (p != null && p.Sibling!=null)
            {
                p = p.Sibling;
                p.Father = temp;
            }
            return temp;
        }

        /// <summary>
        ///  7. params → params-list | void
        ///  8. param-list→ param-list, param | param
        ///  9. param→ type-specifier ID | type-specifier ID [ ]
        /// </summary>
        /// <returns></returns>
        TreeNode Params()
        {
            TreeNode first = null;
            TreeNode temp = null;
            typeToken = currentToken = GetNextToken();
            if (CheckCurrentToken(')')) {
                --currentTokenIndex;//scan->push();
                return temp;//null
            }
            if (typeToken.TokenTypes == Convert.ToInt32(TokenType.VOID))
            { 
                if (Match(Convert.ToInt32( ')')))
		        {
                    --currentTokenIndex;
			        return temp;	// NULL
		        }
		        else
		        {
                    --currentTokenIndex;//scan->push();	// not ")"
		        }
	        }
            while (typeToken.TokenTypes == Convert.ToInt32(TokenType.INT) ||
                typeToken.TokenTypes == Convert.ToInt32(TokenType.VOID)) 
            {
                idNameToken = currentToken = GetNextToken();
                if (idNameToken.TokenTypes != Convert.ToInt32(TokenType.ID))
                {
                    //error
                    ErrorRecord(ParseErrorType.InvalidParameter);
                }
                else {
                    Word iToken = idNameToken as Word;
                    temp = new TreeNode(NodeKind.paramK, (TokenType)typeToken.TokenTypes, iToken.Lexeme, currentToken.LineNumber, scope);
                    temp.Sibling = first;
                    first = temp;
                }
                currentToken = GetNextToken();
                if (CheckCurrentToken('[')) {
                    temp.IsArray = true;
                    if (!Match(Convert.ToInt32(']')))
                    {
                        //错误处理
                        ErrorRecord(ParseErrorType.MissingArrayParameter);
                        ConsumeUntil(";", ")");
                    }
                    else {
                        currentToken = GetNextToken();//','
                    }
                }
                if (CheckCurrentToken(')')) {//')'
                    break;//')'
                }
                else if (CheckCurrentToken(','))//','
                {
                    typeToken = currentToken = GetNextToken();
                }
                else {
                    break;
                }
            }
            --currentTokenIndex; //next is ')'
            return first;
        }

        /// <summary>
        /// 10. compound-stmt → { local-declarations statement-list }
        /// </summary>
        /// <returns></returns>
        TreeNode CompoundStmt()
        {
            TreeNode first = null;
            TreeNode last = null;
            TreeNode temp = null;
            bool noBraces = false;
            if (!Match(Convert.ToInt32('{')))
            {// match "{"
                noBraces = true;
                --currentTokenIndex;
            }
            // local-declarations
            while (true)
            {

                typeToken = currentToken = GetNextToken();
                if (CheckCurrentToken(TokenType.INT) ||
                    CheckCurrentToken(TokenType.VOID))
                {
                    temp = LocalDeclarations();
                }
                else
                {
                    --currentTokenIndex;
                    break;
                }
                if (noBraces)
                    return temp;
                if (temp != null)
                {
                    if (first == null)
                    {
                        first = temp;
                        last = temp.LastSibling();
                    }
                    else
                    {
                        last.Sibling = temp;
                        last = temp.LastSibling();
                    }
                }
            }
            
            //currentToken = GetNextToken();
            currentToken = GetNextToken();
            while (true) {
                temp = null;
                if (CheckCurrentToken('}')) {
                    if (noBraces) { 
                        //错误处理
                        ErrorRecord(ParseErrorType.UnpairedRBrace);
                    }
                    break;
                }
                if (CheckCurrentToken(TokenType.EOF)) { 
                    //错误处理
                    ErrorRecord(ParseErrorType.MissingRBraceBeforeEOF);
                    --currentTokenIndex;
                    break;
                }
                switch (currentToken.TokenTypes) { 
                    case (Int32)TokenType.READ:
                        temp = ReadStmt();
                        break;
                    case (Int32)TokenType.WRITE:
                        temp = WriteStmt();
                        break;
                    case (Int32)(';'):
                    case (Int32)TokenType.NUMBER:
                    case (Int32)('('):
                        temp = ExpressionStmt();
                        break;
                    case (Int32)TokenType.ID:
                        temp = SubcompoundStmt();
                        break;
                    case (Int32)TokenType.IF:
                        temp = IfStmt();
                      //  currentToken = GetNextToken();
                        break;
                    case (Int32)TokenType.WHILE:
                        temp = WhileStmt();
                        break;
                    case (Int32)TokenType.RETURN:
                        temp = ReturnStmt();
                        break;
                    case (Int32)TokenType.ELSE:
                        //错误处理
                        ErrorRecord(ParseErrorType.UnpairedElseStatement);
                        ConsumeUntil(";", "}");
                        //scan->add_err();
                        //outputMsg(scan->lineno(), " unpaired \'else\' statement");
                        //consumeUntil(SEMI, RBRACE);
                        break;
                    default:
                        //错误处理
                        ErrorRecord(ParseErrorType.UndefinedSymbol);
                        ConsumeUntil(";", "}");

                        //scan->add_err();
                        //sprintf(msg_temp, "Syntax error: undefined symbol \"%s\"", cToken.str.c_str());
                        //outputMsg(scan->lineno(), msg_temp);
                        //consumeUntil(SEMI, RBRACE);
                        break;
                }

                if (noBraces)
                    return temp;//// no braces
                if (temp != null) {
                    if (first == null)
                    {
                        first = temp;
                        last = temp.LastSibling();
                    }
                    else {
                        last.Sibling = temp;
                        last = temp.LastSibling();
                    }
                }

                currentToken = GetNextToken();
            }
            return first;
        }

        /// <summary>
        /// 11. local-declarations→ local-declarations var-declaration | empty
        /// </summary>
        /// <returns></returns>
        TreeNode LocalDeclarations()
        {
            TreeNode temp = null;
            idNameToken = currentToken = GetNextToken();
            if (idNameToken.TokenTypes != Convert.ToInt32(TokenType.ID))
            {
                //错误处理
                ErrorRecord(ParseErrorType.ReservedToken);
                ConsumeUntil(";");
                return null;
            }

            currentToken = GetNextToken();//';', '[', ','

            if (CheckCurrentToken(';') || CheckCurrentToken('[') || CheckCurrentToken(','))
            {
                temp = VarDeclaration();
            }
            else
            {
                //错误处理
                ErrorRecord(ParseErrorType.MissingSemi);
                --currentTokenIndex;
            }
            return temp;
        }

        private void ConsumeUntil(string type)
        {
            while (currentToken.TokenInfo != type && currentToken.TokenInfo != "EOF")
                currentToken = GetNextToken();
        }

        /// <summary>
        /// 12. statement-list → statement-list statement | empty
        /// 13. statement → expression-stmt | compound-stmt | selection-stmt                             
        ///    | iteration-stmt | return-stmt 
        /// 14. expression-stmt --> expression; | ;
        /// </summary>
        /// <returns></returns>
        TreeNode ExpressionStmt()
        {
            // cToken == ( / k_ID / k_NUM / ;

            if (CheckCurrentToken(';'))
                return null;
            TreeNode temp = Expression();
            if (!Match(';'))
            {
                //错误处理
                ErrorRecord(ParseErrorType.MissingSemiOnly);
                --currentTokenIndex;
            }

            return temp;
        }

        /// <summary>
        ///  15. selection-stmt → if (expression ) s tatement
        ///  | if ( expression ) statement else statement 
        /// </summary>
        /// <returns></returns>
        TreeNode IfStmt()
        {
            Word CToken = currentToken as Word;
            TreeNode temp = new TreeNode(StmtKind.ifK,CToken.Lexeme,currentToken.LineNumber,scope);
            TreeNode p = null;
            if (!Match('('))
            { // if (
                //错误处理
                ErrorRecord(ParseErrorType.MissingLParenthesisInIf);
                //scan->add_err();
                //outputMsg(scan->lineno(), "Syntax error: missing \'(\' in \"if\" statement");
            }
            else
            {
                currentToken = GetNextToken();
            }

            temp.Child[0] = Expression();

            if (temp.Child[0] != null)
                temp.Child[0].Father = temp;
            if (!Match(')')) { 
                //错误处理
                ErrorRecord(ParseErrorType.MissingRParenthesisInIf);
            }

            p = temp.Child[1] = CompoundStmt();
            if (p != null)
                p.Father = temp;
            while (p != null && p.Sibling != null) {
                p = p.Sibling;
                p.Father = temp;
            }

            currentToken = GetNextToken();
            if (CheckCurrentToken(TokenType.ELSE))
            {
                p = temp.Child[2] = CompoundStmt();
                if (p != null) p.Father = temp;
                while (p!=null && p.Sibling!=null)
                {
                    p = p.Sibling;
                    p.Father = temp;
                }
            }
            else {
                // if has no 'else' statement, push the next token back
                --currentTokenIndex;
            }

            return temp;
        }

        /// <summary>
        /// 16. iteration-stmt → while ( expression ) statement
        /// </summary>
        /// <returns></returns>
        TreeNode WhileStmt()
        {
            Word CToken = currentToken as Word;
            TreeNode temp = new TreeNode(StmtKind.whileK,CToken.Lexeme,currentToken.LineNumber,scope);
            TreeNode p = null;
            if (!Match('('))
            {
                ErrorRecord(ParseErrorType.MissingLParenthesisInWhile);
                /*
                // while (
                scan->add_err();	// add error count,
                // out put the error message
                outputMsg(scan->lineno(), "Syntax error: missing \'(\' in \"while\" statement");
            
                 */
            }
            else {
                currentToken = GetNextToken();
            }

            // cToken should be the first token of expression now ...
            temp.Child[0] = Expression();
            if (temp.Child[0] != null)
                temp.Child[0].Father = temp;
            if (!Match(')')) { 
                //while()
                //错误处理
                ErrorRecord(ParseErrorType.MissingRParenthesisInWhile);
            }

            p = temp.Child[1] = CompoundStmt();
            if (p != null)
                p.Father = temp;
            while (p != null && p.Sibling != null) {
                p = p.Sibling;
                p.Father = temp;
            }
            return temp;
        }

        /// <summary>
        /// 17. return-stmt → return; | return expression;
        /// </summary>
        /// <returns></returns>
        TreeNode ReturnStmt()
        {
            // cToken.str == "return"
            Word NToken = currentToken as Word;
            TreeNode temp = new TreeNode(StmtKind.returnK, NToken.Lexeme, currentToken.LineNumber, scope);
            currentToken = GetNextToken();

            //TreeNode temp11 = new TreeNode(StmtKind.returnK, NToken.Lexeme, currentToken.LineNumber, scope);

            //TreeNode temp1 = new TreeNode(StmtKind.returnK, NToken.Lexeme, currentToken.LineNumber, scope);
            if (!CheckCurrentToken(';')) {
                temp.Child[0] = Expression();
                if (!Match(';')) { 
                    //错误处理
                    ErrorRecord(ParseErrorType.MissingSemiInReturn);
                    --currentTokenIndex;
                }
            }
            return temp;
        }
        
        /// <summary>
        /// 18. expression → var = expression | simple-expression
        /// </summary>
        /// <returns></returns>
        private TreeNode Expression()
        {// cToken == (, k_ID, k_NUM
            TreeNode temp = SimpleExpression();
            TreeNode p = temp;			

            currentToken = GetNextToken();

            if (CheckCurrentToken('='))
            {
                if (temp.Type != TokenType.ID && (Int32)temp.Type != (Int32)('='))
                {
                    //错误处理
                    ErrorRecord(ParseErrorType.LeftID);
                    ConsumeUntil(";", ")");
                    return null;
                }
                p = new TreeNode(ExpKind.OpK, (TokenType)currentToken.TokenTypes, currentToken.TokenInfo, currentToken.LineNumber, scope);
                p.Child[0] = temp;
                if (p.Child[0] != null)
                    p.Child[0].Father = p;
                currentToken = GetNextToken();
                p.Child[1] = Expression();
                if (p.Child[1] != null)
                    p.Child[1].Father = p;
            }
            else {
                --currentTokenIndex;
            }
            return p;
        }

        /// <summary>
        /// 19. var → ID | ID [ expression ]
        /// </summary>
        /// <returns></returns>
        TreeNode Var()
        {
            Word iToken = idNameToken as Word;
            TreeNode temp = new TreeNode(ExpKind.IDK, (TokenType)currentToken.TokenTypes, iToken.Lexeme, currentToken.LineNumber, scope);
            // "["
            currentToken = GetNextToken();
            if (CheckCurrentToken('['))
            {
                temp.IsArray = true;
                currentToken = GetNextToken();
                temp.Child[0] = Expression();

                if (!Match(']'))
                {
                    ErrorRecord(ParseErrorType.MissingRBracket);
                    --currentTokenIndex;
                    //scan->add_err();
                    //outputMsg(scan->lineno(), "Syntax error: missing \']\'");
                    //scan->push();
                }
                //改动，不把数组的大小变成一个单独的节点。
                temp.ArraySize = Convert.ToInt16(temp.Child[0].Name.ToString());
                temp.Child[0] = null;
            }
            else
            {
                --currentTokenIndex;
            }

            return temp;
        }

        /// <summary>
        /// 20. simple-expression → additive-expression relop additive-expression
        /// |additive-expression
        /// 21. relop →<= | < | > | >= | == | !=
        /// </summary>
        /// <returns></returns>
        private TreeNode SimpleExpression()
        {
            TreeNode p = AdditiveExpression();
            currentToken = GetNextToken();
            if (CheckCurrentToken(TokenType.LESSEQUAL) ||
                CheckCurrentToken('<') || CheckCurrentToken('>') ||
                CheckCurrentToken(TokenType.GREATERQUAL) || CheckCurrentToken(TokenType.EQUAL) ||
                CheckCurrentToken(TokenType.NOTEQUAL))
            {
                TreeNode temp = new TreeNode(ExpKind.OpK, (TokenType)currentToken.TokenTypes, currentToken.TokenInfo, currentToken.LineNumber, scope);
                temp.Child[0] = p;
                p = temp;
                if (p.Child[0] != null)
                    p.Child[0].Father = p;
                currentToken = GetNextToken();
                p.Child[1] = AdditiveExpression();
                if (p.Child[1] != null) {
                    p.Child[1].Father = p;
                }
            }
            else {
                --currentTokenIndex;
            }
            return p;
        }

        /// <summary>
        /// 22. additive-expression → additive-expression addop term | term
        /// 23. addop →+ | -
        /// </summary>
        /// <returns></returns>
        private TreeNode AdditiveExpression()
        {
            TreeNode p = Term();
            currentToken = GetNextToken();
            while (CheckCurrentToken('+') || CheckCurrentToken('-')) {
                TreeNode temp = new TreeNode(ExpKind.OpK, (TokenType)currentToken.TokenTypes, currentToken.TokenInfo, currentToken.LineNumber, scope);
                temp.Child[0] = p;
                p = temp;
                if (p.Child[0] != null)
                    p.Child[0].Father = p;
                currentToken = GetNextToken();
                p.Child[1] = Term();
                if (p.Child[1] != null) {
                    p.Child[1].Father = p;
                }
                currentToken = GetNextToken();
            }

            --currentTokenIndex;
            return p;
        }

        /// <summary>
        ///  24. term → term mulop factor | factor
        ///  25. mulop →* | /
        /// </summary>
        /// <returns></returns>
        private TreeNode Term()
        {
            TreeNode p = Factor();
            currentToken = GetNextToken();
            while (CheckCurrentToken('*') || CheckCurrentToken('/') || CheckCurrentToken('%')) {

                TreeNode temp = new TreeNode(ExpKind.OpK, (TokenType)currentToken.TokenTypes, currentToken.TokenInfo, currentToken.LineNumber, scope);
                temp.Child[0] = p;
                p = temp;
                if (p.Child[0] != null)
                    p.Child[0].Father = p;
                currentToken = GetNextToken();
                p.Child[1] = Factor();
                if (p.Child[1] != null)
                {
                    p.Child[1].Father = p;
                }
                currentToken = GetNextToken();
            }
            --currentTokenIndex;
            return p;
        }

        /// <summary>
        /// 26. factor → ( expression ) | var | call | NUM
        /// </summary>
        /// <returns></returns>
        private TreeNode Factor()
        {
            TreeNode temp = null;
            switch (currentToken.TokenTypes)
            {
                  
                case (Int32)TokenType.NUMBER:
                    Number NToken = currentToken as Number;
                    temp = new TreeNode(ExpKind.ConstK, (TokenType)currentToken.TokenTypes, NToken.Lexeme , currentToken.LineNumber, scope);
                    break;
                case (Int32)TokenType.ID:
                    idNameToken = currentToken;
                    currentToken = GetNextToken();
                    if (CheckCurrentToken('(')) temp = Call();
                    else
                    {
                        --currentTokenIndex;
                        temp = Var();
                    }
                    break;
                case (Int32)('('):
                    currentToken = GetNextToken();
                    temp = Expression();
                    if (!Match(')'))
                    {
                        ErrorRecord(ParseErrorType.MissingRParenthesis);
                        --currentTokenIndex;
                        //scan->add_err();
                        //outputMsg(scan->lineno(), "Syntax Error: missing \')\'");
                        //scan->push();
                    }
                    break;
                default:
                    ErrorRecord(ParseErrorType.ExpressionIsUnexpected);
                    ConsumeUntil(";", "}");
                    //scan->add_err();
                    //sprintf(msg_temp, "Syntax error: \'%s\' expression is unexpected!", cToken.str.c_str());
                    //outputMsg(scan->lineno(), msg_temp);
                    //consumeUntil(SEMI, RBRACE);
                    break;
            }

            return temp;
        }

        /// <summary>
        ///  27. call → ID ( args )
        /// </summary>
        /// <returns></returns>
        private TreeNode Call()
        {
            Word iToken = idNameToken as Word;
            TreeNode p = new TreeNode(StmtKind.callK, iToken.Lexeme, currentToken.LineNumber, scope);
            TreeNode temp = null;
            p.Scope = "global";

            p.Child[0] = Args();
            if (p.Child[0] != null) {
                p.Child[0].Father = p;
            }

            temp = p.Child[0];
            while (temp != null && temp.Sibling != null) {
                temp = temp.Sibling;
                temp.Father = p;
            }
            if (!Match(')')) { 
                //错误处理
                ErrorRecord(ParseErrorType.MissingRParenthesis);
                --currentTokenIndex;
            }
            return p;
        }

        /// <summary>
        /// 28. args → arg-list | empty & 29. arg-list → arg-list , expression | expression
        /// </summary>
        /// <returns></returns>
        private TreeNode Args()
        {
            TreeNode first = null;
            TreeNode temp = null;
            currentToken = GetNextToken();
            if (CheckCurrentToken(')')) {
                --currentTokenIndex;
                return null;
            }
            while (true) {
                if ((temp = Expression()) != null)
                {
                    if (first == null) first = temp;
                    else
                    {
                        temp.Sibling = first;
                        first = temp;
                    }
                }

                currentToken = GetNextToken();
                if (currentToken.TokenTypes == Convert.ToInt32(','))
                {
                    currentToken = GetNextToken();
                }
                else
                {
                    break;
                }
            }
            --currentTokenIndex;
            return first;
        }

        /// <summary>
        /// 30. read → read( int ); 
        /// </summary>
        /// <returns></returns>
        TreeNode ReadStmt()
        {
            // currentToken.str == "read"
            TreeNode temp = null;
            if (!Match('('))
            {
                ErrorRecord(ParseErrorType.MissingLParenthesis);
                ConsumeUntil(";","}");
                //scan->add_err();
                //outputMsg(scan->lineno(), "Syntax error: missing \'(\'");
                //consumeUntil(SEMI, RBRACE);
                return null;
            }

            idNameToken = currentToken = GetNextToken();
            if (!CheckCurrentToken(TokenType.ID))
            {
                //错误处理
                ErrorRecord(ParseErrorType.BadArgument);
                ConsumeUntil(";", "}");
                return null;
            }
            temp = new TreeNode(StmtKind.readK, "read", currentToken.LineNumber, scope);
            if ((temp.Child[0] = Var()) != null)
            {
                temp.Child[0].Father = temp;
            }

            if (!Match(')'))
            {
                ErrorRecord(ParseErrorType.MissingRParenthesis);
                ConsumeUntil(";", "}");
                //scan->add_err();
                //outputMsg(scan->lineno(), "Syntax error: missing \')\'");
                //consumeUntil(SEMI, RBRACE);
                return temp;
            }

            if (!Match(';'))
            {
                ErrorRecord(ParseErrorType.MissingSemiOnly);
                --currentTokenIndex;
                //scan->add_err();
                //outputMsg(scan->lineno(), "Syntax error: missing \';\'");
                //scan->push();
            }

            return temp;
        }

        /// <summary>
        /// 31. write → write( int );
        /// </summary>
        /// <returns></returns>
        TreeNode WriteStmt()
        {
            TreeNode temp = null;
            if (!Match('('))
            {
                ErrorRecord(ParseErrorType.MissingRParenthesis);
                ConsumeUntil(";", "}");

                //scan->add_err();
                //outputMsg(scan->lineno(), "Syntax error: missing \'(\'");
                //consumeUntil(SEMI, RBRACE);
                return null;
            }

            temp = new TreeNode(StmtKind.writeK, "write", currentToken.LineNumber, scope);
            currentToken = GetNextToken();
            if ((temp.Child[0] = Expression()) != null)
            {
                temp.Child[0].Father = temp;
            }

            if (!Match(')'))
            {
                ErrorRecord(ParseErrorType.MissingRParenthesis);
                ConsumeUntil(";", "}");
                //scan->add_err();
                //outputMsg(scan->lineno(), "Syntax error: missing \')\'");
                //consumeUntil(SEMI, RBRACE);
                return temp;
            }

            if (!Match(';'))
            {
                ErrorRecord(ParseErrorType.MissingSemiOnly);
                --currentTokenIndex;
                //scan->add_err();
                //outputMsg(scan->lineno(), "Syntax error: missing \';\'");
                //scan->push();
            }

            return temp;
        }

        /// <summary>
        /// sub_compoundstmt → ID; | call; | expression_stmt
        /// </summary>
        /// <returns></returns>
        TreeNode SubcompoundStmt()
        {
            TreeNode temp = null;

            // cout << "Subcompound_stmt()" <<endl;
            idNameToken = currentToken;
            currentToken = GetNextToken();
            if (CheckCurrentToken('('))
            {	// call statement
                temp = Call();
                if (!Match(';'))
                {
                    ErrorRecord(ParseErrorType.MissingSemiAfterFunCall);
                    --currentTokenIndex;
                    //scan->add_err();
                    //outputMsg(scan->lineno(), "Syntax error: missing \';\' after fun call");
                    //scan->push();
                }
            }
            else
            {
                --currentTokenIndex;
                currentToken = idNameToken;
                temp = ExpressionStmt();
            }
            return temp;
        }


        /// <summary>
        /// 当前token与相应的tokentype比较，相等就返回true,否则false
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CheckCurrentToken(TokenType type)
        {
            //int testnum = Convert.ToInt32(type);
           
            if (currentToken.TokenTypes == Convert.ToInt32(type))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CheckCurrentToken(char type)
        {
            if (currentToken.TokenTypes == Convert.ToInt32(type))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// get next token and check if its type is expected..
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool Match(int type)
        {
            bool flag;
            currentToken = GetNextToken();
            flag = (currentToken.TokenTypes == type);
            //--currentTokenIndex;
            return flag;
        }

        bool Match(char ch)
        {
            int type = Convert.ToInt32(ch);
            bool flag;
            currentToken = GetNextToken();
            flag = (currentToken.TokenTypes == type);
            //--currentTokenIndex;
            return flag;
        }
    }
}
