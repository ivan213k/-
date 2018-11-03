using System.Collections.Generic;
using System.Windows.Input;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;
using Управление_заказами.Views;

namespace Управление_заказами.ViewModels
{
    class UsersViewModel : BaseViewModel
    {
        private readonly IUserManager UserManager;

        private List<User> _users;

        public UsersViewModel()
        {
            UserManager = new UserManager();
            RemoveUserCommand = new Command(RemoveUser);
            LoadUsers();
        }

        private async void LoadUsers()
        {
            EnableProgressBar();
            Users = await UserManager.GetAllUsers();
            DisableProgressBar();
        }


        private async void RemoveUser(object obj)
        {
            ConfirmationWindow window = new ConfirmationWindow();
            window.ConfirmText.Text = $"Вы действительно хотите удалить пользователя '{SelectedUser.Name}' ?";
            if (window.ShowDialog() == false)
            {
                return;
            }
            EnableProgressBar();
            await UserManager.DeleteUserAsync(SelectedUser.Id);
            LoadUsers();
            DisableProgressBar();
        }

        public ICommand RemoveUserCommand { get; set; }

        public List<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnePropertyChanged();
            }
        }

        public User SelectedUser { get; set; }

    }
}
