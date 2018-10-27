﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core
{
    class EquipmentInfo : IEquipmentInfo
    {
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

        public async Task<int> GetAvalibleCountAsync(string equiomentName)
        {
            return await Task.Factory.StartNew(()=> 
            {
                using (AppDbContext db = new AppDbContext())
                {
                    return (from equipment in db.EquipmentsInStock
                            where equipment.Name == equiomentName
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
    }
}
