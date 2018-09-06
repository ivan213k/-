using System;
using System.Collections.Generic;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core.Abstractions
{
    interface IEquipmentInfo
    {
        List<EquipmentInStock> GetEquipments();

        int GetAvalibleCount(string equiomentName);

        int GetAvalibleCount(string equipmentName, DateTime startDate, DateTime endDate);
    }
}
