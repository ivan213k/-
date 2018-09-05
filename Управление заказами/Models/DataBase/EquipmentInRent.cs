using System;

namespace Управление_заказами.Models.DataBase
{
    class EquipmentInRent: Equipment
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
