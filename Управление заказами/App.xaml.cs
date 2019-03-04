using System.Windows;
using System.Windows.Threading;

namespace Управление_заказами
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message + e.Exception.StackTrace, "", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
