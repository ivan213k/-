using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.ViewModels
{
    class CloseOrderViewModel : BaseViewModel
    {
        private readonly IOrderManager orderManager = new OrderManager();
        private List<CloseEquipmentModel> _equipments;
        private Order _order;

        public CloseOrderViewModel()
        {
            CloseOrderCommand = new Command(CloseOrder);
        }

        public string SelectedCloseMode { get; set; }

        public Order Order
        {
            get => _order;
            set
            {
                _order = value;
                Equipments = new List<CloseEquipmentModel>();
                foreach (var equipment in value.Equipments)
                {
                    Equipments.Add(equipment);
                }
            }
        }

        public List<CloseEquipmentModel> Equipments
        {
            get => _equipments;
            set
            {
                _equipments = value;
                OnePropertyChanged();
            }
        }

        public class CloseEquipmentModel : Equipment
        {
            private int _count;
            public bool IsClose { get; set; }

            public new int Count
            {
                get => _count;
                set
                {
                    if (value > 0 && value <= MaxCount)
                    {
                        _count = value;
                    }
                    else
                    {
                        _count = MaxCount;
                    }
                }
            }

            public int MaxCount { get; set; }

            public static implicit operator CloseEquipmentModel(EquipmentFromOrder equipment)
            {
                return new CloseEquipmentModel()
                {
                    MaxCount = equipment.Count,
                    Count = equipment.Count,
                    Name = equipment.Name,
                    Category = equipment.Category,
                };
            }
        }

        async void CloseOrder(object obj)
        {
            if (SelectedCloseMode.Contains("Весь заказ"))
            {
                EnableProgressBar();
                await orderManager.CloseOrderAsync(Order.Id);
                DisableProgressBar();
                (obj as Window).Close();
                MessageBox.Show("Заказ успешно закрыт");
            }
            else
            {
                EnableProgressBar();
                foreach (var closeEquipment in Equipments)
                {
                    var equipment = (from eq in Order.Equipments
                        where eq.Name == closeEquipment.Name
                        select eq).Single();
                    if (closeEquipment.IsClose)
                    {
                        equipment.Count = closeEquipment.Count;
                    }
                    else
                    {
                        Order.Equipments.Remove(equipment);
                    }
                }
                await orderManager.CloseOrderPartiallyAsync(Order);
                DisableProgressBar();
                (obj as Window).Close();
                MessageBox.Show("Часть заказа успешно закрыто");
            }
        }

        public ICommand CloseOrderCommand { get; set; }

        
    }
}
