using System.ComponentModel.DataAnnotations;

namespace Управление_заказами.Models.DataBase
{
     class Equipment
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

    }
}
