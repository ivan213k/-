using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core.Abstractions
{
    interface IOrderManager
    {
        void CreateOrder(Order order);

        void CloseOrder(Order order);

        void CancelOrder(Order order);

        void UpdateOrder(Order order);
    }
}
