using System.Windows;
using Управление_заказами.Models.DataBase;
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
            SearchOrderWindow window = new SearchOrderWindow();
            window.ShowDialog();
        }

        private void HistoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            OrdersHistoryWindow window = new OrdersHistoryWindow();
            window.Show();
        }

        private void AvalibleEquipmentButton_OnClick(object sender, RoutedEventArgs e)
        {
            EquipmentInStockWindow window = new EquipmentInStockWindow();
            if (AppSettings.AccountType != AccountType.Administrator)
            {
                //window.DataGrid.IsReadOnly = true;
            }
            window.Show();
        }

        private void RegisterUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            RegistrateUserWindow window = new RegistrateUserWindow();
            window.ShowDialog();
        }
    }
}
