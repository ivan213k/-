using System.Windows;
using System.Windows.Controls;

namespace Управление_заказами.Views
{
    /// <summary>
    /// Interaction logic for EquipmentInStockWindow.xaml
    /// </summary>
    public partial class EquipmentInStockWindow : Window
    {
        public EquipmentInStockWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            SaveChanges.Visibility = Visibility.Visible;
        }
    }
}
