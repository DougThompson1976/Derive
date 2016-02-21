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

        private void initTaskPane(CustomTaskPane taskPane) {
            ElementHost controlHost = new ElementHost();
            controlHost.Dock = System.Windows.Forms.DockStyle.Fill;
            controlHost.Child = new TaskPaneControl();
            taskPane.Control.Controls.Add(controlHost);
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
