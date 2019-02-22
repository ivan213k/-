using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;
using Управление_заказами.Views;

namespace Управление_заказами.ViewModels
{
    class SearchOrderViewModel : BaseViewModel
    {
        private List<Order> _orders;
        private readonly IOrderManager OrderManager;

        public SearchOrderViewModel()
        {
            OrderManager = new OrderManager();
            EditOrderCommand = new Command(EditOrder);
            FilterByDateCommand = new Command(FiltrByDate);
            CancelOrderCommand = new Command(CancelOrder);
            Refresh();
        }

        public Order SelectedOrder { get; set; }

        public List<Order> Orders
        {
            get => _orders;
            set { _orders = value; OnePropertyChanged();}
        }

        private async void Refresh()
        {
            EnableProgressBar();
            Orders = await OrderManager.GetActiveOrdersAsync();
            View = CollectionViewSource.GetDefaultView(Orders);
            DisableProgressBar();
        }

        private async void CancelOrder(object obj)
        {
            if(SelectedOrder==null) return;
            int selectedOrderId = SelectedOrder.Id;
            var window = new ConfirmationWindow();
            window.ConfirmText.Text = "Вы действительно хотите отменить заказ ?";
            if (window.ShowDialog() != true)
            {
                return;
            }
            EnableProgressBar();
            await OrderManager.CancelOrderAsync(selectedOrderId);
            Refresh();
            DisableProgressBar();
        }

        private void FiltrByDate(object obj)
        {
            View.Filter = IsDateAccepted;
        }

        private bool IsDateAccepted(object obj)
        {
            Order order = obj as Order;
            DateTime createDate = new DateTime(order.CreateDate.Year, order.CreateDate.Month,order.CreateDate.Day);
            DateTime returnDate = new DateTime(order.ReturnDate.Year,order.ReturnDate.Month,order.ReturnDate.Day);
            return (createDate >= StartDate && EndDate >= returnDate);

        }

        private void EditOrder(object obj)
        {
            ObservableCollection<EquipmentFromOrder> eqsForOrder = new ObservableCollection<EquipmentFromOrder>();
            foreach (var equipment in SelectedOrder.Equipments)
            {
                eqsForOrder.Add(equipment);
            }
            EditOrderWindow window = new EditOrderWindow()
            {
                DataContext = new EditOrderViewModel(SelectedOrder)
                {
                    SelectedEquipmentsForOrder = eqsForOrder,
                    Adress = SelectedOrder.Adress,
                    StartDate = SelectedOrder.CreateDate,
                    EndDate = SelectedOrder.ReturnDate,
                    MobilePhone = SelectedOrder.MobilePhone,
                    Note = SelectedOrder.Note,
                    CustomerName = SelectedOrder.CustomerName,
                    SelectedDeliveryIndex = SelectedOrder.Adress == "Самовывоз" ? 0 : 1,
                }
            };
            window.ShowDialog();
            (obj as Window).Close();
        }

        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(5);

        public ICommand EditOrderCommand { get; set; }

        public ICommand FilterByDateCommand { get; set; }

        public ICommand CancelOrderCommand { get; set; }

        private ICollectionView View;
        private string _searchPattern;

        public string SearchPattern
        {
            get => _searchPattern;
            set
            {
                _searchPattern = value;
                if (View != null)
                {
                    View.Filter = x => x.ToString().ToLower().Contains(value.ToLower());
                }
            }
        }

    }
}
