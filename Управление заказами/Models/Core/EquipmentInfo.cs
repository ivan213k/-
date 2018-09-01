using System;
using System.Collections.Generic;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core
{
    class EquipmentInfo : IEquipmentInfo
    {
        public List<Equipment> GetEquipments()
        {
            throw new NotImplementedException();
        }

        public int GetAvalibleCount(int equiomentId)
        {
            throw new NotImplementedException();
        }

        public int GetAvalibleCount(int equipmentId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
