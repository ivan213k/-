using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core
{
    class UserManager : IUserManager
    {
        public async Task<User> AuthorizeAsync(string name, string password)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    return (from user in db.Users
                        where user.Name == name && user.Password == password
                        select user).FirstOrDefault();
                }
            });
        }

        public async Task RegistrateUserAsync(User user)
        {
            await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    var tempUser = (from _user in db.Users
                        where _user.Name == user.Name && _user.Password == user.Password
                        select _user).FirstOrDefault();
                    if (tempUser == null)
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new ArgumentException("A user with such data already exists", nameof(user));
                    }
                }
            });
        }

        public async Task DeleteUserAsync(int userId)
        {
            await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    var user = (from _user in db.Users
                        where _user.Id == userId
                        select _user).Single();
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            });
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    return db.Users.ToList();
                }
            });
        }
    }
}
