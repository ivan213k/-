using System.Collections.Generic;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core.Abstractions
{
    interface IOrderManager
    {
        void CreateOrder(Order order);

        void CloseOrder(int order);

        void CloseOrderPartially(Order order);

        void CancelOrder(int orderId);

        void UpdateOrder(int oldOrderId, Order newOrder);

        List<Order> GetActiveOrders();

    }
}
