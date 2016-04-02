using Derive.model;
using Derive.view;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Derive.control {
    class RuleListController {

        private TaskPaneControl taskPaneControl;
        private List<Control> addedControls = new List<Control>();
        private Range selection;
        private List<Rule> rules;

        public RuleListController(TaskPaneControl taskPaneControl, Range selection) {
            this.taskPaneControl = taskPaneControl;
            this.selection = selection;

            updateTaskPaneControl();
        }

        private void updateTaskPaneControl() {
            RuleListFormulaParser parser = new RuleListFormulaParser((String)selection.Formula);
            if(! parser.isCorrectlyFormatted()) {
                // TODO fail gracefully
                System.Windows.Forms.MessageBox.Show(parser.ParseException.ToString());
                return;
            }
            int ruleNumber = 1;
            this.rules = parser.Result;
            foreach(Rule rule in rules) {
                addToTaskPaneControl(RulePaneBuilder.create()
                    .name("Rule " + ruleNumber)
                    .condition(rule.Condition, (s) => {
                        updateFormula();
                    })
                    .value(rule.Value, (s) => {
                        updateFormula();
                    })
                    .build());
                ruleNumber++;
            }
        }

        private void addToTaskPaneControl(Control control) {
            this.addedControls.Add(control);
            this.taskPaneControl.addToStack(control);
        }

        public void clearTaskPaneControl() {
            addedControls.ForEach((control) => {
                this.taskPaneControl.removeFromStack(control);
            });
        }

        private void updateFormula() {
            //TODO compose formula from rules, update in selection
        }

    }
}
