using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(2);

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
                    Count = value.Count;
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
                Count = Count,
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
          
            if (EndDate < StartDate)
            {
                MessageBox.Show("Дата возврата не может быть ранее даты создания");
                return;
            }

            Order order = new Order()
            {
                Adress = SelectedDeliveryTypeIndex == 1 ? this.Adress : "Самовывоз",
                CreateDate = StartDate.AddSeconds(-StartDate.Second),
                CustomerName = CustomerName,
                Manager = AppSettings.CurrentUserName,
                MobilePhone = MobilePhone,
                Note = Note,
                ReturnDate = EndDate.AddSeconds(-EndDate.Second),  
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
                    StartDate = StartDate,
                    EndDate = EndDate
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
    }
}
