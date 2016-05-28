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

namespace VitaLib.src {

    public partial class AddInBetweenButton : UserControl {

        private const int PLUS_IMAGE_WIDTH = 17;

        public AddInBetweenButton(int lineLeftX = 0,
            int lineRightX = PLUS_IMAGE_WIDTH,
            Action onPlusMouseUp = null) {
            InitializeComponent();
            this.leftSpace.Width = lineLeftX;
            this.rightSpace.Width = lineRightX - plusImage.Width;
            if (onPlusMouseUp != null) {
                plusImage.MouseUp += (s, e) => onPlusMouseUp();
            }
        }
    }
}
