namespace CMinus
{
    using System.Windows.Input;
    using System.Windows;
    using Microsoft.Windows.Controls.Ribbon;
    /// <summary>
    /// This class holds the global commands used by the application
    /// </summary>
    public static class AppCommands
    {
        #region RibbonCommands
        public static RibbonCommand Help
        {
            get { return (RibbonCommand)Application.Current.Resources["HelpCommand"]; }
        }
        public static RibbonCommand Cut
        {
            get { return (RibbonCommand)Application.Current.Resources["CutCommand"]; }
        }
        public static RibbonCommand Copy
        {
            get { return (RibbonCommand)Application.Current.Resources["CopyCommand"]; }
        }
        public static RibbonCommand Paste
        {
            get { return (RibbonCommand)Application.Current.Resources["PasteCommand"]; }
        }
        public static RibbonCommand AddNew
        {
            get { return (RibbonCommand)Application.Current.Resources["AddNewCommand"]; }
        }
        public static RibbonCommand Open
        {
            get { return (RibbonCommand)Application.Current.Resources["OpenCommand"]; }
        }
        public static RibbonCommand Save
        {
            get { return (RibbonCommand)Application.Current.Resources["SaveCommand"]; }
        }
        //public static RibbonCommand Reconcile
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["ReconcileCommand"]; }
        //}
        //public static RibbonCommand CashflowReport
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["CashflowReportCommand"]; }
        //}
        public static RibbonCommand SyntaxAnalysis
        {
            get { return (RibbonCommand)Application.Current.Resources["SyntaxAnalysisCommand"]; }
        }
        public static RibbonCommand LexicalAnalysis
        {
            get { return (RibbonCommand)Application.Current.Resources["LexicalAnalysisCommand"]; }
        }
        public static RibbonCommand SemanticAnalysis
        {
            get { return (RibbonCommand)Application.Current.Resources["SemanticAnalysisCommand"]; }
        }
        public static RibbonCommand TargetCodeGeneration
        {
            get { return (RibbonCommand)Application.Current.Resources["TargetCodeGenerationCommand"]; }
        }
        //public static RibbonCommand DownloadCreditCards
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["DownloadCreditCardsCommand"]; }
        //}
        //public static RibbonCommand Transfer
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["TransferCommand"]; }
        //}
        //public static RibbonCommand Backup
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["BackupCommand"]; }
        //}
        //public static RibbonCommand Calculator
        //{
        //    get { return (RibbonCommand)Application.Current.Resources["CalculatorCommand"]; }
        //}
        #endregion
    }
}
