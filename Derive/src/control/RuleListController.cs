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
        private List<RulePane> rulePanes = new List<RulePane>();
        private Boolean clearing = false;
        private VitaLib.src.CollapsablePane defaultValuePane;
        System.Windows.Controls.TextBox defaultValueTextBox;
        private Range selection;

        public RuleListController(TaskPaneControl taskPaneControl, Range selection) {
            this.taskPaneControl = taskPaneControl;
            this.taskPaneControl.Stack.ChildRemoved += (o, e) => {
                if (!clearing && e.RemovedChild is RulePane && rulePanes.Contains((RulePane)e.RemovedChild)) {
                    rulePanes.Remove((RulePane)e.RemovedChild);
                    updateFormula();
                }
            };
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
            foreach(Rule rule in parser.Result.Rules) {
                RulePane rulePane = new RulePane("Rule" + ruleNumber, rule.Condition, rule.Value);
                rulePane.ConditionChange += (o, e) => updateFormula();
                rulePane.ValueChange += (o, e) => updateFormula();

                this.rulePanes.Add(rulePane);
                this.taskPaneControl.Stack.newChild(rulePane)
                    .withRemoveButton()
                    .add();

                ruleNumber++;
            }

            defaultValueTextBox = new System.Windows.Controls.TextBox();
            defaultValueTextBox.Text = parser.Result.DefaultValue;
            defaultValueTextBox.TextChanged += (o, e) => updateFormula();
            defaultValuePane = VitaLib.src.TitledCollapsablePaneBuilder.create("Default value")
                .addIndentedChild(defaultValueTextBox)
                .collapsed(false)
                .build();
            this.taskPaneControl.Stack.newChild(defaultValuePane).add();
        }

        public void clearTaskPaneControl() {
            clearing = true; // flag needed to intercept ChildRemoved events from taskPaneControl.Stack
            rulePanes.ForEach((rulePane) => {
                this.taskPaneControl.Stack.removeChild(rulePane);
            });
            this.taskPaneControl.Stack.removeChild(defaultValuePane);
            rulePanes.Clear();
            clearing = false;
        }

        public bool taskPaneControlMatches(Range changed) {
            RuleListFormulaParser parser = new RuleListFormulaParser((String)changed.Formula);
            if (!parser.isCorrectlyFormatted()) {
                return false; // an incorrectly parsed formula can only come from outside
            } else {
                return parser.Result.Equals(getShownRuleList());
            }
        }

        private void updateFormula() {
            RuleListFormulaComposer composer = new RuleListFormulaComposer(getShownRuleList());
            if(! composer.isCorrectlyFormatted()) {
                // do nothing
            } else {
                selection.Formula = composer.Result;
            }
        }

        private RuleList getShownRuleList() {
            List<Rule> rules = rulePanes.ConvertAll<Rule>((rulePane) => {
                return new Rule(rulePane.Condition, rulePane.Value);
            });
            String defaultValue = defaultValueTextBox.Text;
            return new RuleList(rules, defaultValue);
        }
    }
}
