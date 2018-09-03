using System.Windows;
using Управление_заказами.Models.DataBase;

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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            using (AppDbContext db = new AppDbContext())
            {
                
            }
        }
    }
}
