using System;
using System.Collections.Generic;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core.Abstractions
{
    interface IEquipmentInfo
    {
        List<Equipment> GetEquipments();

        int GetAvalibleCount(int equiomentId);

        int GetAvalibleCount(int equipmentId, DateTime startDate, DateTime endDate);
    }
}
