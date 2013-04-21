using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.ParseRecursive
{
    public class TreeNode
    {
        #region
        const int MAX_CHILDREN = 1000;
        public StmtKind stmt;
        public ExpKind exp;
        public TreeNode[] Child = new TreeNode[MAX_CHILDREN]; // pointer to child node
        public int ChildNum { get; set; }
        public TreeNode Father { get; set; }						// pointer to father node
        public TreeNode Sibling { get; set; }				 // pointer to sibling node
        public NodeKind NodeKind { get; set; }
        public TokenType Type { get; set; }
        /// <summary>
        /// TreeNode 在Tree的层号
        /// </summary>
        public int LevelNum { get; set; }
        /// <summary>
        /// TreeNode  在 每一层的序号
        /// </summary>
        public int PositionInLevel { get; set; }
        public string Name { get; set; }
        public string Scope { get; set; }	    // node function scope
        public bool IsArray { get; set; }		// array ?
        public int LineNum { get; set; }
        public int ArraySize { get; set; }	// arrary size;

        /// <summary>
        /// 
        /// </summary>
        public object Kind
        {
            get
            {
                if (NodeKind == NodeKind.stmtK)
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
                string str =  value.GetType().ToString();
                if (value.GetType().ToString() == "CMinusClassLibrary.ParseRecursive.StmtKind")
                {
                    stmt = (StmtKind)value;
                }
                else
                {
                    exp = (ExpKind)value;
                }
            }
        }
        #endregion

        /// <summary>
        /// 构造函数，孩子节点数初始为0
        /// </summary>
        public TreeNode()
        {
            Father = null;
            Sibling = null;
            LineNum = 0;
            IsArray = false;
            ChildNum = 0;
        }

        /// <summary>
        /// 最后一个兄弟节点
        /// </summary>
        /// <returns></returns>
        public TreeNode LastSibling()
        {
            TreeNode last = this;
            while (last.Sibling != null)
            {
                last = last.Sibling;
            }
            return last;

        }

        /// <summary>
        /// 产生普通节点
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="type"></param>
        /// <param name="tID"></param>
        /// <param name="Linenum"></param>
        /// <param name="scope"></param>
        public TreeNode(NodeKind kind, TokenType type, string name, int Linenum, string scope)
            : this()
        {
            this.NodeKind = kind;
            this.Type = type;
            this.Name = name;
            this.LineNum = Linenum;
            this.Scope = scope;
        }

        /// <summary>
        /// create a new statment node
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="name"></param>
        /// <param name="Linenum"></param>
        /// <param name="scope"></param>
        public TreeNode(StmtKind kind, string name, int Linenum, string scope)
            : this()
        {
            this.NodeKind = NodeKind.stmtK;
            this.Kind = kind;
            this.Type = TokenType.NONE;
            this.Name = name;
            this.LineNum = Linenum;
            this.Scope = scope;
        }

        /// <summary>
        /// create a new expression node
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="Linenum"></param>
        /// <param name="scope"></param>
        public TreeNode(ExpKind kind, TokenType type, string name, int Linenum, string scope)
            : this()
        {
            this.NodeKind = NodeKind.expK;
            this.Kind = kind;
            this.Type = type;
            this.Name = name;
            this.LineNum = Linenum;
            this.Scope = scope;
        }

    }
}
