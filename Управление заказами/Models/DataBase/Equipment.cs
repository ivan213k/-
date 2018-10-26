using System.ComponentModel.DataAnnotations;

namespace Управление_заказами.Models.DataBase
{
    class Equipment
    {
        private int _count;

        [Key]
        public int Id { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public int Count
        {
            get => _count;
            set
            {
                if (value >= 0)
                {
                    _count = value;
                }
            }
        }

        //public decimal ReplacmentCost { get; set; }

    }
}
