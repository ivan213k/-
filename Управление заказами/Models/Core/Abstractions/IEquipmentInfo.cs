using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core.Abstractions
{
    interface IEquipmentInfo
    {
        Task<List<EquipmentInStock>> GetEquipmentsAsync();

        Task<int> GetAvalibleCountAsync(string equiomentName);

        Task<int> GetAvalibleCountAsync(string equipmentName, DateTime startDate, DateTime endDate);
    }
}
