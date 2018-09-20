using System.ComponentModel.DataAnnotations;

namespace Управление_заказами.Models.DataBase
{
    class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string GoogleCalendarColorId { get; set; }

        public AccountType AccountType { get; set; }

        public string Password { get; set; }
    }
}
