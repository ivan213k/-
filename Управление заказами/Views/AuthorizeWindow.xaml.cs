using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Views
{
    /// <summary>
    /// Interaction logic for AuthorizeWindow.xaml
    /// </summary>
    public partial class AuthorizeWindow : Window
    {
        private readonly IUserManager UserManager = new UserManager();
        public AuthorizeWindow()
        {
            InitializeComponent();
        }

        public void EnableProgressBar()
        {
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.IsIndeterminate = true;
        }

        public void DisableProgressBar()
        {
            ProgressBar.Visibility = Visibility.Collapsed;
            ProgressBar.IsIndeterminate = false;
        }

        private async Task Authorize()
        {
            var user = await UserManager.AuthorizeAsync(Name.Text, Password.Password);
            if (user!=null)
            {
                try
                {
                    await GoogleCalendarAuthorize();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Не удалось авторизоваться в Google Calendar");
                    return;
                }
               
                AppSettings.CurrentUserName = user.Name;
                AppSettings.GoogleCalendarColorId = user.GoogleCalendarColorId;
                AppSettings.AccountType = user.AccountType;
                MainWindow window = new MainWindow();
                switch (user.AccountType)
                {
                    case AccountType.Administrator:
                        this.Close();
                        window.ShowDialog();
                        break;
                    case AccountType.Manager:
                        this.Close();
                        window.RegisterUserButton.Visibility = Visibility.Collapsed;
                        window.ShowDialog();
                        break;
                    case AccountType.User:
                        this.Close();
                        window.CheckEquipmentButton.Visibility = Visibility.Collapsed;
                        window.CreateOrderButton.Visibility = Visibility.Collapsed;
                        window.RegisterUserButton.Visibility = Visibility.Collapsed;
                        window.EditOrderButton.Visibility = Visibility.Collapsed;
                        window.ShowDialog();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Логин или пароль введен неверно");
            }
        }

        private async void Password_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EnableProgressBar();
                await Authorize();
                DisableProgressBar();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            EnableProgressBar();
            await Authorize();
            DisableProgressBar();
        }

        private async Task GoogleCalendarAuthorize()
        {
            GoogleCalendar calendar = new GoogleCalendar();
            await calendar.ReAuthorizeAsync();
        } 
    }
}
