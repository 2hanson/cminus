using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMinusClassLibrary.Lex;

namespace CMinusClassLibrary.Parse
{
    public static class OperateTree
    {
        /********************************************************
         *********以下是创建语法树所用的各类节点的申请***********
         ********************************************************/

        /********************************************************/
        /* 函数名 newRootNode									*/
        /* 功  能 创建语法树根节点函数			        		*/
        /* 说  明 该函数为语法树创建一个新的根结点      		*/
        /*        并将语法树节点成员初始化						*/
        /********************************************************/
        public static TreeNode NewRootNode()
        {
            TreeNode t = new TreeNode();
            /* 指定新语法树节点t成员:结点类型nodekind为语句类型ProK */
            t.NodeKind = NodeKind.proK;
            /* 指定新语法树节点t成员:源代码行号lineno为全局变量lineno */
            t.LineNum = -1;
            t.LevelNum = 0;
            t.PositionInLevel = 1;
            /* 函数返回语法树根节点指针t */
            return t;

        }

       
}
