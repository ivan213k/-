using System.Windows;
using System.Windows.Controls;

namespace Управление_заказами.Views
{
    /// <summary>
    /// Interaction logic for CheckEquipmentWindow.xaml
    /// </summary>
    public partial class CheckEquipmentWindow : Window
    {
        public CheckEquipmentWindow()
        {
            InitializeComponent();

            txtNum.Text = _numValue.ToString();
        }
        private int _numValue = 1;

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                if (value < 1) return;
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {

            NumValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }
            int.TryParse(txtNum.Text, out int myNum);
            if (myNum < 1)
            {
                txtNum.Text = _numValue.ToString();
            }

            if (!int.TryParse(txtNum.Text, out _numValue))
                txtNum.Text = _numValue.ToString();
        }
    }
}
