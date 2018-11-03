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
            SetNoteCommand = new Command(SetNote);
            _orderManager = new OrderManager();

            LoadEquipments();
        }

        private async void SetNote(object obj)
        {
            EnableProgressBar();
            await _orderManager.SetNoteAsync(SelectedOrder.Id, SelectedOrder.Note);
            DisableProgressBar();
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
            LoadEquipments();
        }

        public ICommand CloseOrderCommand { get; set; }

        public ICommand CancelOrderCommand { get; set; }

        public ICommand SetNoteCommand { get; set; }

    }
}
