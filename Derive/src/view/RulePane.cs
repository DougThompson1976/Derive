using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Derive.view {
    class RulePane : VitaLib.src.CollapsablePane {

        private Label titleLabel;
        private TextBox conditionTextBox;
        private TextBox valueTextBox;

        public String RuleName {
            get {
                return (String) titleLabel.Content;
            }
            set {
                titleLabel.Content = value;
            }
        }
        public String Condition {
            get {
                return conditionTextBox.Text;
            }
            set {
                conditionTextBox.Text = value;
            }
        }
        public String Value {
            get {
                return valueTextBox.Text;
            }
            set {
                valueTextBox.Text = value;
            }
        }

        public RulePane(String ruleName, String condition, String value) {
            titleLabel = new Label();
            RuleName = ruleName;

            conditionTextBox = new TextBox();
            Condition = condition;

            valueTextBox = new TextBox();
            Value = value;

            init();
        }

        private void init() {
            // title
            addTitle(titleLabel);
            // condition
            conditionTextBox.TextChanged += (o, e) => OnConditionChange(new ConditionChangeEventArgs(conditionTextBox.Text));
            addIndentedChild(VitaLib.src.TitledCollapsablePaneBuilder.create("Condition")
                .addIndentedChild(conditionTextBox)
                .collapsed(false)
                .build());
            // value
            valueTextBox.TextChanged += (o, e) => OnValueChange(new ValueChangeEventArgs(valueTextBox.Text));
            addIndentedChild(VitaLib.src.TitledCollapsablePaneBuilder.create("Value")
                .addIndentedChild(valueTextBox)
                .collapsed(false)
                .build());
        }

        #region events

        public event ConditionChangeEventHandler ConditionChange;
        protected virtual void OnConditionChange(ConditionChangeEventArgs e) {
            if (ConditionChange != null) ConditionChange(this, e);
        }

        public event ValueChangeEventHandler ValueChange;
        protected virtual void OnValueChange(ValueChangeEventArgs e) {
            if (ValueChange != null) ValueChange(this, e);
        }

        #endregion
    }

    public delegate void ConditionChangeEventHandler(object sender, ConditionChangeEventArgs e);

    public class ConditionChangeEventArgs : EventArgs {

        private readonly String condition;
        public String Condition {
            get { return condition; }
        }

        public ConditionChangeEventArgs(String condition) {
            this.condition = condition;
        }

    }

    public delegate void ValueChangeEventHandler(object sender, ValueChangeEventArgs e);

    public class ValueChangeEventArgs : EventArgs {

        private readonly String value;
        public String Value {
            get { return value; }
        }

        public ValueChangeEventArgs(String Value) {
            this.value = Value;
        }

    }
}
