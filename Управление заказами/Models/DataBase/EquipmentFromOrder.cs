using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Управление_заказами.Models.DataBase
{
    class EquipmentFromOrder: Equipment
    {
        public int EquipmentFromOrderKey { get; set; }

        [ForeignKey("EquipmentFromOrderKey")]
        public Order Order { get; set; }
    }
}
