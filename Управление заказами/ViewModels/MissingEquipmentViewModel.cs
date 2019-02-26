using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Управление_заказами.Models;

namespace Управление_заказами.ViewModels
{
    class MissingEquipmentViewModel : BaseViewModel
    {
        public MissingEquipmentViewModel()
        {
            CreateOrderCommand = new Command(CreateOrder);
        }

        private void CreateOrder(object obj)
        {
            var window = obj as Window;
            if (CanCreateOrder())
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private List<MissingEquipment> _equioments;

        public List<MissingEquipment> Equioments { get => _equioments;
            set
            {
                _equioments = value; OnePropertyChanged();

            }
        }

        public ICommand CreateOrderCommand { get; set; }

        bool CanCreateOrder()
        {
            foreach (var missingEquipment in Equioments)
            {
                if (!missingEquipment.IsPartnerHave || string.IsNullOrWhiteSpace(missingEquipment.PartnerName))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
