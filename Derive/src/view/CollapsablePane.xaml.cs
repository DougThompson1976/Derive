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

namespace Derive.view {
    /// <summary>
    /// Interaction logic for CollapsablePane.xaml
    /// </summary>
    public partial class CollapsablePane : UserControl {

        #region constants

        private static readonly double INDENT_DEPTH = 15;

        #endregion

        #region properties

        public bool Collapsed {
            get {
                return this.contentPanel.Visibility == Visibility.Collapsed;
            }
            set {
                this.collapseIcon.RenderTransform = new RotateTransform(
                    value ? 0 : 90,
                    collapseIcon.Width / 2,
                    collapseIcon.Height / 2
                );
                this.contentPanel.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                OnCollapseChange(new CollapseChangeEventArgs(value));
            }
        }

        #endregion

        public CollapsablePane() {
            InitializeComponent();

            Collapsed = true;
        }

        #region control

        public void addTitle(Control childControl) {
            this.titlePanel.Children.Add(childControl);
        }

        public void addChild(Control childControl) {
            this.contentPanel.Children.Add(childControl);
        }

        public void addIndentedChild(Control childControl) {
            addChild(childControl);
            childControl.Margin = new Thickness(INDENT_DEPTH,
                childControl.Margin.Top, childControl.Margin.Right, childControl.Margin.Bottom);
        }

        #endregion

        #region internal event handling

        private void onCollapseIconMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Collapsed = !Collapsed;
        }

        #endregion

        #region events

        public event CollapseChangeEventHandler CollapseChange;
        protected virtual void OnCollapseChange(CollapseChangeEventArgs e) {
            if (CollapseChange != null) CollapseChange(this, e);
        }

        #endregion
    }

    public delegate void CollapseChangeEventHandler(object sender, CollapseChangeEventArgs e);

    public class CollapseChangeEventArgs : EventArgs {

        private readonly bool collapsed;
        public bool Collapsed {
            get { return collapsed; }
        }

        public CollapseChangeEventArgs(bool collapsed) {
            this.collapsed = collapsed;
        }
    }
}
