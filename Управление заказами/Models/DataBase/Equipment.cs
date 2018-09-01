using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Управление_заказами.Models.DataBase
{
    class Equipment
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public int TotalCount { get; set; }

        public string Note { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ReturnDate { get; set; }

        [ForeignKey("EquipmentKey")]
        public Order Order { get; set; }
    }
}
