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
        private CollapsablePane defaultValuePane;
        System.Windows.Controls.TextBox defaultValueTextBox;
        private Range selection;

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
            foreach(Rule rule in parser.Result.Rules) {
                RulePane rulePane = new RulePane("Rule" + ruleNumber, rule.Condition, rule.Value);
                rulePane.ConditionChange += (o, e) => updateFormula();
                rulePane.ValueChange += (o, e) => updateFormula();

                this.rulePanes.Add(rulePane);
                this.taskPaneControl.addToStack(rulePane);

                ruleNumber++;
            }

            defaultValueTextBox = new System.Windows.Controls.TextBox();
            defaultValueTextBox.Text = parser.Result.DefaultValue;
            defaultValueTextBox.TextChanged += (o, e) => updateFormula();
            defaultValuePane = TitledCollapsablePaneBuilder.create("Default value")
                .addIndentedChild(defaultValueTextBox)
                .collapsed(false)
                .build();
            this.taskPaneControl.addToStack(defaultValuePane);
        }

        public void clearTaskPaneControl() {
            rulePanes.ForEach((rulePane) => {
                this.taskPaneControl.removeFromStack(rulePane);
            });
            this.taskPaneControl.removeFromStack(defaultValuePane);
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
