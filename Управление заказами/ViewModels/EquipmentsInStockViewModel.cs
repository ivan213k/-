using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.ViewModels
{
    class EquipmentsInStockViewModel : BaseViewModel
    {
        private readonly IEquipmentInfo EquipmentInfo;

        public EquipmentsInStockViewModel()
        {
            EquipmentInfo = new EquipmentInfo();
            SaveChangesCommand = new Command(SaveChanges);
            Refresh();
        }

        async void Refresh()
        {
            EnableProgressBar();
            Equipments = await EquipmentInfo.GetEquipmentsAsync();
            DisableProgressBar();
        }

        List<EquipmentInStock> equipments;
        public List<EquipmentInStock> Equipments
        {
            get => equipments;
            set
            {
                equipments = value;
                OnePropertyChanged();
            }
        }

        public ICommand SaveChangesCommand { get; set; }

        async void SaveChanges(object parametr)
        {
            Button button = parametr as Button;
            EnableProgressBar();
            await EquipmentInfo.UpdateEquipmentsRange(Equipments);
            DisableProgressBar();
            button.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void DisableProgressBar()
        {
            IsEnabled = true;
            IsDeterminate = false;
        }

        private void EnableProgressBar()
        {
            IsEnabled = false;
            IsDeterminate = true;
        }

        bool isEnabled;

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnePropertyChanged();
            }
        }

        bool isDeterminate;

        public bool IsDeterminate
        {
            get => isDeterminate;
            set
            {
                isDeterminate = value;
                OnePropertyChanged();
            }
        }
    }
}
