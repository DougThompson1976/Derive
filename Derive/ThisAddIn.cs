using Derive.control;
using Derive.res;
using Derive.view;
using Microsoft.Office.Tools;
using System.Windows.Forms;

namespace Derive
{
    public partial class ThisAddIn
    {
        private ExcelController excelController;
        private RibbonAddition ribbonAddition;

        #region start and stop

        private void startUp(object sender, System.EventArgs e)
        {
            setCulture();
            excelController = new ExcelController(createTaskPane(), ribbonAddition);
        }

        private void setCulture() {
            System.Threading.Thread.CurrentThread.CurrentUICulture =
                new System.Globalization.CultureInfo(
                    Application.LanguageSettings.get_LanguageID(
                        Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI));
            //TODO dit zet de culture in en-US ipv de verwachte culture...
        }

        private CustomTaskPane createTaskPane() {
            // create winFormsProxyUserControl
            UserControl winFormsProxyUserControl = new UserControl();
            // create taskPane and add winFormsProxyUserControl
            CustomTaskPane taskPane = this.CustomTaskPanes.Add(winFormsProxyUserControl, language.TaskPaneTitle);
            // return taskPane
            return taskPane;
        }

        private void shutDown(object sender, System.EventArgs e) {
            excelController.shutDown();
        }

        #endregion

        #region ribbon

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject() {
            this.ribbonAddition = new RibbonAddition();
            return this.ribbonAddition;
        }

        #endregion

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup() {
            this.Startup += new System.EventHandler(startUp);
            this.Shutdown += new System.EventHandler(shutDown);
        }
        
        #endregion
    }
}
