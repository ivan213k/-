using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core
{
    class EquipmentInfo : IEquipmentInfo
    {
        private void UpdateEquipmensNameInRent(List<EquipmentInStock> newEquipments)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var oldEquipments = db.EquipmentsInStock.ToList();
                foreach (var newEquipment in newEquipments)
                {
                    string oldEquipmentName = oldEquipments.Select(eq => eq).
                        Where(eq => eq.Id == newEquipment.Id).Single().Name;
                    UpdateEquipmentName(oldEquipmentName, newEquipment.Name, db);
                }

                db.SaveChanges();
            }
        }

        private void UpdateEquipmentName(string oldName, string newName, AppDbContext db)
        {
            foreach (var equipmentInRent in db.EquipmentsInRent)
            {
                if (equipmentInRent.Name == oldName)
                {
                    equipmentInRent.Name = newName;
                }
            }

            foreach (var equipmentFromOrder in db.EquipmentsFromOrder)
            {
                if (equipmentFromOrder.Name == oldName)
                {
                    equipmentFromOrder.Name = newName;
                }
            }
        }

        public async Task<List<EquipmentInStock>> GetEquipmentsAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    return db.EquipmentsInStock.ToList();
                }
            });    
        }

        public async Task<int> GetAvalibleCountAsync(string equipmentName)
        {
            return await Task.Factory.StartNew(()=> 
            {
                using (AppDbContext db = new AppDbContext())
                {
                    return (from equipment in db.EquipmentsInStock
                            where equipment.Name == equipmentName
                            select equipment.Count).Single();
                }
            });
        }

        public async Task<int> GetAvalibleCountAsync(string equipmentName, DateTime startDate, DateTime endDate)
        {
            return await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    return (from equipment in db.EquipmentsInRent
                            where equipment.Name == equipmentName
                                  && (equipment.EndDate <= startDate || endDate <= equipment.StartDate)
                            select equipment.Count).Sum();
                }
            });  
        }

        public async Task UpdateEquipmentsRange(List<EquipmentInStock> equipments)
        {
            await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    UpdateEquipmensNameInRent(equipments);
                    db.EquipmentsInStock.UpdateRange(equipments);
                    db.SaveChanges();
                }
            });
               
        }

        public async Task AddEquipment(EquipmentInStock equipment)
        {
            await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    var equipmentInStock = (from _equipment in db.EquipmentsInStock
                        where _equipment.Name == equipment.Name
                        select _equipment).SingleOrDefault();
                    if (equipmentInStock != null)
                    {
                        throw new ArgumentException("Such equipment already exists");
                    }

                    db.EquipmentsInStock.Add(equipment);
                    db.SaveChanges();
                }
            });
        }

        public async Task DeleteEquipment(EquipmentInStock equipment)
        {
            await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    db.EquipmentsInStock.Remove(equipment);
                    db.SaveChanges();
                }
            });
        }

        public async Task<List<MissingEquipment>> GetMissingEquipments(List<EquipmentFromOrder> equipmentsForCheck, DateTime startDate, DateTime endDate)
        {
            var checkResult = new List<MissingEquipment>();

            foreach (var equipment in equipmentsForCheck)
            {
                if (equipment.IsPartnerEquipment) continue;
      
                int needCount = equipment.Count;
                int balance = await GetAvalibleCountAsync(equipment.Name);
                int avalibleInRange = await GetAvalibleCountAsync(equipment.Name, startDate, endDate);
                if ((needCount > (balance+avalibleInRange)))
                {
                    checkResult.Add(new MissingEquipment()
                    {
                        Name = equipment.Name,
                        NeedCount = equipment.Count,
                        Balance = balance,
                        AvalibleInSelectedDateRange = avalibleInRange,
                    });
                }
            }
            return checkResult;
        }
    }
}
