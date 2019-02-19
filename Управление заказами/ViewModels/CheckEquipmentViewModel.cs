using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            public int NotEnough
            {
                get
                {
                   int notEnough = NeedCount - (Balance + AvalibleInSelectedDateRange);
                   return notEnough > 0 ? notEnough : 0;
                }
            }

            public int TotalAvalibleCount
            {
                get { return Balance + AvalibleInSelectedDateRange; }
            }

            public int WillRemainInStock
            {
                get
                {
                    int remainCount = (Balance+AvalibleInSelectedDateRange) - NeedCount;
                    return remainCount > 0 ? remainCount : 0;
                }
            }
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
            Check();
        }

        async void LoadEquipments()
        {
            EnableProgressBar();
            Equipments = await EquipmentInfo.GetEquipmentsAsync();
            DisableProgressBar();
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
                if (SelectedEquipmentForCheck != null && SelectedEquipment == SelectedEquipmentForCheck.Name && SelectedEquipmentForCheck.Count != value)
                {
                    SelectedEquipmentForCheck.Count = value;
                    ICollectionView view = CollectionViewSource.GetDefaultView(SelectedEquipmentsForCheck);
                    view.Refresh();
                    Check();
                }
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                Check();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value; 
                Check();
            }
        }

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

        private string _selectedImage;
        private string _selectedEquipment;
        private List<EquipmentInStock> _equipments;


        private List<AvailabilityEquipment> _checkResult;

        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now.AddDays(2);

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

        async void Check(object parametr = null)
        {
            var checkResult = new List<AvailabilityEquipment>();

            EnableProgressBar();
            foreach (var equipment in SelectedEquipmentsForCheck)
            {
                int balance = await EquipmentInfo.GetAvalibleCountAsync(equipment.Name);
                int avalibleInRange = await EquipmentInfo.GetAvalibleCountAsync(equipment.Name, StartDate.AddSeconds(-StartDate.Second), EndDate.AddSeconds(-EndDate.Second));
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
                    StartDate = this.StartDate,
                    EndDate = this.EndDate,
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
