using Derive.control;
using Derive.res;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;

namespace Derive.view
{
    [ComVisible(true)]
    public class RibbonAddition : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;
        private bool buttonDeriveChecked;

        public RibbonAddition() {
            // empty constructor
        }

        #region control

        internal void checkButtonDerive() {
            buttonDeriveChecked = true;
            this.ribbon.InvalidateControl("ButtonDerive");
        }

        internal void uncheckButtonDerive() {
            buttonDeriveChecked = false;
            this.ribbon.InvalidateControl("ButtonDerive");
        }

        #endregion

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID) {
            return GetResourceText("Derive.src.view.RibbonAddition.xml");
        }

        #endregion

        #region callbacks

        public void onRibbonLoad(Office.IRibbonUI ribbonUI) {
            this.ribbon = ribbonUI;
            buttonDeriveChecked = false;
        }

        public String getButtonDeriveLabel(Office.IRibbonControl control) {
            return language.ribbon_deriveButton_label;
        }

        public void onButtonDeriveAction(Office.IRibbonControl control, bool pressed) {
            if(pressed) {
                buttonDeriveChecked = true;
                DeriveController.Instance.showTaskPane();
            }
            else {
                buttonDeriveChecked = false;
                DeriveController.Instance.hideTaskPane();
            }
        }

        public bool getButtonDerivePressed(Office.IRibbonControl control) {
            return buttonDeriveChecked;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName) {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i) {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0) {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i]))) {
                        if (resourceReader != null) {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
