using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.ViewModels
{
    class CreateOrderViewModel : BaseViewModel
    {
        private readonly IEquipmentInfo EquipmentInfo = new EquipmentInfo();
        private readonly IOrderManager OrderManager = new OrderManager();
        public CreateOrderViewModel()
        {
            AddEquipmentCommand = new Command(AddEquipmentToOrder, CanAddEquipmentToOrder);
            CreateOrderCommand = new Command(CreateOrder);
            RemoveEquipmentCommand = new Command(RevoveEquipment);
            LoadEquipments();
        }
        async void LoadEquipments()
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
                SelectedImage = (from eq in Equipments
                    where eq.Name == value
                    select eq.ImageUrl).FirstOrDefault();
                AddEquipmentCommand.OneExecuteChaneged();
                OnePropertyChanged();
            }
        }


        private string count = "1";
        public string Count
        {
            get => count;
            set
            {
                if (!int.TryParse(value, out int result)) return;
                count = value;
                OnePropertyChanged();
                if (SelectedEquipmentForOrder != null && SelectedEquipment == SelectedEquipmentForOrder.Name && SelectedEquipmentForOrder.Count != int.Parse(value))
                {
                    SelectedEquipmentForOrder.Count = int.Parse(value);
                    ICollectionView view = CollectionViewSource.GetDefaultView(SelectedEquipmentsForOrder);
                    view.Refresh();
                }
            }
        }


        public string StartDate { get; set; } = DateTime.Now.ToString(new CultureInfo("uk-Ua"));

        public string EndDate { get; set; } = DateTime.Now.AddDays(1).ToString(new CultureInfo("uk-Ua"));

        public int StartHour { get; set; } = DateTime.Now.Hour;

        public int EndHour { get; set; } = DateTime.Now.Hour;

        public int StartMinute { get; set; } = DateTime.Now.Minute;

        public int EndMinute { get; set; } = DateTime.Now.Minute;

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

        string selectedImage;
        public string SelectedImage
        {
            get => selectedImage;
            set
            {
                selectedImage = value;
                OnePropertyChanged();
            }
        }

        private EquipmentInStock selectedEquipmentForOrder;
        public EquipmentInStock SelectedEquipmentForOrder
        {
            get => selectedEquipmentForOrder;
            set
            {
                selectedEquipmentForOrder = value;
                OnePropertyChanged();

                if (value != null)
                {
                    Count = value.Count.ToString();
                    SelectedCategory = value.Category;
                    SelectedEquipment = value.Name;

                }
            }
        }

        public ObservableCollection<EquipmentInStock> SelectedEquipmentsForOrder { get; set; } = new ObservableCollection<EquipmentInStock>();

        public string Adress { get; set; }

        public string MobilePhone { get; set; }

        public string Note { get; set; }

        public string CustomerName { get; set; }

        public int SelectedDeliveryTypeIndex { get; set; } = 0;

        #region Commands
        public Command AddEquipmentCommand { get; set; }

        public ICommand CreateOrderCommand { get; set; }

        public ICommand RemoveEquipmentCommand { get; set; }

        #endregion

        void AddEquipmentToOrder(object parametr)
        {
            SelectedEquipmentsForOrder.Add(new EquipmentInStock()
            {
                Category = SelectedCategory,
                Name = SelectedEquipment,
                Count = int.Parse(Count),
                ImageUrl = SelectedImage
            });
        }

        bool CanAddEquipmentToOrder(object parametr)
        {
            if (string.IsNullOrWhiteSpace(SelectedEquipment))
            {
                return false;
            }
            foreach (var item in SelectedEquipmentsForOrder)
            {
                if (item.Name == SelectedEquipment)
                {
                    return false;
                }
            }

            return true;
        }

        async void CreateOrder(object parametr)
        {
            var window = parametr as Window;
            if (SelectedEquipmentsForOrder.Count == 0)
            {
                MessageBox.Show("Невозможно создать заказ. Оборудование для заказа не выбрано");
                return;
            }
            DateTime tempdate = DateTime.Parse(StartDate, new CultureInfo("uk-UA"));
            DateTime tempReturnDate = DateTime.Parse(EndDate, new CultureInfo("uk-UA"));
            DateTime startDate = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day, StartHour, StartMinute, 0);
            DateTime endDate = new DateTime(tempReturnDate.Year, tempReturnDate.Month, tempReturnDate.Day, EndHour, EndMinute, 0);

            if (endDate < startDate)
            {
                MessageBox.Show("Дата возврата не может быть ранее даты создания");
                return;
            }

            Order order = new Order()
            {
                Adress = SelectedDeliveryTypeIndex == 1 ? this.Adress : "Самовывоз",
                CreateDate = startDate,
                CustomerName = CustomerName,
                Manager = AppSettings.CurrentUserName,
                MobilePhone = MobilePhone,
                Note = Note,
                ReturnDate = endDate,  
                GoogleCalendarColorId = AppSettings.GoogleCalendarColorId
            };
            List<EquipmentFromOrder> equipments = new List<EquipmentFromOrder>();
            foreach (var equipment in SelectedEquipmentsForOrder)
            {
                equipments.Add(new EquipmentFromOrder()
                {
                    Category = equipment.Category,
                    Count = equipment.Count,
                    Name = equipment.Name,
                    StartDate = startDate,
                    EndDate = endDate
                });
            }

            order.Equipments = equipments;
            try
            {
                EnableProgressBar();
                await OrderManager.CreateOrderAsync(order);
                window.Close();
                MessageBox.Show("Заказ успешно создан");
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Не хватает оборудования. Проверьте наличие.");
            }
            
            DisableProgressBar();
        }


        private void RevoveEquipment(object parametr)
        {
            if (SelectedEquipmentForOrder != null)
            {
                SelectedEquipmentsForOrder.Remove(SelectedEquipmentForOrder);
                AddEquipmentCommand.OneExecuteChaneged();
            }
            
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

        #region Help Property

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

        #endregion
    }
}
