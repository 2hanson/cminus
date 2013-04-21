using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.ParseRecursive;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.Analyzer
{
    public class FunCheck
    {
        public FunDecListRec first,last;

        public void Insert(TreeNode pNode)
        {	// pNode.nodekind must be funK,
	        // pNode.name	must be not in FunDecListRec;
	        FunDecListRec temp = new FunDecListRec(pNode.Name, pNode.Type);

	        // count the params
	        TreeNode p = pNode.Child[0];	// params link in pNode.chilld[0].sibling
            temp.lineno = pNode.LineNum;// record functions' line
        	
	        if (p != null)
	        {
		        temp.Params = new ParamListRec(p.Type, p.IsArray);
		        temp.count++;

		        ParamListRec l = temp.Params;
		        while (p.Sibling  != null)
		        {
			        p = p.Sibling;
			        l.next = new ParamListRec(p.Type, p.IsArray);
			        l = l.next;
			        temp.count++;
		        }
	        }

	        if (first == null)  // has not function declarations in list
	        {
		        first = last = temp;
	        }
	        else
	        {
		        last.next = temp;
		        last = last.next;
	        }
        }
        
        /// <summary>
        /// check if a function call's argument match its declaration parameters;
        /// return -1, not found;
        /// return -2, type not match;
        /// return -3, match;
        /// else return delcaration parameter count, not match,  and
        /// "string &args"  is the function's parament list, like "int, int[], int"
        /// line to record the functions' lineno that its defined here.
        /// </summary>
        public int Check(TreeNode pNode, string args,ref int line)
        {
	        FunDecListRec l = first;

	        while ((l!=null) && l.name != pNode.Name)	l = l.next;
	        if (l==null)	return -1;	// function use before its declaration or not declara

	        ParamListRec p = l.Params;
	        TreeNode t = pNode.Child[0];

            // record paraments list, this is not a good idea
            while (p!=null) {
    	        if (p.type == TokenType.VOID) args  = "void, " + args;
                else if (p.type == TokenType.INT){
        	         args =  (p.isArr ? "int[], " : "int, ") + args;
                }

                p = p.next;
            }

           // args.erase(args.Length-2);	// erase last ", "
            args = args.Remove(args.Length - 2,1);
            line = l.lineno;
            // set it point to l.params after record paraments list
            p = l.Params;
	        while (p!=null &&t!=null)
	        {
		        if ((p.type == t.Type && p.isArr == t.IsArray)
                    || (t.NodeKind ==NodeKind.expK && t.exp ==ExpKind.ConstK && t.Type == TokenType.NUMBER))
		        {
			        p = p.next;
			        t = t.Sibling ;
		        }
		        else
		        {
			        return -2;	// type not match;
		        }
	        }

	        if (p!=null || t!=null) {
         	        return l.count;	// params count not match
            }
	        else	return -3;				// all match;
        }

    }
}
