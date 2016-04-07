using Derive.view;
using Microsoft.Office.Tools;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace Derive.control
{
    class DeriveController
    {
        #region state

        private static DeriveController instance;
        public static DeriveController Instance {
            get {
                if (instance == null) {
                    throw new NullReferenceException("No ExcelController was created yet.");
                }
                return instance;
            }
        }

        private CustomTaskPane taskPane;
        private TaskPaneControl taskPaneControl;
        private RuleListController ruleListController;

        private RibbonAddition ribbonAddition;

        #endregion

        #region workbook events

        public void onSelectionChange(Range target) {
            updateRuleList(target);
        }

        public void onContentsChange(Range changed) {
            if (this.ruleListController != null && ! ruleListController.taskPaneControlMatches(changed)) {
                updateRuleList(changed);
            }
        }

        private void updateRuleList(Range cells) {
            if (this.ruleListController != null) {
                this.ruleListController.clearTaskPaneControl();
            }
            this.ruleListController = new RuleListController(taskPaneControl, cells);
        }

        #endregion

        #region start and stop

        internal DeriveController(CustomTaskPane taskPane, RibbonAddition ribbonAddition) {
            if (instance != null) {
                throw new Exception("An ExcelController was already created, and only one is allowed.");
            }
            instance = this;

            this.taskPane = taskPane;
            this.ribbonAddition = ribbonAddition;
            initTaskPane(taskPane);
        }

        internal void shutDown() {
            // TODO
        }

        #endregion

        #region taskpane

        /**
         * The structure of the task pane will be as folows:
         *    CustomTaskPane taskPane (created and added in ThisAddIn.createTaskPane())
         *      UserControl winFormsProxyUserControl (created and added in ThisAddIn.createTaskPane() on taskPane creation)
         *        ElementHost wpfControlHost (created and added here to winFormsProxyUserControl.Controls)
         *          TaskPaneControl taskPaneControl (created and added here as wpfControlHost.Child)
         */
        private void initTaskPane(CustomTaskPane taskPane) {
            // create taskPaneControl
            this.taskPaneControl = new TaskPaneControl();
            // create wpfControlHost and add taskPaneControl
            ElementHost wpfControlHost = new ElementHost();
            wpfControlHost.Dock = System.Windows.Forms.DockStyle.Fill;
            wpfControlHost.Child = taskPaneControl;
            // add wpfControlHost to winFormsProxyUserControl
            taskPane.Control.Controls.Add(wpfControlHost);
            // set taskPane visibility handler
            taskPane.VisibleChanged += onTaskPaneVisibilityChanged;
        }

        void onTaskPaneVisibilityChanged(object sender, EventArgs e) {
            if (taskPane.Visible) {
                checkRibbonDeriveButton();
            }
            else {
                uncheckRibbonDeriveButton();
            }
        }

        internal void showTaskPane() {
            taskPane.Visible = true;
        }

        internal void hideTaskPane() {
            taskPane.Visible = false;
        }

        #endregion

        #region ribbon

        internal void checkRibbonDeriveButton() {
            ribbonAddition.checkButtonDerive();
        }

        internal void uncheckRibbonDeriveButton() {
            ribbonAddition.uncheckButtonDerive();
        }

        #endregion
    }
}
