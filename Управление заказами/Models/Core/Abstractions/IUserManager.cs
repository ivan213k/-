using System.Collections.Generic;
using System.Threading.Tasks;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core.Abstractions
{
    interface IUserManager
    {
        Task<User> AuthorizeAsync(string name, string password);

        Task RegistrateUserAsync(User user);

        Task DeleteUserAsync(int userId);

        Task<List<User>> GetAllUsers();
    }
}
