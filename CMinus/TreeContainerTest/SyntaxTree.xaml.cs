using System;
using System.Collections.Generic;
using System.Linq;
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
using TreeContainer;
using GraphLayout;
using CMinusClassLibrary.ParseRecursive;

namespace TreeContainerTest
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class winTreeContainerTest : Window
	{
		public winTreeContainerTest()
		{
			InitializeComponent();
            
            Button b = new Button();
            b.Height = 50;
            b.Width = 50;
            b.Content = "XYZ";
            A.Content = b;
            A.TreeParent = "O";
           
            
		}
        public void CreatSyntaxTree()
        {

        }
	}
}
