using System.Windows;

namespace Управление_заказами.Views
{
    /// <summary>
    /// Interaction logic for CloseOrderWindow.xaml
    /// </summary>
    public partial class CloseOrderWindow : Window
    {
        public CloseOrderWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
