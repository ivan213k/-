﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;
using Управление_заказами.Views;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace Управление_заказами.ViewModels
{
    class CheckEquipmentViewModel : BaseViewModel
    {
        public class AvailabilityEquipment
        {
            public string Name { get; set; }
            public int NeedCount { get; set; }
            public int Balance { get; set; }
            public int AvalibleInSelectedDateRange { get; set; }
            public bool IsEnough { get; set; }
            public string Result { get; set; }
            public int TotalCount { get; set; }
            public string ImageUrl { get; set; }
        }

        private IEquipmentInfo EquipmentInfo;

        public CheckEquipmentViewModel()
        {
            _checkResult = new List<AvailabilityEquipment>();
            AddEquipmentCommand = new Command(AddEquipmentToCheck, CanAddEquipment);
            RemoveEquipmentCommand = new Command(RemoveEquipment);
            CheckCommand = new Command(Check);
            GoToCreateOrderCommand = new Command(GoToCreateOrder, CanGoToCreateOrder);
            EquipmentInfo = new EquipmentInfo();
            LoadEquipments();
            SelectedEquipmentsForCheck.CollectionChanged += SelectedEquipmentsForCheck_CollectionChanged;
        }

        private void SelectedEquipmentsForCheck_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Check(null);
        }

        async void LoadEquipments()
        {
            EnableProgressBar();
            Equipments = await EquipmentInfo.GetEquipmentsAsync();
            DisableProgressBar();
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

        public List<EquipmentInStock> Equipments
        {
            get => _equipments;
            set
            {
                _equipments = value;
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

        public string SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                _selectedEquipment = value;
                SelectedImage = (from eq in Equipments
                                 where eq.Name == value
                                 select eq.ImageUrl).FirstOrDefault();
                AddEquipmentCommand.OneExecuteChaneged();
                OnePropertyChanged();
            }
        }

        public string SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
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

        private int count = 1;
        public int Count
        {
            get => count;
            set
            {
                count = value;
                OnePropertyChanged();
                if (SelectedEquipmentForCheck!=null && SelectedEquipment == SelectedEquipmentForCheck.Name && SelectedEquipmentForCheck.Count!= value)
                {
                    SelectedEquipmentForCheck.Count = value;
                    ICollectionView view = CollectionViewSource.GetDefaultView(SelectedEquipmentsForCheck);
                    view.Refresh();
                    Check(null);
                }
            }
        }

        private string startDate = DateTime.Now.ToString(new CultureInfo("uk-Ua"));
        public string StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                Check(null);
            }
        } 

        string endDate = DateTime.Now.AddDays(1).ToString(new CultureInfo("uk-Ua"));
        public string EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                Check(null);
            }
        } 

        int startHour = DateTime.Now.Hour;
        public int StartHour
        {
            get => startHour;
            set
            {
                startHour = value;
                Check(null);
            }
        }

        int endHour = DateTime.Now.Hour;
        public int EndHour
        {
            get => endHour;
            set
            {
                endHour = value;
                Check(null);
            }
        }

        private int startMinute = DateTime.Now.Minute;
        public int StartMinute
        {
            get => startMinute;
            set
            {
                startMinute = value;
                Check(null);
            }
        }

        int endMinute = DateTime.Now.Minute;
        public int EndMinute
        {
            get => endMinute;
            set
            {
                endMinute = value;
                Check(null);
            }
        }  

        public List<int> Hours { get; set; }
            = new List<int>()
        {
          0 , 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23
        };

        public List<int> Minutes { get; set; }
       = new List<int>
       {
            00, 01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,
            31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59
       };

        public ObservableCollection<EquipmentInStock> SelectedEquipmentsForCheck { get; set; } = new ObservableCollection<EquipmentInStock>();

        public List<AvailabilityEquipment> CheckResult
        {
            get => _checkResult;
            set
            {
                _checkResult = value;
                OnePropertyChanged();
            }
        }

        private EquipmentInStock selectedEquipmentForCheck;
        public EquipmentInStock SelectedEquipmentForCheck
        {
            get => selectedEquipmentForCheck;
            set
            {
                selectedEquipmentForCheck = value;
                OnePropertyChanged();

                if (value != null)
                {
                    Count = value.Count;
                    SelectedCategory = value.Category;
                    SelectedEquipment = value.Name;

                }
            }
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


        private string _selectedImage;
        private string _selectedEquipment;
        private List<EquipmentInStock> _equipments;

        
        private List<AvailabilityEquipment> _checkResult;

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

        #region Commands
        public Command AddEquipmentCommand { get; set; }

        public Command CheckCommand { get; set; }

        public Command GoToCreateOrderCommand { get; set; }

        public Command RemoveEquipmentCommand { get; set; }
        #endregion

        void AddEquipmentToCheck(object parametr)
        {
            SelectedEquipmentsForCheck.Add(new EquipmentInStock()
            {
                Name = SelectedEquipment,
                Category = SelectedCategory,
                Count = Count,
                ImageUrl = SelectedImage
            });
        }

        bool CanAddEquipment(object parametr)
        {
            if (string.IsNullOrWhiteSpace(SelectedEquipment))
            {
                return false;
            }
            foreach (var item in SelectedEquipmentsForCheck)
            {
                if (item.Name == SelectedEquipment)
                {
                    return false;
                }
            }

            return true;
        }

        async void Check(object parametr)
        {
            var checkResult = new List<AvailabilityEquipment>();

            DateTime tempdate = DateTime.Parse(StartDate, new CultureInfo("uk-UA"));
            DateTime startDate = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day, StartHour, StartMinute, 0);

            DateTime tempEndDate = DateTime.Parse(EndDate, new CultureInfo("uk-UA"));
            DateTime endDate = new DateTime(tempEndDate.Year, tempEndDate.Month, tempEndDate.Day, EndHour, EndMinute, 0);
            EnableProgressBar();
            foreach (var equipment in SelectedEquipmentsForCheck)
            {
                int balance = await EquipmentInfo.GetAvalibleCountAsync(equipment.Name);
                int avalibleInRange = await EquipmentInfo.GetAvalibleCountAsync(equipment.Name, startDate, endDate);
                bool isEnough = balance + avalibleInRange >= equipment.Count;
                checkResult.Add(new AvailabilityEquipment()
                {
                   Name = equipment.Name,
                   NeedCount = equipment.Count,
                   Balance = balance,
                   AvalibleInSelectedDateRange = avalibleInRange,
                   IsEnough = isEnough,
                   Result = isEnough ? "ОK" : "Не хватает",
                   ImageUrl = equipment.ImageUrl,
                   TotalCount = (from equipmentInStock in Equipments
                       where equipmentInStock.Name == equipment.Name
                       select equipmentInStock.TotalCount).Single(),
               }); 
            }
            DisableProgressBar();
            CheckResult = checkResult;
            GoToCreateOrderCommand.OneExecuteChaneged();
        }


        void GoToCreateOrder(object parametr)
        {
            CreateOrderWindow window = new CreateOrderWindow()
            {
                DataContext = new CreateOrderViewModel()
                {
                    SelectedEquipmentsForOrder = SelectedEquipmentsForCheck,
                    Categoryes = this.Categoryes,
                    Count = this.Count,
                    Equipments = this.Equipments,
                    SelectedCategory = this.SelectedCategory,
                    SelectedEquipment = this.SelectedEquipment,
                    StartDate  = this.StartDate,
                    StartHour = this.StartHour,
                    EndDate = this.EndDate,
                    EndHour = this.EndHour,
                    StartMinute = this.StartMinute,
                    EndMinute= this.EndMinute,
                }
            };
            window.Show();
        }

        bool CanGoToCreateOrder(object parametr)
        {
            if (CheckResult.Count == 0)
            {
                return false;
            }

            foreach (var item in CheckResult)
            {
                if (!item.IsEnough)
                {
                    return false;
                }
            }

            return true;
        }

        void RemoveEquipment(object parametr)
        {
            if (SelectedEquipmentForCheck != null)
            {
                SelectedEquipmentsForCheck.Remove(SelectedEquipmentForCheck);
                AddEquipmentCommand.OneExecuteChaneged();
            }

        }
    }
}
