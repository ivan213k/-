using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace Управление_заказами.Models.DataBase
{
    class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public string CustomerName { get; set; }

        public string Manager { get; set; }

        public string MobilePhone { get; set; }

        public string Adress { get; set; }

        public OrderStatus Status { get; set; }

        public string Note { get; set; }

        public string EventId { get; set; }

        public string ReturnEventId { get; set; }

        public List<EquipmentFromOrder> Equipments { get; set; }

        public override string ToString()
        {
            return $"Статус: {Status} Имя: {CustomerName} Адресс: {Adress} Мобильный телефон: {MobilePhone} Время создания заказа {CreateDate} Время возврата: {ReturnDate} {Manager}";
        }
    }
}
