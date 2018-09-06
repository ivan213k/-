using System;
using System.Collections.Generic;
using System.Linq;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core
{
    class EquipmentInfo : IEquipmentInfo
    {
        public List<EquipmentInStock> GetEquipments()
        {
            using (AppDbContext db = new AppDbContext())
            {
                return db.EquipmentsInStock.ToList();
            }
        }

        public int GetAvalibleCount(string equiomentName)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return (from equipment in db.EquipmentsInStock
                    where equipment.Name == equiomentName
                        select equipment.Count).Single();
            }
        }

        public int GetAvalibleCount(string equipmentName, DateTime startDate, DateTime endDate)
        {
            using (AppDbContext db = new AppDbContext())
            {
                return (from equipment in db.EquipmentsInRent
                    where equipment.Name == equipmentName
                          && (equipment.EndDate <= startDate || endDate <= equipment.StartDate)
                    select equipment.Count).Sum();
            }
            
        }
    }
}
