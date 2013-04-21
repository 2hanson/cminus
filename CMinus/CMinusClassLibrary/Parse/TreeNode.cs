using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.Parse
{
    public class TreeNode
    {
        public string	Name{get; set;}
	    public string	Scope{get; set;}	    // node function scope
	    public bool	IsArray{get; set;}		// array ?
	    public int		LineNum{get; set;}
	    public int		ArraySize{get; set;}	// arrary size;
        const int MAX_CHILDREN = 1000;
        StmtKind	stmt;
        ExpKind		exp;
        public TreeNode[]	Child = new TreeNode[MAX_CHILDREN]; // pointer to child node
        public int ChildNum { get; set; }
	    public TreeNode	Father{get; set;}						// pointer to father node
	    public TreeNode	Sibling{get; set;}				 // pointer to sibling node
	    public NodeKind	NodeKind{get ; set;}   
        public object Kind
        {
            get
            {
                if(NodeKind == NodeKind.stmtK)
                {
                    return stmt;
                }
                else
                {
                    return exp;
                }
            }
            set
            {
                if (value.GetType().ToString() == "StmtKind")
                {
                    stmt = (StmtKind)value;
                }
                else
                {
                    exp = (ExpKind)value;
                }
            }
        }
	    public TokenType	Type{get; set;}
        /// <summary>
        /// TreeNode 在Tree的层号
        /// </summary>
        public int LevelNum{get; set;}
        /// <summary>
        /// TreeNode  在 每一层的序号
        /// </summary>
        public int PositionInLevel{get; set;}

        public TreeNode()
        {
            ChildNum = 0;
        }

        /// <summary>
        /// 加孩子，从1开始编号
        /// </summary>
        /// <param name="treenode"></param>
        public void AddChild(TreeNode treeNode)
        {
            ++ChildNum;
            Child[ChildNum] = treeNode;
        }
        /// <summary>
        ///  create a new node
        /// </summary>
        public TreeNode(NodeKind kind, int LineNum,TokenType Type,  int currentLevelNum, string Scope):this()
        {
            this.NodeKind = kind;
            this.LineNum = LineNum;
            this.Type = Type;
            this.LevelNum = currentLevelNum;
            this.Scope = Scope;
        }

        // // create a new statment node
        //public TreeNode(StmtKind Kind, string Name)
        //{
        //    //this.lineno  = scan.lineno();
        //    //this.NodeKind = NodeKind.stmtK;
        //    //this.kind.stmt = Kind;
        //    //this.Type  = TokenType.NONE;
        //    //this.Name  = Name;
        //    //this.Scope	= Scope;

        //    //return t;
        //}

        // create a new expression node
        //public TreeNode()
        //{
        //    //TreeNode t = new TreeNode();
        //    //t.lineno = scan.lineno();
        //    //t.nodekind = expK;
        //    //t.kind.exp = Kind;
        //    //t.type		= Type;
        //    //t.Name  = Name;
        //    //t.scope	= Scope;

        //    //return t;
        //}

        private TreeNode LastSibling()
        {
            TreeNode last = this;
	        while (last.Sibling != null)
            {
                last = last.Sibling;
	        }
            return last;

        }
    }    
}
