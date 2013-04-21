using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.Parse
{
    public class Parser
    {
        Stack<LL1StackElement> elementStack = new Stack<LL1StackElement>();//符号栈
        Stack<TreeNode> treeStack = new Stack<TreeNode>();//语法树栈
        Stack<String> operatorStack = new Stack<String>();//操作符栈
        Stack<String> numStack = new Stack<String>();//操作数栈
        int currentTokenIndex;//当前token的编号
        Token currentToken;//当前token
        List<Token> tokenList;
        int currentLevelNum;//当前层
        String currentScope;//当前作用域

        /// <summary>
        /// 构造函数，获取从词法分析传过来的tokenlist
        /// </summary>
        /// <param name="tokenList"></param>
        public Parser(List<Token> tokenList) 
        {
            currentScope = "Global";
            this.tokenList = tokenList;
            currentTokenIndex = -1;
            currentLevelNum = 0; //初始化为0层
        }

        /// <summary>
        /// 取下一个token
        /// </summary>
        /// <returns></returns>
        private Token GetNextToken()
        {
            ++currentTokenIndex;
            if (currentTokenIndex == tokenList.Count)
                return null;
            return tokenList[currentTokenIndex];
        }

        public void ParseLL1(List<Token> tokenList)
        {
            ///*语法树的根节点*/
            TreeNode rootPointer;
            currentToken = GetNextToken();
            int pNum = 0; //纪录选中的产生式编号

            LL1Table.CreatLL1Table();

            ///*指向整个语法树根节点的指针，由它得到语法树*/
            rootPointer = OperateTree.NewRootNode();
            treeStack.Push(rootPointer);
            ///*从这里开始进行语法分析和语法树的生成*/
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Program,-1));
            
          
            /*取一个token*/
            int lineNum = currentToken.LineNumber;
            while (elementStack.Count != 0)
            {
                LL1StackElement element = (LL1StackElement)elementStack.Peek();
                
                /*检测终极符是否匹配*/
                if (element.Flag == false)
                {
                    if (Convert.ToInt16(element.TokenType) == currentToken.TokenTypes)
                    {
                        elementStack.Pop();
                        currentToken = GetNextToken();
                        if (currentToken == null) {//空
                            break;
                        }
                        //lineno = currentToken.lineshow;

                    }
                    else
                    {
                        //syntaxError("unexpected  token ->  ");
                        //fprintf(listing, "  %s", currentToken.Sem);
                        //fprintf(listing, "		");
                        ////printf("terminal not match!\n");
                        ////printf("%d\n",stacktopT);
                        //exit(0);
                    }
                }
                else
                {  /*根据非终极符和栈中符号进行预测*/
                    pNum = LL1Table.LL1table[Convert.ToInt16(element.NonTerminalType), currentToken.TokenTypes];
                    if (pNum == 39)
                    {
                        //向前看一个 读完i返回原处
                    }
                    else if (pNum == 7)
                    {
                        //向前看两个 读完i返回原处
                    }
                    
                    elementStack.Pop();
                    //	if (0==pnum)
                    //	{	printf("no predict!\n");
                    //        printf("%d\n",stacktopN);
                    //	}
                    Predict(pNum);

                    }
                }

                if (tokenList[i].TokenTypes != Convert.ToInt16(TokenType.EOF))
                {
                    //syntaxError("Code  ends  before  file \n");
                }
                //return rootPointer;
        }

        /// <summary>
        /// 选择产生式函数
        /// </summary>
        /// <param name="num"></param>
        private void Predict(int num)
        {
            switch (num)
            {
                case 0:
                case 1: Process1(); break;
                case 2: Process2(); break;
                case 3: Process3(); break;
                case 4: Process4(); break;
                case 5: Process5(); break;
                case 6: Process6(); break;
                case 7: Process7(); break;
                case 8: Process8(); break;
                case 9: Process9(); break;
                case 10: process10(); break;
                case 11: process11(); break;
                case 12: process12(); break;
                case 13: process13(); break;
                case 14: process14(); break;
                case 15: process15(); break;
                case 16: process16(); break;
                case 17: process17(); break;
                case 18: process18(); break;
                case 19: Process19(); break;
                case 20: Process20(); break;
                case 21: process21(); break;
                case 22: process22(); break;
                case 23: process23(); break;
                case 24: process24(); break;
                case 25: process25(); break;
                case 26: process26(); break;
                case 27: process27(); break;
                case 28: process28(); break;
                case 29: process29(); break;
                case 30: process30(); break;
                case 31: process31(); break;
                case 32: process32(); break;
                case 33: process33(); break;
                case 34: process34(); break;
                case 35: process35(); break;

                case 36: process36(); break;
                case 37: process37(); break;
                case 38: process38(); break;
                case 39: process39(); break;
                case 40: process40(); break;

                case 41: process41(); break;
                case 42: process42(); break;
                case 43: process43(); break;
                case 44: process44(); break;
                case 45: process45(); break;

                case 46: process46(); break;
                case 47: process47(); break;
                case 48: process48(); break;
                case 49: process49(); break;
                case 50: process50(); break;

                case 51: process51(); break;
                case 52: process52(); break;
                case 53: process53(); break;
                case 54: process54(); break;
                case 55: process55(); break;
                case 56: process56(); break;

                case 57: process57(); break;
                case 58: process58(); break;
                case 59: process59(); break;
                case 60: process60(); break;
                case 61: process61(); break;
                case 62: process62(); break;
                case 63: process63(); break;
                case 64: process64(); break;
                case 65: process65(); break;
                case 66: process66(); break;
                case 67: process67(); break;
                case 68: process68(); break;
                case 69: process69(); break;
                case 70: process70(); break;
                case 71: process71(); break;
                case 72: process72(); break;
                case 73: Process73(); break;
                case 74: Process74(); break;

                default:
                        //syntaxError("unexpected token ->");
                        //fprintf(listing, "  %s", currentToken.Sem);
                    break;
            }
        }

       /// <summary>
       /// 处理树栈。
       /// </summary>
       /// <param name="t"></param>
        private void OperateTreeStack(TreeNode t)
        {
            //取出栈顶元素
            TreeNode curTop = treeStack.Peek();
            if (curTop.LevelNum == currentLevelNum)//同一层
            { //同一层，兄弟节点
                //指向兄弟节点
                curTop.Sibling = t;
                treeStack.Pop();
                //增加儿子节点
                curTop = treeStack.Peek();
                curTop.AddChild(t);
                //curtop. = t;
                //指向父亲节点
                t.Father = curTop;
                treeStack.Push(t);
            }
            else if (curTop.LevelNum +1  == currentLevelNum) //当前元素是栈顶元素的下一层
            {
                curTop.AddChild(t);
                //curtop. = t;
                //指向父亲节点
                t.Father = curTop;
                treeStack.Push(t);
            }
            else if (curTop.LevelNum - 1 == currentLevelNum)//当前元素是栈顶元素的上一层
            {
                //儿子出栈
                treeStack.Pop();
                //当前节点的兄弟节点
                curTop = treeStack.Peek();
                //指向兄弟节点
                curTop.Sibling = t;
                treeStack.Pop();
                //增加儿子节点
                curTop = treeStack.Peek();
                curTop.AddChild(t);
                //curtop. = t;
                //指向父亲节点
                t.Father = curTop;
                treeStack.Push(t);
            }
        }
       
        /// <summary>
        /// program - > declaration-list 
        /// </summary>
        private void Process1()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.DeclarationList,1));
        }

        /// <summary>
        /// declaration-list - > declaration declaration-list' 
        /// </summary>
        private void Process2()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.DeclarationList2,2));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Declaration,2));
        }

        /// <summary>
        /// declaration-list'- > empty
        /// </summary>
        private void Process3()
        {
        }

        /// <summary>
        /// declaration-list'- > declaration declaration-list'
        /// </summary>
        private void Process4()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.DeclarationList2,4));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Declaration,4));
        }

        /// <summary>
        /// declaration -> var-declaration 
        /// </summary>
        private void Process5()
        {
            //
            elementStack.Push(new LL1StackElement(true, NonTerminalType.VarDeclaration,5));
            //建立变量声明新节点
            TreeNode t = new TreeNode(NodeKind.varK, currentToken.LineNumber, (TokenType)currentToken.TokenTypes, currentLevelNum, currentScope);
            //
            OperateTreeStack(t);
        }

        /// <summary>
        /// declaration -> fun-declaration
        /// </summary>
        private void Process6()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.FunDeclaration,6));
            //建立函数声明节点
            TreeNode t = new TreeNode(NodeKind.funK, currentToken.LineNumber, (TokenType)currentToken.TokenTypes, currentLevelNum, currentScope);
            //
            OperateTreeStack(t);
        }

        /// <summary>
        /// var-declaration -> type-specifier ID' array
        /// </summary>
        private void Process7()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Array,7));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ID2,7));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.TypeSpecifier,7));
        }

        /// <summary>
        /// array -> empty ;
        /// </summary>
        private void Process8()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(';'),8));
        }

        /// <summary>
        /// array -> [NUM] ; 
        /// </summary>
        private void Process9()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(';'),9));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(']'), 9));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.NUM2, 9));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('['), 9));
            //补全信息,把数组信息给TreeNode
            TreeNode curTop = treeStack.Peek();
            Number currentNumber = currentToken as Number;
            curTop.IsArray = true;
            curTop.ArraySize = Convert.ToInt32(currentNumber.Lexeme);
        }

        /// <summary>
        /// type-specifier -> int 
        /// </summary>
        private void Process10()
        {
            elementStack.Push(new LL1StackElement(false, TokenType.INT,10));
        }

        /// <summary>
        /// type-specifier -> void 
        /// </summary>
        private void Process11()
        {
            elementStack.Push(new LL1StackElement(false, TokenType.VOID,11));
        }

        /// <summary>
        /// ?fun-declaration -> type-specifier ID'(params) compound-stmt
        /// </summary>
        private void Process12()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.CompoundStmt,12));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(')'), 12));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Params, 12));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('('), 12));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ID2, 12));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.TypeSpecifier, 12));
        }

        /// <summary>
        /// params ->  param-list 
        /// </summary>
        private void Process13()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ParamList, 13));
            //函数声明， 遇到参数 层数加一
            currentLevelNum++;

        }

        /// <summary>
        /// params -> empty
        /// </summary>
        private void Process14()
        {
        }

        /// <summary>
        ///  param-list -> param param-list'
        /// </summary>
        private void Process15()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ParamList2, 15));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Param,15));
        }

        /// <summary>
        /// param-list' -> empty 
        /// </summary>
        private void Process16()
        {
        }

        /// <summary>
        /// param-list' ->  , param param-list'
        /// </summary>
        private void Process17()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ParamList2,17));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Param,17));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(','),17));
        }

        /// <summary>
        /// param -> type-specifier ID array
        /// </summary>
        private void process18()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Array,18));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ID2,18));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.TypeSpecifier,18));
            //新建参数节点
            TreeNode t = new TreeNode(NodeKind.paramK, currentToken.LineNumber, (TokenType)currentToken.TokenTypes, currentLevelNum, currentScope);
            OperateTreeStack(t);
        }

        /// <summary>
        /// compound-stmt -> {local-declarations statement-list}
        /// </summary>
        private void Process19()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('}'),19));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.StatementList,19));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.LocalDeclarations,19));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('{'),19));

        }

        /// <summary>
        /// local-declarations -> empty 
        /// </summary>
        private void Process20()
        {
        }

        /// <summary>
        /// local-declarations ->  var-declaration local-declarations
        /// </summary>
        private void process21()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.VarDeclaration));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.LocalDeclarations));
        }

        /// <summary>
        /// statement-list ->  empty 
        /// </summary>
        private void process22()
        {
        }

        /// <summary>
        /// statement-list ->  statement statement-list
        /// </summary>
        private void process23()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.StatementList));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Statement));
        }

        /// <summary>
        /// statement -> expression-stmt 
        /// </summary>
        private void process24()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ExpressionStmt));
        }

        /// <summary>
        /// statement -> compound-stmt 
        /// </summary>
        private void process25()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.CompoundStmt));
        }

        /// <summary>
        /// statement -> selection-stmt
        /// </summary>
        private void process26()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.SelectionStmt));

        }

        /// <summary>
        /// statement -> iteration-stmt 
        /// </summary>
        private void process27()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.IterationStmt));
        }

        /// <summary>
        /// statement -> return-stmt
        /// </summary>
        private void process28()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ReturnStmt));
        }

        /// <summary>
        /// expression-stmt-> expression ; 
        /// </summary>
        private void process29()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(';')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Expression));
        }

        /// <summary>
        /// expression-stmt-> ;
        /// </summary>
        private void process30()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(';')));
        }
        /// <summary>
        /// selection-stmt -> if (expression) statement else-statement
        /// </summary>
        void process31()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ElseStatement));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Statement));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(')')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Expression));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('(')));
            elementStack.Push(new LL1StackElement(false, TokenType.IF));
        }
        /// <summary>
        /// else-statement -> empty
        /// </summary>
        void process32()
        {
            
        }
        /// <summary>
        /// else-statement -> else statement
        /// </summary>
        void process33()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Statement));
            elementStack.Push(new LL1StackElement(false, TokenType.ELSE));
        }
        /// <summary>
        /// iteration-stmt -> while (expression) statement 
        /// </summary>
        void process34()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Statement));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(')')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Expression));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('(')));
            elementStack.Push(new LL1StackElement(false, TokenType.WHILE));
        }
        /// <summary>
        /// return-stmt → return res
        /// </summary>
        void process35()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Res));
            elementStack.Push(new LL1StackElement(false, TokenType.RETURN));
        }
        /// <summary>
        /// res -> ; 
        /// </summary>
        void process36()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(';')));
        }
        /// <summary>
        /// res -> expression;
        /// </summary>
        void process37()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(';')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Expression));
        }
        /// <summary>
        /// expression -> var = expression 
        /// </summary>
        void process38()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Expression));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('=')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Var));
        }
        /// <summary>
        /// expression -> simple-expression
        /// </summary>
        void process39()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.SimpleExpression));
        }
        /// <summary>
        /// var -> ID array'
        /// </summary>
        void process40()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Array2));
            elementStack.Push(new LL1StackElement(false, TokenType.ID));
        }
        /// <summary>
        /// array' -> empty
        /// </summary>
        void process41()
        {
        }
        /// <summary>
        /// array' -> [expression]
        /// </summary>
        void process42()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(']')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Expression));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('[')));
        }
        /// <summary>
        /// simple-expression -> additive-expression relop-expression
        /// </summary>
        void process43()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.RelopExpression));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.AdditiveExpression));
        }
        /// <summary>
        /// relop-expression -> empty 
        /// </summary>
        void process44()
        {
        }
        /// <summary>
        /// relop-expression -> relop additive-expression
        /// </summary>
        void process45()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.AdditiveExpression));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Relop));
        }

        /// <summary>
        /// relop → <=
        /// </summary>
        void process46()
        {
            elementStack.Push(new LL1StackElement(true, TokenType.LESSEQUAL));
        }
        /// <summary>
        /// additive-expression -> term additive-expression'
        /// </summary>
        void process47()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.AdditiveExpression2));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Term));
        }
        /// <summary>
        /// additive-expression' -> empty
        /// </summary>
        void process48()
        {
        }
        /// <summary>
        /// additive-expression' ->  addop term additive-expression'
        /// </summary>
        void process49()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.AdditiveExpression2));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Term));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Addop));
        }
        /// <summary>
        /// addop → + 
        /// </summary>
        void process50()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('+')));
        }
        /// <summary>
        /// term → factor term'
        /// </summary>
        void process51()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Term2));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Factor));
        }
        /// <summary>
        /// term' -> empty 
        /// </summary>
        void process52()
        {
        }
        /// <summary>
        /// term' -> mulop factor term2
        /// </summary>
        void process53()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Term2));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Factor));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Mulop));
        }
        /// <summary>
        ///  mulop → * 
        /// </summary>
        void process54()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('*')));
        }
        /// <summary>
        /// factor → ID A2
        /// </summary>
        void process55()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.A2));
            elementStack.Push(new LL1StackElement(true, TokenType.ID));
        }
        /// <summary>
        /// factor → ( expression ) 
        /// </summary>
        void process56()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(')')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Expression));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(')')));
        }
        /// <summary>
        /// factor →  NUM
        /// </summary>
        void process57()
        {
            elementStack.Push(new LL1StackElement(false, TokenType.NUMBER));
        }
        /// <summary>
        /// A2 -> array2 
        /// </summary>
        void process58()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Array2));
        }
        /// <summary>
        /// A2 -> array2 ( args )
        /// </summary>
        void process59()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(')')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Args));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(')')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Array2));
        }
        /// <summary>
        /// call  → ID( args )
        /// </summary>
        void process60()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(')')));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Args));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(')')));
            elementStack.Push(new LL1StackElement(false, TokenType.ID));
        }
        /// <summary>
        /// args → arg-list 
        /// </summary>
        void process61()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ArgList));
        }
        /// <summary>
        /// args → empty
        /// </summary>
        void process62()
        {
        }
        /// <summary>
        ///  arg-list -> expression arg-list2
        /// </summary>
        void process63()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ArgList2));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Expression));
        }
        /// <summary>
        /// arg-list2 -> empty 
        /// </summary>
        void process64()
        {

        }
        /// <summary>
        /// arg-list2 -> ,expression arg-list2
        /// </summary>
        void process65()
        {
            elementStack.Push(new LL1StackElement(true, NonTerminalType.ArgList2));
            elementStack.Push(new LL1StackElement(true, NonTerminalType.Expression));
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16(',')));
        }
        /// <summary>
        ///  addop →  -
        /// </summary>
        void process66()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('-')));
        }
        /// <summary>
        /// mulop → /
        /// </summary>
        void process67()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('/')));
        }
        /// <summary>
        /// relop →  < 
        /// </summary>
        void process68()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('<')));
        }
        /// <summary>
        /// relop →  >
        /// </summary>
        void process69()
        {
            elementStack.Push(new LL1StackElement(false, Convert.ToInt16('>')));
        }
        /// <summary>
        /// relop →  >=
        /// </summary>
        void process70()
        {
            elementStack.Push(new LL1StackElement(false, TokenType.GREATERQUAL));
        }
        /// <summary>
        /// relop →  ==
        /// </summary>
        void process71()
        {
            elementStack.Push(new LL1StackElement(false, TokenType.EQUAL));
        }
        /// <summary>
        /// relop →  !=
        /// </summary>
        void process72()
        {
            elementStack.Push(new LL1StackElement(false, TokenType.NOTEQUAL));
        }
        /// <summary>
        /// ID' -> ID
        /// </summary>
        private void Process73()
        {
            elementStack.Push(new LL1StackElement(false, TokenType.ID),73);
            TreeNode curTop = treeStack.Peek();
            Word currentWord = currentToken as Word;
            //补全信息,把语义信息给TreeNode
            curTop.Name = currentWord.Lexeme;

        }

        /// <summary>
        /// NUM' -> NUM
        /// </summary>
        private void Process74()
        {
            elementStack.Push(new LL1StackElement(false, TokenType.NUMBER));
        }
    }
}
