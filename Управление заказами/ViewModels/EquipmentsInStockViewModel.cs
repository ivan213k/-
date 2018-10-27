using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;
using Управление_заказами.Views;

namespace Управление_заказами.ViewModels
{
    class EquipmentsInStockViewModel : BaseViewModel
    {
        private readonly IEquipmentInfo EquipmentInfo;

        public EquipmentsInStockViewModel()
        {
            EquipmentInfo = new EquipmentInfo();
            SaveChangesCommand = new Command(SaveChanges);
            AddEquipmentCommand = new Command(AddEquipment);
            DeleteEquipmentCommand = new Command(DeleteEquipment);
           Refresh();
        }

        private async void DeleteEquipment(object obj)
        {
            if (SelectedEquipment!=null)
            {
                ConfirmationWindow window = new ConfirmationWindow();
                window.ConfirmText.Text = $"Вы действительно хотите удалить \"{SelectedEquipment.Name}\" {SelectedEquipment.Count} шт. ?";
                
                if (window.ShowDialog() == true)
                {
                    EnableProgressBar();
                    await EquipmentInfo.DeleteEquipment(SelectedEquipment);
                    DisableProgressBar();
                    Refresh();
                }
            }
        }

        private void AddEquipment(object obj)
        {
            AddEquipmentWindow window = new AddEquipmentWindow();
            window.ShowDialog();
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

        public EquipmentInStock SelectedEquipment { get; set; }

        public ICommand SaveChangesCommand { get; set; }

        public ICommand AddEquipmentCommand { get; set; }

        public ICommand DeleteEquipmentCommand { get; set; }

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
