using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CMinusClassLibrary.ParseRecursive;

namespace CMinus
{
	/// <summary>
	/// Interaction logic for SyntaxTree.xaml
	/// </summary>
	public partial class SyntaxTree : Window
    {
        #region
        private int treeNodeCount;
        #endregion
        public SyntaxTree()
		{
			this.InitializeComponent();
            treeNodeCount = -1;
		}

        /// <summary>
        /// 产生语法树，传入虚拟根节点，由他扩展。
        /// </summary>
        /// <param name="vitualRoot"></param>
        public void CreatSyntaxTree(TreeNode vitualRoot)
        {
            TreeContainer.TreeNode treeRoot;
            //命名树节点
            treeRoot = CreaterTreeNode(vitualRoot);
            treeRoot = Tree.AddRoot(treeRoot);
            BuildingTree(vitualRoot, treeRoot, null);
        }

        /// <summary>
        /// 递归生成语法树，节点的名字从0开始编号，显示的内容不是这些数字
        /// </summary>
        /// <param name="TN"></param>
        /// <param name="FatherNode"></param>
        private void BuildingTree(TreeNode TN, TreeContainer.TreeNode CurrentNode, TreeContainer.TreeNode FatherNode)
        {
            TreeContainer.TreeNode temp, tempFatherNode;
            //生成所有的孩子节点
            for (int i = 0; i < 4; ++i) {
                if (TN.Child[i] == null) {
                    continue;
                }
                temp = CreaterTreeNode(TN.Child[i]);
                temp = Tree.AddNode(temp, CurrentNode);
                tempFatherNode = CurrentNode;
                BuildingTree(TN.Child[i], temp, tempFatherNode);
            }
            //生成兄弟节点
            TreeNode tempTN = TN;
            //开始用while，错了，改成if。
            if (tempTN.Sibling != null)
            {
                tempTN = tempTN.Sibling;

                temp = CreaterTreeNode(tempTN);
                temp = Tree.AddNode(temp, FatherNode);
                BuildingTree(tempTN, temp, FatherNode);
            }
        }

        /// <summary>
        /// 生成一个显示树的节点，给它付名字（0开始变化的数字），和内容
        /// </summary>
        /// <param name="LogicalTreeNode"></param>
        /// <returns></returns>
        private TreeContainer.TreeNode CreaterTreeNode(TreeNode LogicalTreeNode)
        {
            ++treeNodeCount;
            TreeContainer.TreeNode tempTreeNode = new TreeContainer.TreeNode();
            TextBlock NodeName = new TextBlock();
            NodeName.Width = 75;
            NodeName.Height = 30;
            NodeName.TextWrapping = TextWrapping.Wrap;
            NodeName.FontSize = 18;
            NodeName.FontWeight = FontWeights.Bold;
            NodeName.TextAlignment = TextAlignment.Center;
            NodeName.FontFamily = new FontFamily("Comic Sans MS");
            NodeName.Text = LogicalTreeNode.Name;

            ToolTip toolTip = new ToolTip();
            SyntaxTreeNodeToolTip toolTipContent = new SyntaxTreeNodeToolTip();

            String NodeKindName = "";
            switch (LogicalTreeNode.NodeKind)
            {
                case NodeKind.proK:
                    NodeKindName = "Program";
                    break;
                case NodeKind.funK:
                    NodeKindName = "Function declaration";
                    break;
                case NodeKind.paramK:
                    NodeKindName = "Parameter";
                    break;
                case NodeKind.varK:
                    NodeKindName = "Variable declaration";
                    break;
                case NodeKind.stmtK:
                    switch ((StmtKind)LogicalTreeNode.Kind)
                    {
                        case StmtKind.ifK:
                            NodeKindName = "If";
                            break;
                        case StmtKind.readK:
                            NodeKindName = "Call";
                            break;
                        case StmtKind.writeK:
                            NodeKindName = "Call";
                            break;
                        case StmtKind.returnK:
                            NodeKindName = "Return";
                            break;
                        case StmtKind.callK:
                            NodeKindName = "Call";
                            break;
                    }
                    break;
                case NodeKind.expK:
                    switch ((ExpKind)LogicalTreeNode.Kind)
                    {
                        case ExpKind.ConstK:
                            NodeKindName = "const";
                            break;
                        case ExpKind.IDK:
                            NodeKindName = "ID";
                            break;
                        case ExpKind.OpK:
                            NodeKindName = "Operater";
                            break;
                    }
                    break;
            }
            toolTipContent.NodeKindContent.Text = NodeKindName;

            toolTipContent.ScopeContent.Text = LogicalTreeNode.Scope;
            
            String TypeName ="";
            if(Convert.ToInt16(LogicalTreeNode.Type)<=255)
                TypeName = ((char)(Convert.ToInt16(LogicalTreeNode.Type))).ToString();
            else
            {
                switch(Convert.ToInt16(LogicalTreeNode.Type))
                {
                    case 256:
                        TypeName =  "ELSE";
                        break;
                    case 257:
                        TypeName =  "EQUAL";
                        break;
                    case 258:
                        TypeName =  "GREATERQUAL";
                        break;
                    case 259:
                        TypeName =  "ID";
                           break;
                    case 260:
                        TypeName =  "IF";
                        break;
                    case 261:
                        TypeName =  "INT";
                        break;
                    case 262:
                        TypeName =  "LEFTCOMMON";
                        break;
                    case 263:
                        TypeName =  "LESSEQUAL";
                        break;
                    case 264:
                        TypeName =  "MINUS";
                        break;
                    case 265:
                        TypeName =  "NOTEQUAL";
                        break;
                    case 266:
                        TypeName =  "NUMBER";
                        break;
                    case 267:
                        TypeName =  "RETURN";
                        break;
                    case 268:
                        TypeName =  "RIGHTCOMMON";
                        break;
                    case 269:
                        TypeName =  "WHILE";
                        break;
                    case 270:
                        TypeName =  "VOID";
                        break;
                    case 271:
                        TypeName =  "EOF";
                        break;
                    case 272:
                        TypeName =  "NONE";
                        break;
                    case 273:
                        TypeName =  "READ";
                        break;
                    case 274:
                        TypeName =  "WRITE";
                        break;
                }
            }       
        
            toolTipContent.TypeContent.Text = TypeName;
            toolTipContent.IsArrayConent.Text = LogicalTreeNode.IsArray.ToString();
            toolTipContent.ArraySizeContent.Text = LogicalTreeNode.ArraySize.ToString();
            toolTipContent.LineNumberContent.Text = LogicalTreeNode.LineNum.ToString();

            toolTip.Background = Brushes.Transparent;
            toolTip.BorderThickness = new Thickness(0);
            toolTip.Content = toolTipContent;
            NodeName.ToolTip = toolTip;
            tempTreeNode.Content = NodeName;
            tempTreeNode.Name = "node" + treeNodeCount.ToString();
            return tempTreeNode;
        }
	}
}
