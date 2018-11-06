using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

       
    }
}
