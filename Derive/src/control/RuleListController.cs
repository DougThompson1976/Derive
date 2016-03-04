using Derive.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Derive.control {
    class RuleListController {

        public TaskPaneControl taskPaneControl;

        public RuleListController(TaskPaneControl taskPaneControl) {
            this.taskPaneControl = taskPaneControl;

            updateTaskPaneControl();
        }

        private void updateTaskPaneControl() {
            CollapsablePane conditionPane = TitledCollapsablePaneBuilder
                .create("Condition")
                .addIndentedChild(new TextBox())
                .collapsed(true)
                .build();

            CollapsablePane valuePane = TitledCollapsablePaneBuilder
                .create("Value")
                .addIndentedChild(new TextBox())
                .collapsed(true)
                .build();

            CollapsablePane formattingPane = TitledCollapsablePaneBuilder
                .create("Formatting")
                .addIndentedChild(new TextBox())
                .collapsed(true)
                .build();

            CollapsablePane rulePane = TitledCollapsablePaneBuilder
                .create("Rule")
                .collapsed(false)
                .addIndentedChild(conditionPane)
                .addIndentedChild(valuePane)
                .addIndentedChild(formattingPane)
                .build();

            taskPaneControl.addToStack(rulePane);

            CollapsablePane secondRulePane = TitledCollapsablePaneBuilder
                .create("Rule 2")
                .collapsed(true)
                .addIndentedChild(new TextBox())
                .build();

            taskPaneControl.addToStack(secondRulePane);
        }

    }
}
