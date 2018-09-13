using System.Collections.Generic;
using System.Windows.Input;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;
using Управление_заказами.Views;

namespace Управление_заказами.ViewModels
{
    class ReturnOrderViewModel : BaseViewModel
    {
        private bool _isDeterminate;
        private bool _isEnabled;
        private List<Order> _returnOrders;
        private IOrderManager _orderManager;

        public List<Order> ReturnOrders
        {
            get => _returnOrders;
            set
            {
                _returnOrders = value; 
                OnePropertyChanged();
            }
        }

        public Order SelectedOrder { get; set; }

        public ReturnOrderViewModel()
        {
            CloseOrderCommand = new Command(CloseOrder);
            CancelOrderCommand = new Command(CancelOrder);
            _orderManager = new OrderManager();

            LoadEquipments();
        }

        private async void LoadEquipments()
        {
            EnableProgressBar();
            ReturnOrders =  await _orderManager.GetActiveOrdersAsync();
            DisableProgressBar();
        }

        async void CancelOrder(object parametr)
        {
            var orderId = SelectedOrder.Id;
            var window = new ConfirmationWindow();
            window.ConfirmText.Text = "Вы действительно хотите отменить заказ ?";
            if (window.ShowDialog() != true)
            {
                return;
            }
            EnableProgressBar();
            await _orderManager.CancelOrderAsync(orderId);
            DisableProgressBar();
            LoadEquipments();
        }

        void CloseOrder(object parametr)
        {
            CloseOrderWindow window = new CloseOrderWindow()
            {
                DataContext = new CloseOrderViewModel()
                {
                    Order = SelectedOrder
                }
            };
            
            window.ShowDialog();
        }

        public ICommand CloseOrderCommand { get; set; }

        public ICommand CancelOrderCommand { get; set; }

        private void DisableProgressBar()
        {
            IsEnabled = true;
            IsDeterminate = false;
        }

        public bool IsDeterminate
        {
            get => _isDeterminate;
            set
            {
                _isDeterminate = value;
                OnePropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnePropertyChanged();
            }
        }

        private void EnableProgressBar()
        {
            IsEnabled = false;
            IsDeterminate = true;
        }
    }
}
