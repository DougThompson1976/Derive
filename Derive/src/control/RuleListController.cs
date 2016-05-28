using Derive.model;
using Derive.view;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VitaLib.src;

namespace Derive.control {
    class RuleListController {

        #region state

        private TaskPaneControl taskPaneControl;
        private List<RulePane> rulePanes = new List<RulePane>();
        private Boolean clearing = false;
        private VitaLib.src.CollapsablePane defaultValuePane;
        System.Windows.Controls.TextBox defaultValueTextBox;
        private Range selection;

        #endregion

        #region initialisation

        public RuleListController(TaskPaneControl taskPaneControl, Range selection) {
            this.taskPaneControl = taskPaneControl;
            this.taskPaneControl.RulesStack.AddClicked += OnStackAddClicked;
            this.taskPaneControl.RulesStack.ChildRemoved += OnStackChildRemoved;
            this.selection = selection;

            updateTaskPaneControlFromFormula();
        }

        #endregion

        #region event handlers

        public void OnStackAddClicked(object sender, AddClickedEventArgs e) {
            addNewRulePane(e);
            updateFormulaInExcel();
        }

        public void OnStackChildRemoved(object sender, ChildRemovedEventArgs e) {
            if (!clearing && e.RemovedChild is RulePane && rulePanes.Contains((RulePane)e.RemovedChild)) {
                rulePanes.Remove((RulePane)e.RemovedChild);
                updateFormulaInExcel();
            }
        }

        #endregion

        #region external control

        public void addNewRulePane(AddClickedEventArgs e) {
            Control rulePane = getNewRulePane(e);
            if (e.OriginChild.Equals(e.ChildAbove)) { // bovenste child was trigger, en wordt gekopieerd
                this.taskPaneControl.RulesStack.newChild(rulePane)
                    .withRemoveButton()
                    .addUnder(e.OriginChild);
            } else { // onderste child was trigger, en wordt gekopieerd als er geen bovenste child is
                this.taskPaneControl.RulesStack.newChild(rulePane)
                    .withRemoveButton()
                    .addAbove(e.OriginChild);
            }
        }

        private Control getNewRulePane(AddClickedEventArgs e) {
            RulePane triggerRulePane;
            String condition, value;
            if (e.ChildAbove != null) { // er is een control boven de add button, dus zeker een rule pane
                triggerRulePane = e.ChildAbove as RulePane;
                condition = triggerRulePane.Condition;
                value = triggerRulePane.Value;
            } else if (e.ChildUnder is RulePane) { // er is geen rule pane boven de add button, maar wel een eronder
                triggerRulePane = e.ChildUnder as RulePane;
                condition = triggerRulePane.Condition;
                value = triggerRulePane.Value;
            } else { // er is geen rule pane boven of onder de add button
                condition = FormulaElement.Boolean.FALSE;
                value = "some value";
            }
            RulePane newRulePane = getRulePane("New rule", condition, value);
            newRulePane.Collapsed = false;
            return newRulePane;
        }

        public void updateRulesPane(RuleList ruleList) {
            int ruleNumber = 1;
            foreach (Rule rule in ruleList.Rules) {
                RulePane rulePane = getRulePane("Rule" + ruleNumber, rule.Condition, rule.Value);
                // TODO bepaal naam op basis van formule
                this.taskPaneControl.RulesStack.newChild(rulePane)
                    .withRemoveButton()
                    .addToBottom();

                ruleNumber++;
            }

            defaultValueTextBox = new System.Windows.Controls.TextBox();
            defaultValueTextBox.Text = ruleList.DefaultValue;
            defaultValueTextBox.TextChanged += (o, e) => updateFormulaInExcel();
            defaultValuePane = VitaLib.src.TitledCollapsablePaneBuilder.create("Default value")
                .addIndentedChild(defaultValueTextBox)
                .collapsed(false)
                .build();
            this.taskPaneControl.Stack.Children.Add(defaultValuePane);
        }

        public bool taskPaneControlMatches(Range changed) {
            RuleListFormulaParser parser = new RuleListFormulaParser((String)changed.Formula);
            if (!parser.isCorrectlyFormatted()) {
                return false; // an incorrectly parsed formula can only come from outside
            } else {
                return parser.Result.Equals(getShownRuleList());
            }
        }

        public void clearTaskPaneControl() {
            clearing = true; // flag needed to intercept ChildRemoved events from taskPaneControl.Stack
            rulePanes.ForEach((rulePane) => {
                this.taskPaneControl.RulesStack.removeChild(rulePane);
            });
            this.taskPaneControl.Stack.Children.Remove(defaultValuePane);
            rulePanes.Clear();
            clearing = false;
        }

        #endregion

        #region interface with excel

        private void updateTaskPaneControlFromFormula() {
            RuleListFormulaParser parser = new RuleListFormulaParser((String)selection.Formula);
            if(! parser.isCorrectlyFormatted()) {
                // TODO fail gracefully
                System.Windows.Forms.MessageBox.Show(parser.ParseException.ToString());
                return;
            }
            updateRulesPane(parser.Result);
        }

        private void updateFormulaInExcel() {
            RuleListFormulaComposer composer = new RuleListFormulaComposer(getShownRuleList());
            if (!composer.isCorrectlyFormatted()) {
                // do nothing
            } else {
                selection.Formula = composer.Result;
            }
        }

        #endregion

        #region utils

        private RulePane getRulePane(String name, String condition, String value) {
            RulePane rulePane = new RulePane(name, condition, value);
            rulePane.ConditionChange += (o, e) => updateFormulaInExcel();
            rulePane.ValueChange += (o, e) => updateFormulaInExcel();
            this.rulePanes.Add(rulePane);
            return rulePane;
        }

        private RuleList getShownRuleList() {
            List<Rule> rules = rulePanes.ConvertAll<Rule>((rulePane) => {
                return new Rule(rulePane.Condition, rulePane.Value);
            });
            String defaultValue = defaultValueTextBox.Text;
            return new RuleList(rules, defaultValue);
        }

        public void dispose() {
            clearTaskPaneControl();
            this.taskPaneControl.RulesStack.AddClicked -= OnStackAddClicked;
            this.taskPaneControl.RulesStack.ChildRemoved -= OnStackChildRemoved;
        }

        #endregion

    }
}
