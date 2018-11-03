using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public class HierarchicalEquipment
        {
            public string Category { get; set; }

            public List<EquipmentInStock> Equipments { get; set; }
        }

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
            if (SelectedEquipment != null)
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
            Equipments = new ObservableCollection<HierarchicalEquipment>();

            var equipments  = await EquipmentInfo.GetEquipmentsAsync();
            var categories = (from equipment in equipments
                          select equipment.Category).Distinct().ToList();
            foreach (var category in categories)
            {
                Equipments.Add(new HierarchicalEquipment()
                {
                    Category = category,
                    Equipments = (from equipmentInStock in equipments
                                 where equipmentInStock.Category == category
                                       select equipmentInStock).ToList(),
                });
            }
            DisableProgressBar();
        }

        public ObservableCollection<HierarchicalEquipment> Equipments
        {
            get => _equipments;
            set
            {
                _equipments = value; 
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
            List<EquipmentInStock> equipmentsInStocks = new List<EquipmentInStock>();
            foreach (var hierarchicalEquipment in Equipments)
            {
                equipmentsInStocks.AddRange(hierarchicalEquipment.Equipments);
            }
            await EquipmentInfo.UpdateEquipmentsRange(equipmentsInStocks);
            DisableProgressBar();
            button.Visibility = System.Windows.Visibility.Collapsed;
        }

        #region HelpMembers       
        private List<string> _categories;
        private string _selectedCategory;
        private ObservableCollection<HierarchicalEquipment> _equipments;

        #endregion
    }
}
