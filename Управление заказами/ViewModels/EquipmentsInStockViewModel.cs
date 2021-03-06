﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using Управление_заказами.Models;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;
using Управление_заказами.Views;
using WindowState = System.Windows.WindowState;

namespace Управление_заказами.ViewModels
{
    class EquipmentsInStockViewModel : BaseViewModel
    {
        public class HierarchicalEquipment
        {
            public string Category { get; set; }

            public ObservableCollection<EquipmentInStock> Equipments { get; set; }
        }

        private readonly IEquipmentInfo EquipmentInfo;

        public EquipmentsInStockViewModel()
        {
            EquipmentInfo = new EquipmentInfo();
            SaveChangesCommand = new Command(SaveChanges);
            AddEquipmentCommand = new Command(AddEquipment);
            DeleteEquipmentCommand = new Command(DeleteEquipment);
            DeleteCategoryCommand = new Command(DeleteCategory);
            Refresh();
        }


        private async void DeleteCategory(object obj)
        {
            if (SelectedCategory!=null)
            {
                ConfirmationWindow window = new ConfirmationWindow();
                window.ConfirmText.Text = $"Вы действительно хотите удалить категорию \"{SelectedCategory.Category}\" и все ее оборудование ? ";

                if (window.ShowDialog() == true)
                {
                    EnableProgressBar();
                    foreach (var equipment in SelectedCategory.Equipments)
                    {
                        await EquipmentInfo.DeleteEquipment(equipment);
                    }
                    DisableProgressBar();
                    Refresh();
                }
            }
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
                    var selectedCategory =
                        Equipments.Select(e => e).Where(e => e.Category == SelectedEquipment.Category).SingleOrDefault();
                    if (selectedCategory!=null)
                    selectedCategory.Equipments.Remove(SelectedEquipment);
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
                                       select equipmentInStock).ToList().ToObservableCollection(),
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

        public HierarchicalEquipment SelectedCategory { get; set; }

        public ICommand SaveChangesCommand { get; set; }

        public ICommand AddEquipmentCommand { get; set; }

        public ICommand DeleteEquipmentCommand { get; set; }

        public ICommand DeleteCategoryCommand { get; set; }

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

        public async Task SaveChangesOnClosingWindow(object parametr)
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
  
        private ObservableCollection<HierarchicalEquipment> _equipments;

        #endregion
    }
}
