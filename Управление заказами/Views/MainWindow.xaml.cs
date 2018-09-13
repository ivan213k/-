using System.Windows;
using Управление_заказами.Views;

namespace Управление_заказами
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckEquipmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            CheckEquipmentWindow window = new CheckEquipmentWindow();
            window.Show();
        }

        private void CreateOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
           CreateOrderWindow window = new CreateOrderWindow();
           window.Show();
        }

        private void ReturOrdertButton_OnClick(object sender, RoutedEventArgs e)
        {
            ReturnOrdersWindow window = new ReturnOrdersWindow();
            window.Show();
        }

        private void EditOrderButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void HistoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void AvalibleEquipmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void RegisterUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
