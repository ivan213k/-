using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core
{
    class OrderManager : IOrderManager
    {
        private void TakeEquipmentFromRent(string name, int needCount, DateTime startDate, DateTime endDate, AppDbContext db)
        {
            var equipments = (from equipment in db.EquipmentsInRent
                where equipment.Name == name && (equipment.StartDate<=endDate || equipment.EndDate <= startDate)
                select equipment).ToList();
            foreach (var equipment in equipments)
            {
                if (equipment.Count < needCount)
                {
                    needCount -= equipment.Count;
                    equipment.Count = 0;
                }
                else
                {
                    equipment.Count -= needCount;
                    return;
                }
              
            }
            throw new ArgumentException("Не хватает оборудования");
        }

        private void TakeEquipment(EquipmentFromOrder equipment, AppDbContext db)
        {
            var avalibleEquipment = (from equipmentInStock in db.EquipmentsInStock
                where equipmentInStock.Name == equipment.Name
                select equipmentInStock).Single();
            if (avalibleEquipment.Count >= equipment.Count)
            {
                avalibleEquipment.Count = avalibleEquipment.Count - equipment.Count;
            }
            else
            {
                int notEnoughCount = equipment.Count - avalibleEquipment.Count;
                avalibleEquipment.Count = 0;
                TakeEquipmentFromRent(equipment.Name, notEnoughCount, equipment.StartDate, equipment.EndDate, db);
            }
        }

        private void AddEqipmentInRent(EquipmentFromOrder equipment, AppDbContext db)
        {
            var equipmentInRent = (from eq in db.EquipmentsInRent
                where eq.Name == equipment.Name &&
                      eq.EndDate == equipment.EndDate
                select eq).FirstOrDefault();
            if (equipmentInRent != null)
            {
                equipmentInRent.Count += equipment.Count;
            }
            else
            {
                db.EquipmentsInRent.Add(equipment);
            }
        }

        public void CreateOrder(Order order)
        {
            using (AppDbContext db = new AppDbContext())
            {
                foreach (var equipment in order.Equipments)
                {
                    TakeEquipment(equipment,db);
                    AddEqipmentInRent(equipment,db);
                }

                db.OrdersHistory.Add(order);
                db.OrdersHistory.Include(o=> o.Equipments);
                db.SaveChanges();
            }

        }

        public void CloseOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void CancelOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
