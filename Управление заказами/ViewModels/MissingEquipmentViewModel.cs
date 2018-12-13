using System.Collections.Generic;
using Управление_заказами.Models;

namespace Управление_заказами.ViewModels
{
    class MissingEquipmentViewModel : BaseViewModel
    {
        private List<MissingEquipment> _equioments;

        public List<MissingEquipment> Equioments { get => _equioments;
            set
            {
                _equioments = value; OnePropertyChanged();

            }
        }
    }
}
