using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VitaLib.src {
    public class TitledCollapsablePaneBuilder {

        private CollapsablePane pane;
        private String title;
        private bool titleTextEditable = false;

        private TitledCollapsablePaneBuilder(CollapsablePane pane, String title) {
            this.pane = pane;
            this.title = title;
        }

        public static TitledCollapsablePaneBuilder create(String title) {
            CollapsablePane pane = new CollapsablePane();

            return new TitledCollapsablePaneBuilder(pane, title);
        }

        #region configuration

        public TitledCollapsablePaneBuilder collapsed(bool collapsed) {
            this.pane.Collapsed = collapsed;
            return this;
        }

        public TitledCollapsablePaneBuilder titleEditable(bool editable) {
            this.titleTextEditable = editable;
            return this;
        }

        public TitledCollapsablePaneBuilder addIndentedChild(Control childControl) {
            pane.addIndentedChild(childControl);
            return this;
        }

        public TitledCollapsablePaneBuilder addChild(Control childContol) {
            pane.addChild(childContol);
            return this;
        }

        #endregion

        private void addEditableLabelToTitle() {
            // create, fill and add label
            Label label = new Label();
            label.Content = title;
            pane.addTitle(label);
            // create, fill and add textbox
            TextBox textBox = new TextBox();
            textBox.Text = title;
            pane.addTitle(textBox);
            // match textbox layout with label
            textBox.Width = label.Width;
            textBox.Height = label.Height;
            textBox.Margin = new Thickness(
                label.Margin.Left + 2,
                label.Margin.Top + 4,
                label.Margin.Right,
                label.Margin.Bottom + 4
            );
            // hide textbox
            textBox.Visibility = Visibility.Hidden;

            // define enter textbox action
            Action enterTextbox = () => {
                // switch visibilities
                textBox.Visibility = Visibility.Visible;
                label.Visibility = Visibility.Hidden;
                // focus on textbox
                pane.Dispatcher.BeginInvoke(new Action(() => textBox.Focus()),
                    System.Windows.Threading.DispatcherPriority.Input);
                // set caret and selection
                textBox.CaretIndex = textBox.Text.Length;
                textBox.SelectionStart = 0;
                textBox.SelectionLength = textBox.Text.Length;
            };
            // define leaveTextbox action
            Action leaveTextbox = () => {
                // switch visibilities
                textBox.Visibility = Visibility.Hidden;
                label.Visibility = Visibility.Visible;
            };

            // wire label double mouse click to enter texbox action
            label.MouseDoubleClick += (o, e) => {
                if (titleTextEditable) {
                    enterTextbox();
                } else {
                    pane.Collapsed = !pane.Collapsed;
                }
            };
            // wire textbox input
            textBox.KeyDown += (o, e) => {
                if (e.Key == System.Windows.Input.Key.Enter) {
                    label.Content = textBox.Text;
                    leaveTextbox();
                } else if (e.Key == System.Windows.Input.Key.Escape) {
                    leaveTextbox();
                }
            };
            // wire texbox lost focus to leave textbox
            textBox.LostFocus += (o, e) => leaveTextbox();
        }

        public CollapsablePane build() {
            addEditableLabelToTitle();

            return pane;
        }

    }
}