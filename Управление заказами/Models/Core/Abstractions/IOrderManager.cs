using System.Collections.Generic;
using System.Threading.Tasks;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core.Abstractions
{
    interface IOrderManager
    {
        Task CreateOrderAsync(Order order);

        Task CloseOrderAsync(int order);

        Task CloseOrderPartiallyAsync(Order order);

        Task CancelOrderAsync(int orderId);

        Task UpdateOrderAsync(int oldOrderId, Order newOrder);

        Task<List<Order>> GetActiveOrdersAsync();

    }
}
