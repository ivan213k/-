using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Управление_заказами.ViewModels;

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


        private async void EquipmentInStockWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if(SaveChanges.Visibility != Visibility.Visible) return;
            var diResult = MessageBox.Show("Хотите сохранить изменения?", "Сохранение изменений",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question, MessageBoxResult.Yes);
            if (diResult == MessageBoxResult.Yes)
            {
                e.Cancel = true;
                var viewModel = this.DataContext as EquipmentsInStockViewModel;
                await viewModel.SaveChangesOnClosingWindow(SaveChanges);
                this.Close();
            }
            else if(diResult == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                e.Cancel = true;
            }        
        }

        
    }
}
