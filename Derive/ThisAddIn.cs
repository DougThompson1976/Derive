using Derive.control;
using Derive.res;
using Derive.view;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Derive
{
    public partial class ThisAddIn
    {
        private DeriveController deriveController;
        private RibbonAddition ribbonAddition;
        private Workbook workbook;

        #region start and stop

        // is called when a new instance of Excel is activated
        private void startUp(object sender, System.EventArgs e)
        {
            setCulture();
            deriveController = new DeriveController(createTaskPane(), ribbonAddition);

            Application.EnableEvents = true;
            Application.WorkbookActivate += (workbook) => {
                this.workbook = workbook;
                workbook.SheetSelectionChange += (o, r) => {
                    deriveController.onSelectionChange(r);
                };
                workbook.SheetChange += (o, r) => {
                    deriveController.onContentsChange(r);
                };
            };
        }

        void Application_WorkbookActivate(Microsoft.Office.Interop.Excel.Workbook Wb) {
            throw new System.NotImplementedException();
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
            CustomTaskPane taskPane = this.CustomTaskPanes.Add(winFormsProxyUserControl, language.taskpane_title);
            // return taskPane
            return taskPane;
        }

        private void shutDown(object sender, System.EventArgs e) {
            deriveController.shutDown();
            Dispatcher.CurrentDispatcher.InvokeShutdown();
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
