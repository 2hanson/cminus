using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.ParseRecursive;
using CMinusClassLibrary.Analyzer;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.ASMGenerator
{
    public class ASMGenerator
    {
        TreeNode tree_;
        String tempString;
        StreamWriter inStream;
        int cL;		// current label number;
        CMinusClassLibrary.Analyzer.Analyzer analyze;
        bool is_good_ = true;
        string codefile;

        public ASMGenerator(List<Token> tokenList)
        {
            //err = 0;
            //warn= 0;
	        cL	= 0;
	
            //int pos = codefile.rfind('.');
            //codefile.erase(pos, codefile.length()-1);
            //codefile += ".asm";
            inStream = new StreamWriter("c:\\gcd.asm",false,Encoding.ASCII,10);
            analyze = new CMinusClassLibrary.Analyzer.Analyzer(tokenList);
        }
        /// <summary>
        /// This is 80X86 ASM code generator primary interface
        /// </summary>
        public void CodeGen()
        {
            if (analyze != null){
                analyze.GetSymbolFile();  // build syntax tree, symbol table, fun check ...
            } else {
                //is_good_ = false;
                //err++;
                //outputMsg(-1, "some expection happened when analyze::getSymbolFile() !");
            }
	
            if (true)
            {
                // confirm has no error,
                //outputMsg(-1, "80x86 ASM code generating ......");
		
                //code_.open(codefile.c_str());
                
                if (inStream == null)  // create code file fail
                {
                    //is_good_ = false;
                    //err++;
                    //sprintf(msg_temp,
                    //    "create code file \"%s\" fail ... generating x86ASM code stopping ...",
                    //    codefile.c_str());
                    //outputMsg(-1, msg_temp);
		        }
		        else
		        {
                    // primary here
                    X86Gen();
                    inStream.Close();
                    //sprintf(msg_temp,
                    //    "80x86 ASM Code has save to \"%s\"...",
                    //    codefile.c_str());
                    //outputMsg(-5,msg_temp);
		        }
            }
            else
            {
                //is_good_ = false;
                //warn++;
                //outputMsg(-1,
                //    "error(s) occur before 80x86 ASM code generating....."
                //    " code  generating stop ...");
            }
        }

        private bool is_good()
        {
             return is_good_ &&  analyze.is_good(); 
        }


        /// <summary>
        /// This is main part of 80X86 ASM code generator
        /// </summary>
        void X86Gen()
        {
            tree_ = analyze.program;
            if (tree_ !=null) {
                X86GenPre();        // generate stack, data, and program messages
                X86GenCode();       // generate functions
            }
            else
            {
                //is_good_ = false;
                //err++;
                //outputMsg(-1, "Some error(s) occur before 80x86 ASM code generating.....");
                //outputMsg(-1, "80x86 ASM generator get a empty syntax tree!!!!");
            }
        }

        /// <summary>
        /// This program to create 80x86 asm file data segment
        ///*   define varibles and  stack ...
        /// </summary>
        private void X86GenPre()
        {
            string preMsg;
    
            EmitCode(codefile);    // need no ';' again
            preMsg =
		   
		        "       .model  small\n" +
		        "       .586					;maybe use Pentium instructions\n" +
		        ";*****************************************************************************\n" +
		        "stacksg	segment	para	stack	'stack'\n" +
		        "           dw	200h	dup(0)\n"  +      // 512 Bytes of stack space
		        "	tos	label   word			;pointer to stack top\n" +
		        "stacksg	ends\n" +
		        ";*****************************************************************************\n" +
		        "datasg	segment	para	'data'\n" +
		        " 	re_val		dw	0 			;return values\n" +
		        ";other	global variables and main varibales\n"; 
	
            EmitCode(preMsg);
            // other data
            TreeNode t; 
	        TreeNode p = tree_;   // should assert (t)
	
	        // 或者应该直接用 symbolTable 来获得数据；
	        while (p != null) {
		        if (p.NodeKind == NodeKind.varK) {
			        X86GenData(p);
		        }
		        else if (p.Name == "main") {// local variable in _main, see it global
			        t = p.Child[1];    // void main(void), needn't child [0];
			        while (t != null) {
				        if (t.NodeKind == NodeKind.varK) {
					        X86GenData(t);
				        }
				        t = t.Sibling;
			        }
		        }
		        p = p.Sibling;
	        }
	
            // end of data segement
            preMsg =
                "datasg	ends					;end of data segment\n";
            EmitCode(preMsg);
            //inStream.Close();
        }

        /// <summary>
        /// This is code generator of 80X86 ASM code generator
        /// create  code segment, and processes...
        /// </summary>
        void X86GenCode()
        {
            //...
            string codeMsg;
            codeMsg =
                ";*****************************************************************************\n" +
                "codesg	segment	para 'code'\n" +
                ";\n" +
		        ";-----------------------------------------------------------------------------\n" +
		        ";			begin\n" +
		        ";		=======================\n" +
		        ";\n" +
		        ";main part of program, void main(void)\n" +
                ";-----------------------------------------------------------------------------\n" +
                "begin	proc    far\n" +
                "	assume cs:codesg, ds:datasg, ss:stacksg\n" +
                ";set SS register to current stack segment\n" +
                "       mov     ax, stacksg\n" +
                "       mov     ss, ax\n" +
                "       mov     sp, offset tos\n" +
                ";set up stack for return\n" +
                "       push	ds				;save old data segment\n" +
                "       sub	ax, ax				;put zore in AX\n" +
                "       push	ax				;save it on stack\n" +
                ";set DS register to current data segment\n" +
                "       mov	ax, datasg			;data segment addr\n" +
                "       mov	ds, ax				; into DS register\n" +
                "       mov	es, ax				; into ES register\n" +
                ";main part of program goes here\n" +
                ";\n\n";
            EmitCode(codeMsg);
            // main function ....
            TreeNode p;
            p  = tree_;
            while (p != null)
            {
		        if (p.NodeKind == NodeKind.funK && p.Name == "main")
		        {
			        X86GenFun(p, false);
			        break;
		        }
		        p = p.Sibling;
            }
    
            if (p==null) {       // should never happen!!
                //err++;
                //warn++;
                //is_good_ = false;
                //outputMsg(-1, "unfind extern _main in syntax tree!");
                //throw "unfind extern _main in syntax tree!";
            }
    
            codeMsg =
		        "\n	pop     ax\n"  +
		        "	pop     ds\n" +
		        ";\n" +
                "	mov	ax, 4c00h			;return to DOS\n" +
                "	int     21h\n" +
                ";\n" +
                "begin  endp					;end of begin prco\n";
            EmitCode(codeMsg);
            // end main function
    
            // x86GenFun here....
            p = tree_;
            while (p != null)
            {
		        if (p.NodeKind == NodeKind.funK && p.Name != "main")
		        {
			        X86GenFun(p, true);
		        }
		
		        p = p.Sibling;
            }
	
            read_int();
            write_int();
    
            // ok, all done.. end of code segment
            codeMsg=
		        ";\n;-----------------------------------------------------------------------------\n" +
                ";\ncodesg	ends					;end of code segment\n" +
                ";*****************************************************************************\n" +
		        "      end	begin				;end assembly\n";
            EmitCode(codeMsg);
        }

        /// <summary>
        /// generator global variables and main variables
        /// </summary>
        /// <param name="tree"></param>
        void X86GenData(TreeNode tree)
        {
            tempString = "\t"+tree.Scope+"_"+tree.Name+"_ \t dw";
            //sprintf(msg_temp, "\t %s_%s_ \t dw",
            //    tree.Scope.c_str(),tree.Name.c_str());
            EmitCode(tempString);
	
	        if (tree.IsArray) {
		        //sprintf(msg_temp, "\t %d \t dup(0)\n", tree.ArraySize);
                tempString = "\t "+tree.ArraySize.ToString()+" \t dup(0)\n";
                EmitCode(tempString);
	        }
	        else
	        {
		        EmitCode("\t0\n");
	        }
        }

        /// <summary>
        /// generate functions code
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="pre"></param>
        void X86GenFun(TreeNode tree, bool pre)
        {
	        string funName= "_?????_????__\n";
	        string codeStr =
		        ";\n" +
		        ";-----------------------------------------------------------------------------\n";
	        TreeNode p = tree.Child[0];
	        if (pre)                  // main function
	        {
		        EmitCode(codeStr);
                if (tree.Type == Lex.TokenType.INT)
                {
                    tempString = "_" + tree.Name + "_int__";
                }
                else
                {
                    tempString = "_" + tree.Name + "_void__";
                }
                
                //sprintf(msg_temp,"_%s_%s__",
                //tree->name.c_str(), ((tree->type == k_INT) ? "int" : "void"));
		        funName = tempString;
                tempString = ";\t\t\t" + funName + "\n";
		        //sprintf(msg_temp,";\t\t\t%s\n",funName.c_str());
		        EmitCode(tempString);
		        EmitCode(";		=============================\n;\n");
		        EmitCode(";-----------------------------------------------------------------------------\n");
                tempString = funName + "\tproc\tnear\tfortran uses bx cx dx si di";
		        //sprintf(msg_temp,"%s\tproc\tnear\tfortran uses bx cx dx si di", funName.c_str());
		        EmitCode(tempString);
		
		        if (p==null) {
			        EmitCode("\n");
		        }
		        else {  // paramenters generate
			        TreeNode t = p;
			        codeStr=",\n\t\t\t";
                    tempString = codeStr + t.Scope + "_" + t.Name + "_:word";
                    //sprintf(msg_temp, "%s%s_%s_:word",
                    //    codeStr.c_str(),
                    //    t->scope.c_str(),
                    //    t->name.c_str());
			        codeStr = tempString + codeStr;
			        t = t.Sibling;
			
			        while (t != null){
                        tempString = t.Scope + "_" + t.Name + "_:word";
                        //sprintf(msg_temp, ", %s_%s_:word",
                        //    t->scope.c_str(),
                        //    t->name.c_str());
				        codeStr += tempString;
				        t = t.Sibling;
			        }
			
			        EmitCode(codeStr);
			        EmitCode("\n");
		        }
		
		        p = tree.Child[1];
		        X86GenVar(p);
		        EmitComment("\n");
	        }
	
	        p=tree.Child[1];
	        while (p != null)
	        {
		        X86GenSE(p);
		        p = p.Sibling;
	        }
	
	        if (pre)
	        {
                if (tree.Type == Lex.TokenType.VOID)
                {
                    tempString = "\tret\n" + funName + "\tendp\t\t\t\t;end of " + funName + "\n";
                }
                else
                {
                    tempString = "" + funName + "\tendp\t\t\t\t;end of " + funName + "\n";
                }
                //sprintf(msg_temp, "%s%s\tendp\t\t\t\t;end of %s\n", 
                //    (tree.Type == Lex.TokenType.VOID) ? "\tret\n" : "", funName.c_str(), funName.c_str());
		        EmitCode(tempString);
	        }
        }

        /// <summary>
        /// generate local variables
        /// </summary>
        /// <param name="tree"></param>
        void X86GenVar(TreeNode tree)
        {
	        //....
	        string codeStr;
	        TreeNode t = tree;
	        if (t!=null && t.NodeKind == NodeKind.varK) {
		        if (t.IsArray) {
                    tempString = "\tlocal\t" + t.Scope + "_" + t.Name + "_[" + t.ArraySize + "]:word";
                    //sprintf(msg_temp, "\tlocal\t%s_%s_[%d]:word",
                    //    t->scope.c_str(), t->name.c_str(),
                    //    t->iArrSize);
		        } else {
                    tempString = "\tlocal\t" + t.Scope + "_" + t.Name + "_:word";
                    //sprintf(msg_temp, "\tlocal\t%s_%s_:word",
                    //    t->scope.c_str(), t->name.c_str());
		        }
		        codeStr = tempString;
		
		        t = t.Sibling;

                while (t != null && t.NodeKind == NodeKind.varK)
		        {
                    if (t.IsArray)
                    {
                        tempString = ", " + t.Scope + "_" + t.Name + "_["+t.ArraySize+"]:word";
                        //sprintf(msg_temp,
                        //    ", %s_%s_[%d]:word",
                        //    t->scope.c_str(), t->name.c_str(),t->iArrSize);
			        } else {
                        tempString = ", " + t.Scope + "_" + t.Name + "_:word";
                        //sprintf(msg_temp,
                        //    ", %s_%s_:word",
                        //    t->scope.c_str(),t->name.c_str());
			        }
			
			        codeStr += tempString;
			        t = t.Sibling;
		        }
		
		        EmitCode(codeStr);
		        EmitCode("\n");
	        }
        }

        /// <summary>
        /// generate stmt_exp_list code in functions
        /// </summary>
        /// <param name="tree"></param>
        void X86GenSE(TreeNode tree)
        {
	        //...
	        if (tree!=null && tree.NodeKind == NodeKind.stmtK)
	        {
		        X86GenStmt(tree);
	        }
            else if (tree != null && tree.NodeKind == NodeKind.expK)
	        {
		        X86GenExp(tree);
	        }
	        else
	        {
		        
	        }
        }

        /// <summary>
        /// generate stmt_list code in functions
        /// </summary>
        /// <param name="tree"></param>
        void X86GenStmt(TreeNode tree)
        {
	        //...
	        if (tree != null) 
            {
		        int		curL = cL;	// current Label;
		        int		curL1 = cL+1;
		        TreeNode p = tree;
		        TreeNode t;
		        string funname = "_??????_????__";
		
		        switch ((StmtKind)p.Kind)
                {
		                case StmtKind.readK:
			                EmitComment("read statement\n");
			                EmitCode("\tcall\t__read_int_\n");
			                // read result in BX register
			                t = p.Child[0];
			                if (t.IsArray){
                                EmitCode("\tpush\tbx\n");
                                if (isLeft(t) ) {
                                    tempString = "\tmov\tbx, "+t.Scope+"_"+t.Name+"_\n";
                                    //sprintf(msg_temp, "\tmov\tbx, %s_%s_\n", t->scope.c_str(), t->name.c_str());
                                    EmitCode(tempString);
                                }
                                else {
                                    tempString = "\tlea\tbx, "+t.Scope+"_"+t.Name+"_\n";
                                    //sprintf(msg_temp, "\tlea\tbx, %s_%s_\n", t->scope.c_str(), t->name.c_str());
                                    EmitCode(tempString);
                                }

                                EmitCode("\tpush\tbx\n");
                                X86GenExp(t.Child[0]);
				                EmitCode("\tmov\tsi, ax\n\tadd\tsi, si\n"+
                                            "\tpop\tbx\n\tpop\tax\n"+
                                            "\tmov\t[bx + si], ax\t\t;store result read\n");

			                }
			                else
                            {   // not array
                                tempString = "\tmov\t"+t.Scope+"_"+t.Name+"_, bx\t\t\t;store result read\n";
                                 //sprintf(msg_temp,
                                 //   "\tmov\t%s_%s_, bx\t\t\t;store result read\n",
                                 //   t->scope.c_str(), t->name.c_str());
				                EmitCode(tempString);
			                }
			                break;
		                case StmtKind.writeK:
			                EmitComment("write statement\n");
			                /// .... 
			                t = p.Child[0];
			                X86GenSE(t);
			                EmitCode("\tmov\tbx, ax\n");
			                EmitCode("\tcall\t__write_int_\n");
			                break;
		                case StmtKind.ifK:
			                cL+=2;
			                EmitComment(">>>== if statement\n");
			                // .... condition of if 
			                t = p.Child[0];
			                X86GenExp(t);

                            EmitCode("\tsub\tax, 0\n");
			                EmitComment("if: jump to [else] if condition not true\n");
                            tempString = "\tje\tiLe"+curL1+"\n";
                            //sprintf(msg_temp, "\tje\tiLe%d\n", curL1);
			                EmitCode(tempString);
			                ///..... stmt_exp_list of if
			                t = p.Child[1];
			                while (t != null) 
                            {
				                X86GenSE(t);
				                t = t.Sibling;
			                }
                            tempString = "\tjmp\tiL"+curL+"\n";
                            
                            //sprintf(msg_temp, "\tjmp\tiL%d\n", curL);
			                EmitCode(tempString);

			                EmitComment("if: jump here if condition is not true\n");
                            
                            tempString = "iLe"+curL1+":\n";
                            //sprintf(msg_temp, "iLe%d:\n",curL1);
			                EmitCode(tempString);
			                /// .... else part of if ... else ...
			                if ((t=p.Child[2]) != null)
                            {
				                EmitComment("== else statement of if ... else \n");				
				                // ...
				                //t = t->child[0];
				                while (t != null) {
					                X86GenSE(t);
					                t = t.Sibling;
				                }
			                }
                            tempString = "iL" + curL + ":\n";
                            //sprintf(msg_temp, "iL%d:\n", curL);
                            EmitCode(tempString);
            
			                EmitComment("<<<== if statement\n");
			                break;
		                case StmtKind.whileK:
			                cL+=2;
			                EmitComment(">>>== while statement\n");			
			                EmitComment("while: jump after body comes back here\n");
                            
                            tempString = "wL"+curL+":\n";
                            //sprintf(msg_temp, "wL%d:\n", curL);
			                EmitCode(tempString);
			                // ....
			                X86GenExp(p.Child[0]);

                            EmitCode("\tsub\tax, 0\n");
                            tempString = "\tje\twLe"+curL1+"\n";
                            //sprintf(msg_temp, "\tje\twLe%d\n", curL1);
			                EmitCode(tempString);
			                // ...
			                t = p.Child[1];
			                while (t!=null) {
				                X86GenSE(t);
				                t = t.Sibling;
			                }
                            tempString = "\tjmp\twL"+curL;
			                //sprintf(msg_temp, "\tjmp\twL%d", curL);
			                EmitCode(tempString);

			                EmitComment("\n");

                            tempString = "wLe"+curL+":\n";
			                //sprintf(msg_temp, "wLe%d:\n", curL1);
			                EmitCode(tempString);

			                EmitComment("<<<== while statement\n");
			                break;
		                case StmtKind.returnK:
			                EmitComment("return statement!\n");
			                if (p.Child[0] != null)
                            {
				                X86GenSE(p.Child[0]);
				                EmitCode("\tmov\tre_val, ax\n");
			                }
                            EmitCode("\tret\n");
			                break;
		                case StmtKind.callK:
                            //sprintf(msg_temp,"_%s_%s__",
                            //    p->name.c_str(),
                            //    ((p->type == k_INT) ? "int" : "void"));
                            if(p.Type == Lex.TokenType.INT)
                            {
                                tempString = "_"+p.Name+"_int__";
                            }
                            else
                            {
                                tempString = "_"+p.Name+"_void__";
                            }
			                funname = tempString;
                            tempString = "call "+p.Name+"(...)["+funname+"]here\n";
			
                            //sprintf(msg_temp, "call %s(...) [%s] here\n",
                            //    p->name.c_str(), funname.c_str());
                            EmitComment(tempString);
			
			                t = p.Child[0];
			                while (t != null) {		//process argument
				                X86GenExp(t);
				                EmitCode("\tpush\tax\n");
				                t = t.Sibling;
			                }
			                //.....
                            tempString = "\tcall\t"+funname+"\n\tmov\tax, re_val\n";
			                //sprintf(msg_temp, "\tcall\t%s\n\tmov\tax, re_val\n", funname.c_str());
			                EmitCode(tempString);
			                break;
		                default:        //bug!
			                break;
		            }       // end of switch
		
	        }
	        else
	        {
                //outputMsg(-1, "Warning: expection happened in function "
                //        "\"void asmGen::x86GenStmt(TreeNode *tree)\""
                //        " parament tree get a NULL!!!");
                //warn++;
                //is_good_ = false;
	        }
        }

        /// <summary>
        /// generate expression_list code in functions
        /// </summary>
        /// <param name="tree"></param>
        void X86GenExp(TreeNode tree)
        {
	        if (tree!=null){
		        TreeNode p = tree;
		        TreeNode t;
		        //string funname = "_??????_????__";
		
		        string op_name = p.Name;
		        switch ((ExpKind)p.Kind){
		        case ExpKind.OpK:
			        if (op_name == "=") 
                    {
				        EmitComment("==> assign statement\n");
				        X86GenSE(p.Child[1]);

				        t = p.Child[0];
				        if (t.IsArray) {
                            if (isLeft(t)) {
                                tempString = "\tmov\tbx, "+t.Scope+"_"+t.Name+"_\n";
                                //sprintf(msg_temp, "\tmov\tbx, %s_%s_\n",
                                //    t->scope.c_str(), t->name.c_str());
                                EmitCode(tempString);
                            }
                            else {
                                tempString = "\tlea\tbx, "+t.Scope+"_"+t.Name+"_\n";
                                //sprintf(msg_temp, "\tlea\tbx, %s_%s_\n",
                                //    t->scope.c_str(), t->name.c_str());
                                EmitCode(tempString);
                            }
                            EmitCode("\tpush\tax\n");
					        X86GenSE(t.Child[0]);
					        EmitCode("\tmov\tsi, ax\n\tadd\tsi, si\n\tpop\tax\n"+
					                "\tmov\t[bx + si], ax\n");
				        }
				        else
				        {
                            tempString = "\tmov\t"+t.Scope+"_"+t.Name+", ax_\n";
                            //sprintf(msg_temp, "\tmov\t%s_%s_, ax\n", t->scope.c_str(), t->name.c_str());
                            EmitCode(tempString);
				        }
				        EmitComment("<== assign statement\n");
				        break;
			        }
			
			        EmitComment("==> OP\t");
			        EmitCode(op_name);
			        EmitCode("\n");

			        X86GenSE(p.Child[0]);
			        EmitCode("\tmov\tbx, ax\n\tpush\tbx\n");
			        // ...
			        X86GenSE(p.Child[1]);
			        // ...
			        EmitCode("\tpop\tbx\n\txchg\tax, bx\n");
			        if (op_name == "+") 
                    {
				        EmitCode("\tadd\tax, bx\n");
			        }
                    else if (op_name == "-") 
                    {
				        EmitCode("\tsub\tax, bx\n");
			        }
                    else if (op_name == "*") 
                    {
				        EmitCode("\timul\tbx\n");
			        }
                    else if (op_name == "/") 
                    {
                        EmitCode("\tpush\tdx\n\tsub\tdx, dx\n");
				        EmitCode("\tidiv\tbx\n");
				        EmitCode("\tpop\tdx\n");
			        }
                    else if (op_name == "%") 
                    {
                        EmitCode("\tpush\tdx\n\tsub\tdx, dx\n");
				        EmitCode("\tdiv\tbx\n");
				        EmitCode("\tmov\tax, dx\n\tpop\tdx\n");
			        }
                    else if (op_name == "<" ) 
                    {
                        cL++;
                        EmitCode("\tcmp\tax, bx\n");
                        tempString ="\tjl\tLog"+cL+"__\n\tmov\tax, 0\n"+
                                    "\tjmp\tLoge"+cL+"__\nLog"+cL+"__:\n"+
                                    "\tmov\tax, 1\nLoge"+cL+"__:\n";
                        //sprintf(msg_temp,
                        //        "\tjl\tLog%d__\n\tmov\tax, 0\n"+
                        //            "\tjmp\tLoge%d__\nLog%d__:\n"+
                        //            "\tmov\tax, 1\nLoge%d__:\n",
                        //        cL, cL, cL, cL);
                        EmitCode(tempString);
                        //...
                    }
                    else if( op_name == ">" )
                    {
                        cL++;
                        EmitCode("\tcmp\tax, bx\n");
                        tempString ="\tjg\tLog"+cL+"__\n\tmov\tax, 0\n"+
                                    "\tjmp\tLoge"+cL+"__\nLog"+cL+"__:\n"+
                                    "\tmov\tax, 1\nLoge"+cL+"__:\n";

                        //sprintf(msg_temp,
                        //        "\tjg\tLog%d__\n\tmov\tax, 0\n"+
                        //            "\tjmp\tLoge%d__\nLog%d__:\n"+
                        //            "\tmov\tax, 1\nLoge%d__:\n",
                        //        cL, cL, cL, cL);
                        EmitCode(tempString);
                    }
                    else if ( op_name == "EQUAL") 
                    {
                        cL++;
                        EmitCode("\t\tcmp\tax, bx\n\tje");
                        tempString ="\tLoge"+cL+"__\n\tmov\tax, 0\n"+
                                    "\tjmp\tLog"+cL+"__\nLoge"+cL+"__:\n"+
                                    "\tmov\tax, 1\nLog"+cL+"__:\n";

                        //sprintf(msg_temp,
                        //        "\tLoge%d__\n\tmov\tax, 0\n"+
                        //            "\tjmp\tLog%d__\nLoge%d__:\n"+
                        //            "\tmov\tax, 1\nLog%d__:\n" ,
                        //        cL, cL, cL, cL);
                        EmitCode(tempString);
                    }
                    else if (op_name == "NOTEQUAL")
                    {
                        cL++;
                        EmitCode("\tcmp\tax, bx\n\tje");
                        tempString ="\tLoge"+cL+"__\n\tmov\tax, 1\nLoge"+cL+"__:\n";

                        //sprintf(msg_temp,
                        //        "\tLoge%d__\n\tmov\tax, 1\nLoge%d__:\n",
                        //        cL, cL);
                        EmitCode(tempString);
                    }
                    else if (op_name == "LESSEQUAL")
                    {
                        cL++;
                        EmitCode("\tcmp\tax, bx\n");
                        tempString ="\tjle\tLog"+cL+"__\n\tmov\tax, 0\n"+
                                    "\tjmp\tLoge"+cL+"__\nLog"+cL+"__:\n"+
                                    "\tmov\tax, 1\nLoge"+cL+"__:\n";

                        //sprintf(msg_temp,
                        //        "\tjle\tLog%d__\n\tmov\tax, 0\n"+
                        //            "\tjmp\tLoge%d__\nLog%d__:\n"+
                        //            "\tmov\tax, 1\nLoge%d__:\n",
                        //        cL, cL, cL, cL);
                        EmitCode(tempString);
                    }
                    else if (op_name == "GREATERQUAL")
                    {
				        cL++;
                        EmitCode("\tcmp\tax, bx\n");
                        tempString ="\tjge\tLog"+cL+"__\n\tmov\tax, 0\n"+
                                    "\tjmp\tLoge"+cL+"__\nLog"+cL+"__:\n"+
                                    "\tmov\tax, 1\nLoge"+cL+"__:\n";
                        //sprintf(msg_temp,
                        //        "\tjge\tLog%d__\n\tmov\tax, 0\n"+
                        //            "\tjmp\tLoge%d__\nLog%d__:\n"+
                        //            "\tmov\tax, 1\nLoge%d__:\n",
                        //        cL, cL, cL, cL);
                        EmitCode(tempString);
			        }else {
                        tempString = "Warning: BUG!  unknown operator! "+op_name+ "= "+p.Type+"\n";
                        //sprintf(msg_temp,
                        //    "Warning: BUG!  unknown operator! %s = %d\n",
                        //    op_name.c_str(), p->type);
				        EmitComment(tempString);
			        }
			
			        EmitComment("<== OP\t");
			        EmitCode(op_name);
			        EmitCode("\n");
			        break;
		        case ExpKind.ConstK:
                    tempString = "\tmov\tax, "+op_name+"\n";
                    //sprintf(msg_temp, "\tmov\tax, %s\n", op_name.c_str());
                    EmitCode(tempString);
			        break;
		        case ExpKind.IDK:
			        if (p.IsArray){ // a[.]
                        if ( isLeft(p) ) {
                            tempString = "\tmov\tbx, " + p.Scope+"_"+p.Name+ "_\n";
                            //sprintf(msg_temp, "\tmov\tbx, %s_%s_\n", p->scope.c_str(), p->name.c_str());
                            EmitCode(tempString);
                        }
                        else {
                            tempString = "\tlea\tbx, " + p.Scope + "_" + p.Name + "_\n";
                            //sprintf(msg_temp, "\tlea\tbx, %s_%s_\n", p->scope.c_str(), p->name.c_str());
                            EmitCode(tempString);
                        }

                        if (p.Father!=null && p.Father.NodeKind == NodeKind.stmtK && (StmtKind)p.Father.Kind == StmtKind.callK)
                        {
                            EmitCode("\tmov\tax, bx\n");
                        }
                        else
                        {
                            EmitCode("\tpush\tbx\n");
                            X86GenExp(p.Child[0]);
                            EmitCode("\tmov\tsi, ax\n\tadd\tsi, si\n"+
                                     "\tpop\tbx\n\tmov\tax, [bx + si]\n");
                        }

			        }
			        else
			        {
                        tempString = "\tmov\tax, " + p.Scope + "_" + p.Name + "_\n";
				        //sprintf(msg_temp, "\tmov\tax, %s_%s_\n", p->scope.c_str(), p->name.c_str());
                        EmitCode(tempString);
			        }

			        break;
		        default:        //bug;
			        break;
		        }
		
	        }
	        else {  // bug!
                //outputMsg(-1, "Warning: expection happened in function "
                //        "\"void asmGen::x86GenExp(TreeNode *tree)\""
                //        " parament tree get a NULL!!!");
                //warn++;
                //is_good_ = false;
	        }
        }

        bool  isLeft(TreeNode node)
        {
             if (node != null && node.NodeKind !=  NodeKind.expK && node.NodeKind != NodeKind.varK )
             {
                TreeNode t = node.Father;
                while (t != null && t.NodeKind != NodeKind.funK ) t = t.Father;
                t = t.Child[0];
                while (t != null)
                {
                    if (t.IsArray && t.Name == node.Name)  return true;
                    t = t.Sibling;
                }
		
		        return false;
             }
             else 
                 return false;
        }

        /// <summary>
        /// read(int) completed by ASM, also it's have bug
        /// can not input negative numbers!!
        /// </summary>
        void read_int()
        {
	        string  codeStr =
		        ";\n"+
		        ";-----------------------------------------------------------------------------\n"+
		        ";			__read_int_\n"+
		        ";		==========================\n"+
		        ";\n"+
		        ";Proc For Read a int decimal, result in BX\n"+
		        ";-----------------------------------------------------------------------------\n"+
                "__read_int_	proc    near\n"+
                "       local   minus:byte, count:byte\n"+
                ";minus is falg to input number\n"+
                ";initial\n"+
                "       push	ax\n"+
                "       push	cx\n"+
		        "       push	dx\n"+
                "       mov	minus, '+'			;flag, +\n"+
                "       mov 	bx, 0\n"+
                "       mov	count, 5d			;number of digits\n"+
                ";\n"+
                "_new_char_:\n"+
                "       mov	ah, 1				;input from keyboard\n"+
                "       int	21h				;call DOS\n"+
                ";\n"+
                "       cmp	al, '+'\n"+
                "       je	_new_char_			;maybe have bug!\n"+
		        "       cmp	al, '-'\n"+
		        "       jne	_continue_read_\n"+
		        "       mov	minus, '-'\n"+
		        "       jmp	_new_char_\n"+
		        ";\n"+
		        "_continue_read_:\n"+
		        "       sub	al, 30h				;ASCII to Binary\n"+
                "       jl	_exit_read_			;jump exit if < 0\n"+
                "       cmp	al, 9d				;is it > 9d ?\n"+
                "       jg	_exit_read_			;yes\n"+
                "       cbw					;byte in al to word in ax\n"+
                ";digit now in (ax)\n"+
                "       xchg	ax,bx				;trade digit & number\n"+
                "       mov	cx, 10d\n"+
                "       mul	cx\n"+
                "       xchg	ax,bx				;trade number & digit\n"+
                ";Add digit in AX to number in BX\n"+
                "       add	bx, ax				;add digit to number\n"+
                "       dec	count\n"+
                "       jnz	_new_char_			;get next digit if < 5\n"+
                "_exit_read_:					;should process minus!!!\n"+
                "       cmp     minus, '-'\n"+
                "       jne     not_negative_r\n"+
                "       neg     bx                              ;negetive\n"+
                "not_negative_r:\n"+
                "       mov	ah, 2h\n"+
                "       mov	dl, 0ah				;linefeed\n"+
                "       int	21h\n"+
                "       mov	dl, 0dh				;carriage return after read\n"+
                "       int	21h\n"+
                ";\n"+
                "       pop	dx				;recover registers\n"+
                "       pop	cx\n"+
                "       pop	ax\n"+
                ";result number in bx\n"+
                "       ret\n"+
                "__read_int_    endp				;end of proc __read_int_\n";
	
	        EmitCode(codeStr);
        }

        /// <summary>
        /// write(int) completed by ASM, also it's have bug
        ///  can not output negative numbers!!
        /// </summary>
        void write_int()
        {
	        string codeStr =
		        ";\n"+
		        ";-----------------------------------------------------------------------------\n"+
		        ";			__write_int_\n"+
		        ";		==========================\n"+
		        ";\n"+
		        ";Proc For write a int decimal to screen, int data in BX\n"+
		        ";-----------------------------------------------------------------------------\n"+
		        "__write_int_   proc    near\n"+
		        "       push	cx\n"+
		        "       push	ax\n"+
		        ";process if negtive\n"+
		        "       mov     ax, bx\n"+
		        "       not	ax\n"+
		        "       test    ah, 10000000B\n"+
		        "       jne     not_negative_w			;not negative\n"+
		        "       mov     dl, '-'\n"+
		        "       mov     ah, 02h\n"+
		        "       int     21h\n"+
		        "       neg     bx\n"+
		        "not_negative_w:\n"+
		        "       mov	cx, 10000d			;divide by 10000\n"+
		        "       mov	ax, bx				;data in bx, mov to ax\n"+
		        "       call	__dec_div_\n"+
		        "       mov	cx, 1000d\n"+
		        "       mov	ax, bx\n"+
		        "       call	__dec_div_\n"+
		        "       mov	cx, 100d\n"+
		        "       mov	ax, bx\n"+
		        "       call	__dec_div_\n"+
		        "       mov	cx, 10d\n"+
		        "       mov	ax, bx\n"+
		        "       call	__dec_div_\n"+
		        "       mov	cx, 1d\n"+
		        "       mov	ax, bx\n"+
		        "       call	__dec_div_\n"+
		        ";linefeed and carriage after out put a data\n"+
		        "       mov	ah, 2h\n"+
                "       mov	dl, 0ah				;linefeed\n"+
                "       int	21h\n"+
                "       mov	dl, 0dh				;carriage return after read\n"+
                "       int	21h\n"+
		        ";\n"+
		        "       pop	ax\n"+
		        "       pop	cx\n"+
		        "       ret					;return form _write_int_\n";
	
	        EmitCode(codeStr);
	
	        codeStr =
		        ";-----------------------------------------------------------------------------\n"+
		        ";			__dec_div_\n"+
		        ";		==========================\n"+
		        ";\n"+
		        ";Subroutine to divide number in BX by number in CX\n"+
		        ";print quotient on screen, (numberator in DX+AX, denom in CX)\n"+
		        ";-----------------------------------------------------------------------------\n"+
		        "__dec_div_     proc    near\n"+
		        ";\n"+
		        "       mov	ax, bx                          ;number low half\n"+
		        "       mov	dx, 0                           ;zero out high half\n"+
		        "       div	cx                              ;divide by CX\n"+
		        "       mov	bx, dx                          ;remainder into BX\n"+
		        "       mov	dl, al				;quotient into DL\n"+
		        ";print the contents of DL on screen\n"+
		        "       add	dl, 30h                         ;convert to ASCII\n"+
		        "       mov	ah, 2h\n"+
		        "       int	21h\n"+
		        "       ret\n"+
		        "__dec_div_     endp                            ;end of proc __dec_div_\n"+
		        ";-----------------------------------------------------------------------------\n"+
		        "__write_int_   endp                            ;end of proc __write_int_\n";
	
	        EmitCode(codeStr);
        }

        /// <summary>
        /// This program to write to 80x86 asm file comment
        /// </summary>
        /// <param name="eCmt"></param>
        void EmitComment(string eCmt)
        {
            if (inStream != null) 
            {
                inStream.Write(";");
                inStream.Write(eCmt);
            }
            else
            {
                //is_good_ = false;
                //err++;
                //sprintf(msg_temp, "failed write to ASM file \"%s\" the code: \"%s\"...",
                //    codefile.c_str(), eCmt);
                //outputMsg(-1, msg_temp);
	        }
        }

        /// <summary>
        /// This program to write into 80x86 asm file code
        /// </summary>
        /// <param name="eCde"></param>
        void EmitCode(string eCde)
        {
	        if (inStream != null)    
                inStream.Write(eCde);
	        else {
                //is_good_ = false;
                //err++;
                //sprintf(msg_temp, "failed write to ASM file \"%s\" the code: \"%s\"...",
                //    codefile.c_str(), eCde);
                //outputMsg(-1, msg_temp);
	        }
        }

    }
}
