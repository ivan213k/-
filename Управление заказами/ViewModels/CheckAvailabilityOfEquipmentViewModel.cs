using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Управление_заказами.Models;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.ViewModels
{
    class CheckAvailabilityOfEquipmentViewModel : BaseViewModel
    {
        private IEquipmentInfo EquipmentInfo = new EquipmentInfo();

        public CheckAvailabilityOfEquipmentViewModel()
        {
            LoadEquipments();
        }

        List<EquipmentInStock> equipments;
        public List<EquipmentInStock> Equipments
        {
            get => equipments;
            set
            {
                equipments = value;
                OnePropertyChanged();
                Categoryes = (from equipment in Equipments
                              select equipment.Category).Distinct().ToList();
            }
        }

        List<string> categoryes;
        public List<string> Categoryes
        {
            get => categoryes;
            set
            {
                categoryes = value;
                OnePropertyChanged();
            }
        }

        string selectedCategory;
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                SelectedEquipments = (from equipments in Equipments
                                      where equipments.Category == value
                                      select equipments.Name).ToList();
                OnePropertyChanged();
            }
        }

        List<string> selectedEquipments;
        public List<string> SelectedEquipments
        {
            get => selectedEquipments;
            set
            {
                selectedEquipments = value;
                OnePropertyChanged();
            }
        }

        string selectedEquipment;
        public string SelectedEquipment
        {
            get => selectedEquipment;
            set
            {
                selectedEquipment = value;
                OnePropertyChanged();
                CheckAvailability();
            }
        }

        DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                CheckAvailability();
            }
        }

        DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                CheckAvailability();
            }
        }

        int count = 1;
        public int Count
        {
            get => count;
            set
            {
                count = value;
                OnePropertyChanged();
                CheckAvailability();
            }
        }

        List<AvailabilityEquipment> checkResult;
        public List<AvailabilityEquipment> CheckResult
        {
            get => checkResult;
            set
            {
                checkResult = value;
                OnePropertyChanged();
            }
        }

        async void LoadEquipments()
        {
            EnableProgressBar();
            Equipments = await EquipmentInfo.GetEquipmentsAsync();
            DisableProgressBar();
        }

        async void CheckAvailability()
        {
            if(selectedEquipment == null) return;
            var checkResult = new List<AvailabilityEquipment>();

            EnableProgressBar();

            int balance = await EquipmentInfo.GetAvalibleCountAsync(selectedEquipment);
            int avalibleInRange = await EquipmentInfo.GetAvalibleCountAsync(selectedEquipment, StartDate.AddSeconds(-StartDate.Second), EndDate.AddSeconds(-EndDate.Second));
            bool isEnough = balance + avalibleInRange >= Count;
            checkResult.Add(new AvailabilityEquipment()
            {
                Name = SelectedEquipment,
                NeedCount = Count,
                Balance = balance,
                AvalibleInSelectedDateRange = avalibleInRange,
                IsEnough = isEnough,
                Result = isEnough ? "ОK" : "Не хватает",
                TotalCount = (from equipmentInStock in Equipments
                              where equipmentInStock.Name == selectedEquipment
                              select equipmentInStock.TotalCount).Single(),
            });

            DisableProgressBar();
            CheckResult = checkResult;
        }

    }
}
