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
    class EditOrderViewModel : BaseViewModel
    {
        private readonly IEquipmentInfo EquipmentInfo = new EquipmentInfo();
        private readonly IOrderManager OrderManager = new OrderManager();

        public Order OldOrder { get; set; }

        public EditOrderViewModel()
        {
            AddEquipmentCommand = new Command(AddEquipmentToOrder,CanAddEquipmentToOrder);
            EditOrderCommand = new Command(UpdateOrder);
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


        private int count = 1;
        public int Count
        {
            get => count;
            set
            {
                count = value;
                OnePropertyChanged();
                if (SelectedEquipmentForOrder != null && SelectedEquipment == SelectedEquipmentForOrder.Name && SelectedEquipmentForOrder.Count != value)
                {
                    SelectedEquipmentForOrder.Count = value;
                    ICollectionView view = CollectionViewSource.GetDefaultView(SelectedEquipmentsForOrder);
                    view.Refresh();
                }
            }
        }

        public string StartDate { get; set; } 

        public string EndDate { get; set; } 

        public int StartHour { get; set; } 

        public int EndHour { get; set; } 

        public int StartMinute { get; set; } 

        public int EndMinute { get; set; } 

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

        public ObservableCollection<EquipmentInStock> SelectedEquipmentsForOrder { get; set; } = new ObservableCollection<EquipmentInStock>();

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
                    Count = value.Count;
                    SelectedCategory = value.Category;
                    SelectedEquipment = value.Name;

                }
            }
        }

        public string Adress { get; set; }

        public string MobilePhone { get; set; }

        public string Note { get; set; }

        public string CustomerName { get; set; }

        public string SelectetDeliveryType { get; set; }

        public int SelectedDeliveryIndex { get; set; }


        private void RevoveEquipment(object obj)
        {
            if (SelectedEquipmentForOrder != null)
            {
                SelectedEquipmentsForOrder.Remove(SelectedEquipmentForOrder);
                AddEquipmentCommand.OneExecuteChaneged();
            }
        }

        private async void UpdateOrder(object obj)
        {
            DateTime tempdate = DateTime.Parse(StartDate, new CultureInfo("uk-UA"));
            DateTime tempReturnDate = DateTime.Parse(EndDate, new CultureInfo("uk-UA"));
            DateTime startDate = new DateTime(tempdate.Year, tempdate.Month, tempdate.Day, StartHour, StartMinute,0);
            DateTime returnDate = new DateTime(tempReturnDate.Year, tempReturnDate.Month, tempReturnDate.Day,
                EndHour, EndMinute, 0);

            if (returnDate < startDate)
            {
                MessageBox.Show("Дата возврата не может быть ранее даты создания");
                return;
            }

            var equipmentsForOrder = new List<EquipmentFromOrder>();
            foreach (var equipmentInStock in SelectedEquipmentsForOrder)
            {
                equipmentsForOrder.Add(new EquipmentFromOrder()
                {
                    Category = equipmentInStock.Category,
                    Count = equipmentInStock.Count,
                    Name = equipmentInStock.Name,
                    EndDate = returnDate,
                    StartDate = startDate
                });
            }
            EnableProgressBar();
            try
            {
                await OrderManager.UpdateOrderAsync(OldOrder.Id, new Order()
                {
                    Adress = SelectetDeliveryType.Contains("Указать адрес") ? this.Adress : "Самовывоз",
                    CustomerName = CustomerName,
                    MobilePhone = MobilePhone,
                    Manager = AppSettings.CurrentUserName,
                    ReturnDate = returnDate,
                    CreateDate = startDate,
                    Note = Note,
                    Status = OrderStatus.Open,
                    Equipments = equipmentsForOrder,
                    GoogleCalendarColorId = AppSettings.GoogleCalendarColorId
                });
                (obj as Window).Close();
                MessageBox.Show("Заказ успешно обновлено");
            }
            catch (Exception e)
            {
                MessageBox.Show("Не хватает оборудования. Проверьте наличие.");
            }
           
            DisableProgressBar();
        }

        private void AddEquipmentToOrder(object obj)
        {
            SelectedEquipmentsForOrder.Add(new EquipmentInStock()
            {
                Category = SelectedCategory,
                Name = SelectedEquipment,
                Count = Count,
                ImageUrl = SelectedImage,
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

        public ICommand RemoveEquipmentCommand { get; set; }

        public ICommand EditOrderCommand { get; set; }

        public Command AddEquipmentCommand { get; set; }

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
