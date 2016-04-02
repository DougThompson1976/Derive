using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Derive.view {
    class RulePaneBuilder {

        private String ruleName;
        private CollapsablePane conditionPane;
        private CollapsablePane valuePane;

        private RulePaneBuilder() {
            // do nothing
        }

        public static RulePaneBuilder create() {
            return new RulePaneBuilder();
        }

        #region configuration

        public RulePaneBuilder name(String name) {
            this.ruleName = name;

            return this;
        }

        public RulePaneBuilder condition(String initialCondition, Action<String> conditionChanged) {
            TextBox textbox = new TextBox();
            textbox.Text = initialCondition;
            textbox.TextChanged += (o, e) => conditionChanged(textbox.Text);
            conditionPane = TitledCollapsablePaneBuilder.create("Condition")
                .addIndentedChild(textbox)
                .collapsed(false)
                .build();

            return this;
        }

        public RulePaneBuilder value(String initialValue, Action<String> valueChanged) {
            TextBox textbox = new TextBox();
            textbox.Text = initialValue;
            textbox.TextChanged += (o, e) => valueChanged(textbox.Text);
            valuePane = TitledCollapsablePaneBuilder.create("Value")
                .addIndentedChild(textbox)
                .collapsed(false)
                .build();

            return this;
        }

        #endregion

        public CollapsablePane build() {
            if (ruleName == null) throw new NullReferenceException("The name for the rule must be set.");
            if (conditionPane == null) throw new NullReferenceException("The condition for the rule must be set.");
            if (valuePane == null) throw new NullReferenceException("The value for the rule must be set.");

            return TitledCollapsablePaneBuilder.create(ruleName)
                .collapsed(false)
                .addIndentedChild(conditionPane)
                .addIndentedChild(valuePane)
                .build();
        }

    }
}
