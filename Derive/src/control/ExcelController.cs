using Derive.view;
using Microsoft.Office.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;

namespace Derive.control
{
    class ExcelController
    {
        #region state

        private static ExcelController instance;
        public static ExcelController Instance {
            get {
                if (instance == null) {
                    throw new NullReferenceException("No ExcelController was created yet.");
                }
                return instance;
            }
        }

        private CustomTaskPane taskPane;
        private TaskPaneControl taskPaneControl;
        private RibbonAddition ribbonAddition;

        #endregion

        #region start and stop

        internal ExcelController(CustomTaskPane taskPane, RibbonAddition ribbonAddition) {
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
         *      UserControl winFormsProxyUserControl (created and added in ThisAddIn.createTaskPane())
         *        ElementHost wpfControlHost (created and added here)
         *          TaskPaneControl taskPaneControl (created and added here)
         */
        private void initTaskPane(CustomTaskPane taskPane) {
            // create taskPaneControl
            this.taskPaneControl = new TaskPaneControl();
            // create wpfControlHost and add taskPaneControl
            ElementHost wpfControlHost = new ElementHost();
            wpfControlHost.Dock = System.Windows.Forms.DockStyle.Fill;
            wpfControlHost.Child = taskPaneControl;
            // add wpfControlHost to taskPaneControl
            taskPane.Control.Controls.Add(wpfControlHost);
            // set taskPane visibility handler
            taskPane.VisibleChanged += taskPaneVisibleChanged;
        }

        void taskPaneVisibleChanged(object sender, EventArgs e) {
            if (taskPane.Visible) {
                ribbonAddition.checkButtonDerive();
            }
            else {
                ribbonAddition.uncheckButtonDerive();
            }
        }

        internal void showTaskPane() {
            taskPane.Visible = true;
        }

        internal void hideTaskPane() {
            taskPane.Visible = false;
        }

        #endregion
    }
}
