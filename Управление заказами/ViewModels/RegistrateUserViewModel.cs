using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;
using Управление_заказами.Views;

namespace Управление_заказами.ViewModels
{
    class RegistrateUserViewModel : BaseViewModel
    {
        private readonly IUserManager UserManager;
        public RegistrateUserViewModel()
        {
             UserManager = new UserManager();
             GoogleCalendarColors = AppSettings.GoogleCalendarColors;
             AddUserCommand = new Command(AddUser);
             SeeAllUsersCommand = new Command(SeeAllUser);
        }

        private void SeeAllUser(object obj)
        {
            UsersWindow window = new UsersWindow();
            window.ShowDialog();
        }

        private async void AddUser(object obj)
        {
            PasswordBox passwordBox = obj as PasswordBox;
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Имя не введено");
                return;
            }
            if (passwordBox.Password.Length < 4)
            {
                MessageBox.Show("Пароль не может быть менее 4 символов");
                return;
            }

            try
            {
                EnableProgressBar();
                await UserManager.RegistrateUserAsync(new User()
                {
                    Name = Name,
                    Password = passwordBox.Password,
                    GoogleCalendarColorId = SelectedColor.Key,
                    AccountType = (AccountType) SelectedAccountType,
                });
                MessageBox.Show("Пользователь успешно зарегистрирован");
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Такой пользователь уже зарегистрирован");
            }
            finally
            {
                DisableProgressBar();
            }
        }

        public string Name { get; set; }

        public int SelectedAccountType { get; set; } = (int)AccountType.Manager;

        public Dictionary<string, string> GoogleCalendarColors { get; set; }

        public ICommand AddUserCommand { get; set; }

        public ICommand SeeAllUsersCommand { get; set; }

        public KeyValuePair<string, string> SelectedColor { get; set; }
    }
}
