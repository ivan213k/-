using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Управление_заказами.Models.DataBase
{
    class EquipmentFromOrder: Equipment
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int EquipmentFromOrderKey { get; set; }

        [ForeignKey("EquipmentFromOrderKey")]
        public Order Order { get; set; }

        public static implicit operator EquipmentInRent(EquipmentFromOrder equipment)
        {
            return new EquipmentInRent()
            {
                Name = equipment.Name,
                Count = equipment.Count,
                Category = equipment.Category,
                StartDate = equipment.StartDate,
                EndDate = equipment.EndDate
            };
        }

        public static implicit operator EquipmentInStock(EquipmentFromOrder equipment)
        {
            return new EquipmentInStock()
            {
                Name = equipment.Name,
                Count = equipment.Count,
                Category = equipment.Category,
            };
        }
    }
}
