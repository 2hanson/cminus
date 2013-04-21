using System;
using System.IO;
using System.Collections;
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
using CMinusClassLibrary;
using CMinusClassLibrary.Lex;
using CMinusClassLibrary.ErrorBase;
using Microsoft.Windows.Controls.Ribbon;
using CMinusClassLibrary.ParseRecursive;
using CMinusClassLibrary.Analyzer;
using CMinusClassLibrary.ASMGenerator;
  
namespace CMinus
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : RibbonWindow
    {
        #region
        List<Token> tokenlist = new List<Token>();
        string path = "";
        TreeNode root = new TreeNode();
        #endregion

        public Window1()
        {
            InitializeComponent();
        }

        private void LoadFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException();
            }
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException();
            }
            using (FileStream stream = File.OpenRead(filename))
            {
                path = filename;
                richTextBox.Document.LineHeight = 5;
                TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                string dataFormat = DataFormats.Text;
                string ext = System.IO.Path.GetExtension(filename);
                documentTextRange.Load(stream, dataFormat);
            }
        }

        private void OnOpenFile(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Text Files (*.cminus)|*.cminus";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                LoadFile(ofd.SafeFileName);
            }
            
        }

        private  void SaveFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException();
            }
            using (FileStream stream = File.OpenWrite(filename))
            {
                path = filename;
                TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                string dataFormat = DataFormats.Text;
                string ext = System.IO.Path.GetExtension(filename);
                documentTextRange.Save(stream, dataFormat);
            }
        }

        private void OnSaveFile(object sender, System.Windows.RoutedEventArgs e)
        {
        	Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Text Files (*.cminus;)|*.cminus";
            if (sfd.ShowDialog() == true)
            { 
                SaveFile(sfd.SafeFileName);
            }
        }

        private void OnNewFile(object sender, System.Windows.RoutedEventArgs e)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.LineHeight = 5;
        }

        private void LexicalAnalysis(object sender, RoutedEventArgs e)
        {
            if (path != "")
            {
                File.WriteAllText(path, "");
                SaveFile(path);
            }
            else
            {
                OnSaveFile(sender, e);
                while (path == null)
                {
                    OnSaveFile(sender, e);
                }
            }
            Lexer lex = new Lexer(path);
            lex.Scan();
            if (ErrorManager.ErrorList.Count == 0)
            {
                error.Visibility = Visibility.Collapsed;
                dg.Visibility = Visibility.Visible;
                sa.Visibility = Visibility.Collapsed;
                tokenlist = lex.TokenList;

                //ErrorManager.ShowError();
                //RankList1.ItemsSource = tokenlist;
                dg.ItemsSource = tokenlist;
            }
            else
            {
                error.Visibility = Visibility.Visible;
                dg.Visibility = Visibility.Collapsed;
                sa.Visibility = Visibility.Collapsed;
                error.ItemsSource = null;
                error.ItemsSource = ErrorManager.ErrorList;
            }
        }

        private void OnCloseApplication(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OnIgnore(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void RibbonCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnCut(object sender, ExecutedRoutedEventArgs e)
        {
            ApplicationCommands.Cut.Execute(FocusManager.GetFocusedElement(this), null);
        }

        private void CanCut(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ApplicationCommands.Cut.CanExecute(FocusManager.GetFocusedElement(this), null);
        }

        private void CanCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ApplicationCommands.Copy.CanExecute(FocusManager.GetFocusedElement(this), null);
        }

        private void CanPaste(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ApplicationCommands.Paste.CanExecute(FocusManager.GetFocusedElement(this), null);
        }

        private void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            ApplicationCommands.Copy.Execute(FocusManager.GetFocusedElement(this), null);
        }

        private void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
            ApplicationCommands.Paste.Execute(FocusManager.GetFocusedElement(this), null);
        }

        private void SyntaxAnalysis(object sender, ExecutedRoutedEventArgs e)
        {
            LexicalAnalysis(sender, e);
            Parser parser = new Parser(tokenlist);
            root = parser.BuildSyntaxTree();
            if (ErrorManager.ErrorList.Count == 0)
            {
                error.Visibility = Visibility.Collapsed;
                SyntaxTree syntaxTree = new SyntaxTree();
                syntaxTree.CreatSyntaxTree(root);
                syntaxTree.Show();
            }
            else
            {
                error.Visibility = Visibility.Visible;
                dg.Visibility = Visibility.Collapsed;
                error.ItemsSource = null;
                error.ItemsSource = ErrorManager.ErrorList;
            }
            

        }

        private void SemanticAnalysis(object sender, ExecutedRoutedEventArgs e)
        {
            LexicalAnalysis(sender, e);
            Analyzer analyzer = new Analyzer(tokenlist);
            analyzer.GetSymbolFile();
            if (ErrorManager.ErrorList.Count == 0)
            {
                dg.Visibility = Visibility.Collapsed;
                sa.Visibility = Visibility.Visible;
                sa.ItemsSource = null;
                sa.ItemsSource = analyzer.GetSymbolTableList();
            }
            else
            {
                error.Visibility = Visibility.Visible;
                dg.Visibility = Visibility.Collapsed;
                sa.Visibility = Visibility.Collapsed;
                error.ItemsSource = null;
                error.ItemsSource = ErrorManager.ErrorList;
            }
        }

        private void TargetCodeGeneration(object sender, ExecutedRoutedEventArgs e)
        {
            LexicalAnalysis(sender, e);

            ASMGenerator asmGenerator = new ASMGenerator(tokenlist);
            asmGenerator.CodeGen();

        }

    }
}
