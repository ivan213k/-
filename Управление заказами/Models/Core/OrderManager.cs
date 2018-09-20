using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core
{
    class OrderManager : IOrderManager
    {   
        private readonly GoogleCalendar Calendar = new GoogleCalendar();

        public async Task CreateOrderAsync(Order order)
        {
            await Task.Factory.StartNew(async()=> 
            {
                using (AppDbContext db = new AppDbContext())
                {
                    foreach (var equipment in order.Equipments)
                    {
                        TakeEquipment(equipment, db);
                        AddEqipmentInRent(equipment, db);
                    }

                    order.EventId = await Calendar.AddEvent(order);
                    order.ReturnEventId = await Calendar.AddReturnEvent(order);
                    db.OrdersHistory.Add(order);
                    db.OrdersHistory.Include(o => o.Equipments);
                    db.SaveChanges();
                }
            });
        }

        public async Task CloseOrderAsync(int orderId)
        {
            await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    Order closingOrder = (from order in db.OrdersHistory.Include(e => e.Equipments)
                                          where order.Id == orderId
                                          select order).Single();
                    foreach (var equipment in closingOrder.Equipments)
                    {
                        RemoveEquipmentFromRent(equipment, db);
                    }

                    closingOrder.Status = OrderStatus.Closed;
                    db.SaveChanges();
                }
            });
            
        }

        public async Task CloseOrderPartiallyAsync(Order order)
        {
            await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    foreach (var equipment in order.Equipments)
                    {
                        RemoveEquipmentFromRent(equipment, db);
                        RemoveEquipmentFromOrder(equipment, db);
                    }
                    db.SaveChanges();
                }
            });  
        }

        private void RemoveEquipmentFromOrder(EquipmentFromOrder equipmentFromOrder, AppDbContext db)
        {
            var equipment = (from eq in db.EquipmentsFromOrder
                where eq.Id == equipmentFromOrder.Id
                select eq).Single();
            if (equipment.Count== equipmentFromOrder.Count)
            {
                db.EquipmentsFromOrder.Remove(equipment);
            }
            else
            {
                equipment.Count -= equipmentFromOrder.Count;
            }
        }

        public async Task CancelOrderAsync(int orderId)
        {
            await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    Order closingOrder = (from order in db.OrdersHistory.Include(e => e.Equipments)
                                          where order.Id == orderId
                                          select order).Single();
                    foreach (var equipment in closingOrder.Equipments)
                    {
                        RemoveEquipmentFromRent(equipment, db);
                    }

                    closingOrder.Status = OrderStatus.Canceled;
                    db.SaveChanges();
                }
            });
            
        }

        private void RemoveEquipmentFromRent(EquipmentFromOrder equipment, AppDbContext db)
        {
            EquipmentInRent equipmentInRent = (from eq in db.EquipmentsInRent
                where eq.Name == equipment.Name && eq.StartDate == equipment.StartDate &&
                      eq.EndDate == equipment.EndDate
                select eq).Single();

            if (equipmentInRent.Count > equipment.Count)
            {
                equipmentInRent.Count = equipmentInRent.Count - equipment.Count;
                AddEquipmentToAvalible(equipment.Name,equipment.Count,db);
            }
            else
            {
                db.EquipmentsInRent.Remove(equipmentInRent);
                AddEquipmentToAvalible(equipmentInRent.Name, equipmentInRent.Count, db);
            }

        }

        private void AddEquipmentToAvalible(string name, int count, AppDbContext db)
        {
            var equipmentInStock = (from equipment in db.EquipmentsInStock
                                                   where equipment.Name == name
                                    select equipment).Single();
            equipmentInStock.Count += count;
        }

        private void TakeEquipmentFromRent(string name, int needCount, DateTime startDate, DateTime endDate, AppDbContext db)
        {
            var equipments = (from equipment in db.EquipmentsInRent
                              where equipment.Name == name && (equipment.StartDate <= endDate || equipment.EndDate <= startDate)
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

        public async Task UpdateOrderAsync(int oldOrderId, Order newOrder)
        {
            await Task.Factory.StartNew(async()=> 
            {
                using (AppDbContext db = new AppDbContext())
                {
                    Order oldOrder = (from order in db.OrdersHistory.Include(e => e.Equipments)
                                      where order.Id == oldOrderId
                                      select order).Single();
                    db.OrdersHistory.Remove(oldOrder);
                    foreach (var equipment in oldOrder.Equipments)
                    {
                        RemoveEquipmentFromRent(equipment, db);
                    }

                    db.SaveChanges();
                    foreach (var equipment in newOrder.Equipments)
                    {
                        TakeEquipment(equipment, db);
                        AddEqipmentInRent(equipment, db);
                    }

                    newOrder.EventId = await Calendar.UpdateEvent(oldOrder,newOrder);
                    newOrder.ReturnEventId = await Calendar.UpdateReturnEvent(oldOrder,newOrder);
                    db.OrdersHistory.Add(newOrder);
                    db.OrdersHistory.Include(o => o.Equipments);
                    db.SaveChanges();
                }
            });    
        }

        public async Task<List<Order>> GetActiveOrdersAsync()
        {
            return await Task.Factory.StartNew(()=> 
            {
                using (AppDbContext db = new AppDbContext())
                {
                    return (from order in db.OrdersHistory.Include(e => e.Equipments)
                            where order.Status == OrderStatus.Open
                            select order).ToList();
                }
            }); 
        }

        public async Task SetNoteAsync(int orderId, string note)
        {
            await Task.Factory.StartNew(() =>
            {
                using (AppDbContext db = new AppDbContext())
                {
                    var order = (from _order in db.OrdersHistory
                        where _order.Id == orderId
                        select _order).Single();
                    order.Note = note;
                    db.SaveChanges();
                }
            });
        }
    }
}
