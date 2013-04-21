using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.ParseRecursive;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.Analyzer
{
    public class Analyzer
    {
         FunCheck FunArgs = new FunCheck();
         SymbolTable symbolTab = new SymbolTable();
        public TreeNode program;
        bool is_good_;
        bool traceAnalyze;
        bool traceParse;
        Parser parse;
         int location;       // memory location
         int err;
         int warn;
        TreeNode currenttreenode;
        //显示SymbolTable
        public List<BucketList> GetSymbolTableList()
        {
            List<BucketList> bll = new List<BucketList>();
            for (int index = 0; index < 211; ++index) {
                if (symbolTab.hashTable[index] == null)
                    continue;
                LineList ll = symbolTab.hashTable[index].Lines;
                bool first = true;
                //symbolTab.hashTable[index].appear += symbolTab.hashTable[index].Lines.lineno;
                while (ll != null) {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        symbolTab.hashTable[index].appear += ", ";
                    }
                    symbolTab.hashTable[index].appear += ll.lineno;
                    ll = ll.next;
                }
                bll.Add( symbolTab.hashTable[index] );
            }
            return bll;
        }

        public Analyzer(List<Token> tokenlist)
        {
            traceAnalyze= true;
            traceParse = true;
            parse = new Parser(tokenlist);
	        program = null;
            program = parse.BuildSyntaxTree();
            program = program.Child[0];
	       // symbolTab.initial();   // because symbolTab and FunArgs is static member,
	       // FunArgs.initial();     // so, before use , should reset/initial them
        	
	        //symFile = filename;
	        //int pos = symFile.rfind('.');
	        //symFile.erase(pos, symFile.length()-1);
	        //symFile += ".tab";	// symbol table file
            
	        is_good_ = true;
	        location = 0;      //  intial memory location
	        err = 0;           // there are static member, should initial before use
	        warn = 0;
        }

        public bool is_good()
        {
	        return (is_good_ && (err <= 0) && (warn <= 0) && parse.is_good());
        }

        /// <summary>
        /// 主要接口，直接用来构造语法树，完成语义分析， 得到结果，即，符号表。
        /// </summary>
        public void GetSymbolFile()
        {
	        ////outputMsg(-1, "building syntax tree...");
	        //program = parse.BuildSyntaxTree();//此时是root;
           // program = program.Child[0];
	        //if (parse.is_good())
	        //{
            // parse.getListFile();
            // if (traceParse)	parse.getTreeFile();

		    //outputMsg(-1, "successfully build the syntax tree!");
		    //outputMsg(-1, "constructing symbol table and Type cheching...");

		        /* 语法树文件可以完全在后面生成，但是总出错， 不得不放在这里，
		        * 再加上我调试器坏了，实在没有办法调试了，似乎错误出现在
		        * Parser::PrintTree(TreeNode *pNode) 函数中的一条 if 语句，
		        * 我没有跟踪内存数据的变化，调试器有问题，，
		        * 算了， 放在这里虽然布局比较凌乱，但是还算是效果满意！！ 呵呵
		        */
		        BuildSymbolTable(program);
                TypeCheck(program);
		        if (is_good())    // if no parse error ..
		        {
			        ////outputMsg(-1, "now, Type checking...");
			        
			      //  if (is_good())  //outputMsg(-1, "Type check is done, its no error(s)!");
			       // else		//outputMsg(-1, "error(s) occur while Type checking!");
		        }
		        //else
		       // {
			        ////outputMsg(-1, "errors occur while constructing symbol table, stop Type checking!");
		       // }

		       // if (traceAnalyze && is_good())    // if has no parse and ananlyze error
		        //{
			        ////outputMsg(-1, "symbol table complated, printing symbol table file ...");
			       // symbolTab.printSymTable(symFile);
		       // } 
	        //}
            //else
            //{
            //    //outputMsg(-1, "errors occur while parsing, stop constructing symbol table!");
            //    err++;        // only list file
            //    parse.getListFile();
            //}
        }
        
        /// <summary>
        /// constructs the symbol table by preorder traversal of the syntax tree
        /// </summary>
        /// <param name="pNode"></param>
        public void BuildSymbolTable(TreeNode pNode)
        {
	        traverse1(pNode);
        }

        public void traverse1(TreeNode t)
        {
            if (t != null)
            {
                InsertNode(t);
                for (int i = 0; i <= 10; ++i)
                {
                    traverse1( t.Child[i]);
                }
                
               // postProc(t);
                traverse1( t.Sibling);
            }
        }

        private void ErrorRecord(AnalyzerErrorType analyzerErrorType,TreeNode t)
        {
            ErrorBase.ErrorManager.AddAnalyzerError(new AnalyzerError( analyzerErrorType,t.LineNum, 0, t.Name) );
        }

        public void InsertNode(TreeNode t)
        {
	        if (t == null) return;
        	
	        switch (t.NodeKind )
            {
		        case NodeKind.funK:
			        if (symbolTab.LookUp(t.Name, t.Scope) == -1)
			        {
                        symbolTab.Insert(t.Name, t.Scope, t.Type, t.LineNum, location, false);
                        ////addMemLoc();
				        FunArgs.Insert(t);
			        }
			        else
			        {
				        //Analyzer::parse.add_err();
				        err++;
				       // //sprintf(msg_temp, "function \"%s\"(...) redefinition", t.Name.c_str());
				        ////outputMsg(t.LineNum, msg_temp);
			        }

			        break;
                case NodeKind.varK:
                case NodeKind.paramK:
			        if (symbolTab.LookUp(t.Name, t.Scope) == -1)
			        {
				        symbolTab.Insert(t.Name, t.Scope, t.Type, t.LineNum, location, t.IsArray);
				        ////addMemLoc();
			        }
			        else
			        {
				        err++;

				        //sprintf(msg_temp, "variable \"%s\" redefinition", t.Name.c_str());
                        ErrorRecord(AnalyzerErrorType.VariableRedefinition, t);
				        //outputMsg(t.LineNum, msg_temp);
			        }
			        break;
                case NodeKind.stmtK:	// just call
			        if (t.stmt == StmtKind.callK)
			        {
				        if (symbolTab.LookUp(t.Name, t.Scope) == -1)
				        {
					        err++;
					        //sprintf(msg_temp, "unresolved external symbol \"%s\"", t.Name.c_str());
					        //outputMsg(t.LineNum, msg_temp);
				        }
				        else
				        {
					        symbolTab.Insert(t.Name, t.Scope, TokenType.ID, t.LineNum, 0, false);
				        }
			        }

			        break;
                case NodeKind.expK:
			        if (t.exp == ExpKind.IDK) {
				        if ( symbolTab.LookUp(t.Name, t.Scope) == -1
					        && symbolTab.LookUp(t.Name, "global") == -1)
				        {	// undeclared
					        //Analyzer::parse.add_err();
					        err++;
                            ErrorRecord(AnalyzerErrorType.IdentifierUndeclared, t);
                            //sprintf(msg_temp, "undeclared identifier: \"%s\"", t.Name.c_str());
                            //outputMsg(t.LineNum, msg_temp);
				        }
				        else if (symbolTab.LookUp(t.Name, t.Scope) !=  -1) 
				        {	// local variable
                            if ((t.Father!=null) && (t.Father.NodeKind != NodeKind.stmtK || t.Father.stmt != StmtKind.callK)/* not in call statement */ &&
					        t.IsArray != symbolTab.SearchArray(t.Name,t.Scope)) 
					        {	
						        //Analyzer::parse.add_err();
						        err++;
                                //sprintf(msg_temp, "\"%s\" is %s declared as array", t.Name.c_str(), t.bArr? "not" : "");
                                //outputMsg(t.LineNum, msg_temp);
					        }
					        else
					        {
						        symbolTab.Insert(t.Name, t.Scope, t.Type, t.LineNum, 0, false);
					        }
				        }
				        else
				        {	// global variable
					        t.Scope = "global";
                            if ((t.Father != null) && (t.Father.NodeKind != NodeKind.stmtK || t.Father.stmt != StmtKind.callK)/* not in call statement */ &&
                            t.IsArray != symbolTab.SearchArray(t.Name, t.Scope))
                            {
                                //Analyzer::parse.add_err();
                                err++;
                                //sprintf(msg_temp, "\"%s\" is %s declared as array", t.Name.c_str(), t.bArr? "not" : "");
                                //outputMsg(t.LineNum, msg_temp);
                            }
                            else
                            {
                                symbolTab.Insert(t.Name, t.Scope, t.Type, t.LineNum, 0, false);
                            }
				        }
			        }

			        break;
		        default:
			        break;
	        }

        }

        void Traverse2(TreeNode t)
        {
            if (t != null)
            {
                for (int i = 0; i < t.Child.Length; ++i)
                {
                    if (t.Child != null)
                        Traverse2( t.Child[i]);
                }

                CheckNode(t);
                Traverse2( t.Sibling);
            }
        }
                        
        /// <summary>
        /// Type checking by a postorder syntax tree traversal
        /// </summary>
        /// <param name="pNode"></param>
        void TypeCheck(TreeNode pNode)
        {
	        Traverse2(pNode);
	        if ( symbolTab.LookUp("main", "global") == -1)
	        {
		        err++;
		        is_good_ = false;
		        ////outputMsg(-1, "Unresolved external symbol _main");
		        warn++;
		        ////outputMsg(-1, "program must have function \"main(void)\" !");
	        }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pNode"></param>
        void CheckNode(TreeNode pNode)
        {
	        TreeNode t;
            string args;
            int ret, oldline; // record the return value and function defined lineno if its defined.

            // initial them
            ret = oldline = -1;
            args = "";
            t = pNode;

	        switch (t.NodeKind) {
		        //case NodeKind.stmtK:
                case NodeKind.stmtK:
			        switch (t.stmt) {
				        case StmtKind.readK:
					        if (t.Child[0] != null) {
						        t.Type = t.Child[0].Type;
						        if (t.Type != TokenType.INT){
                                    //outputMsg(t.lineno, "read statement can only read a \"int\" value to a int variable!");
							        err++;
						        }
					        }
					        else
					        {
						        err++;
                                ErrorRecord(AnalyzerErrorType.MaybeCompilerBug,t);
						        //outputMsg(-1, "maybe compiler bug!");
					        }

					        break;
                        case StmtKind.writeK:
					        if (t.Child[0] != null) {
						        t.Type = t.Child[0].Type;
                                if (t.Type != TokenType.INT)
                                {
							        //outputMsg(t.lineno, "write statement can only output a \"int\" variable or a number output!");
							        err++;
						        }
					        }
					        else
					        {
						        err++;
                                ErrorRecord(AnalyzerErrorType.MaybeCompilerBug, t);
						        //outputMsg(-1, "maybe compiler bug!");
					        }

					        break;
                        case StmtKind.returnK:
					        if (t.Child[0] == null) { // return should be a void
                                if (symbolTab.SearchType(t.Name, "global") != TokenType.VOID)
                                {
							        //sprintf(msg_temp, " function \"int %s(...)\" must have return value ", t.Name.c_str());
							        //outputMsg(t.lineno, msg_temp);
							        err++;
						        }
					        }
					        break;
                        case StmtKind.callK:
					        ret = FunArgs.Check(t, args, ref oldline);
					        if (ret != -3)  // -3 is ok
					        {							
						        err++;
						        warn++;

						        if (ret >= 0) {
                                    //sprintf(msg_temp,
                                    //        "too few or many paramenters to call function \"%s(%s)\", its has %d params",
                                    //        t.Name.c_str(),
                                    //        args.c_str(),
                                    //        ret);
							        //outputMsg(t.lineno, msg_temp);
                                    // out put the functions' declaration,
                                    //sprintf(msg_temp,
                                    //        "see the function \"%s(%s)\" declaration here!!",
                                    //        t.Name.c_str(),
                                    //        args.c_str());
                                    //outputMsg(oldline, msg_temp);

						        } else if (ret == -1) {
							        //sprintf(msg_temp, "function \"%s\"(...) has not declared before use it", t.Name.c_str());
							        //outputMsg(t.lineno, msg_temp);
						        } else {
                                    //sprintf(msg_temp,
                                    //        " call function \"%s(%s)\" with no matched Type paramenters!",
                                    //        t.Name.c_str(), args.c_str());
							        //outputMsg(t.lineno, msg_temp);

                                    //sprintf(msg_temp,
                                    //        "see the function \"%s(%s)\" declaration here!!",
                                    //        t.Name.c_str(),
                                    //        args.c_str());
                                    //outputMsg(oldline, msg_temp);
						        }
					        }
					        else
					        {
						        t.Type = symbolTab.SearchType(t.Name, t.Scope);
					        }
					        break;
				        default:
					        break;
			        }
                    break;
                case NodeKind.expK:
			        switch (t.exp) {
				        case ExpKind.OpK:
					        if (Convert.ToInt32(t.Type) == Convert.ToInt32('='))
					        {
						        t.Type = t.Child[0].Type;
					        }
					        else
					        {
                                if (t.Child[0].Type == TokenType.VOID || t.Child[1].Type == TokenType.VOID)
						        {
							        err++;
							        //outputMsg(t.lineno, "illegal use of Type \"void\"");
						        }
                                else if (t.Child[0].Type == TokenType.INT || t.Child[1].Type == TokenType.INT)
						        {
                                    t.Type = TokenType.INT;
						        }
						        else
						        {
							        // ...
						        }
					        }
					        break;
				        case ExpKind.IDK:
					        t.Type = symbolTab.SearchType(t.Name, t.Scope);
					        t.IsArray = symbolTab.SearchArray(t.Name, t.Scope);
					        break;
				        default:
					        break;
			        }
			        break;
		        default:
			        break;
		        }
        }

    }
}
