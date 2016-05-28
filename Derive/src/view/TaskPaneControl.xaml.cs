using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VitaLib.src;

namespace Derive.view {
    /// <summary>
    /// Interaction logic for TaskPaneControl.xaml
    /// </summary>
    public partial class TaskPaneControl : UserControl {

        public StackPanel Stack {
            /**
             * The stack that contains the rule panes and the default value pane
             */
            get {
                return this.stack;
            }
        }

        public EditableStackPanel RulesStack {
            /**
             * The stack that only contains the rule panes, not the default value pane
             */
            get {
                return this.rulesStack;
            }
        }

        public TaskPaneControl() {
            InitializeComponent();
        }
    }
}
