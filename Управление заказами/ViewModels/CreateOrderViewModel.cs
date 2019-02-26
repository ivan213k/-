using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Управление_заказами.Models;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;
using Управление_заказами.Views;

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
            GoogleCalendarColors = AppSettings.GoogleCalendarColors;
            SelectedColor = GoogleCalendarColors.Select(c => c).Where(c => c.Key == AppSettings.GoogleCalendarColorId)
                .SingleOrDefault();
            LoadEquipments();
        }
        async void LoadEquipments()
        {
            EnableProgressBar();
            try
            {
                Equipments = await EquipmentInfo.GetEquipmentsAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + e.StackTrace,"",MessageBoxButton.OK,MessageBoxImage.Error);
                throw;
            }
           
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

        public bool AllDateIsChecked { get; set; }

        public Dictionary<string, string> GoogleCalendarColors { get; set; }

        public KeyValuePair<string, string> SelectedColor { get; set; }

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

        private EquipmentFromOrder selectedEquipmentForOrder;
        public EquipmentFromOrder SelectedEquipmentForOrder
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

        public ObservableCollection<EquipmentFromOrder> SelectedEquipmentsForOrder { get; set; } = new ObservableCollection<EquipmentFromOrder>();

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
            SelectedEquipmentsForOrder.Add(new EquipmentFromOrder()
            {
                Category = SelectedCategory,
                Name = SelectedEquipment,
                Count = Count,
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

        private async Task CreateDayOff()
        {
            GoogleCalendar calendar = new GoogleCalendar();
            if (StartDate.Day == EndDate.Day && StartDate.Month == EndDate.Month && StartDate.Year == EndDate.Year)
            {
                await calendar.AddEmployeDayOff(StartDate, CustomerName, AppSettings.GoogleCalendarColorId);
            }
            else
            {
                await calendar.AddEmployeDayOff(StartDate, EndDate, CustomerName, AppSettings.GoogleCalendarColorId);
            }
            MessageBox.Show("Выходной успешно создан","",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        async void CreateOrder(object parametr)
        {
            var window = parametr as Window;
            if (SelectedEquipmentsForOrder.Count == 0)
            {
                await CreateDayOff();
                return;
            }

            if (EndDate < StartDate)
            {
                MessageBox.Show("Дата возврата не может быть ранее даты создания","",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
                GoogleCalendarColorId = SelectedColor.Key,
                IsAllDayEvent = AllDateIsChecked
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
                    EndDate = EndDate,
                    Amount = equipment.Amount
                });
            }

            order.Equipments = equipments;
            try
            {
                EnableProgressBar();
                await OrderManager.CreateOrderAsync(order);
                window.Close();
                MessageBox.Show("Заказ успешно создан","",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            catch (ArgumentException )
            {
                List<EquipmentFromOrder> eqsForOrder = new List<EquipmentFromOrder>();
                foreach (var equipment in SelectedEquipmentsForOrder)
                {
                    eqsForOrder.Add(new EquipmentFromOrder()
                    {
                        Name = equipment.Name,
                        Category =  equipment.Category,
                        Count = equipment.Count,
                        ReplacmentCost = equipment.ReplacmentCost,
                        StartDate = StartDate,
                        EndDate = EndDate
                    });
                }
                var missingEquiomentWindow = new MissingEquipmentWindow()
                {
                    DataContext = new MissingEquipmentViewModel()
                    {
                        Equioments = await EquipmentInfo.GetMissingEquipments(eqsForOrder, 
                            StartDate.AddSeconds(-StartDate.Second), EndDate.AddSeconds(-EndDate.Second))
                    }
                };
                missingEquiomentWindow.ShowDialog();
                if (missingEquiomentWindow.DialogResult == true)
                {
                    MissingEquipmentViewModel viewModel =
                        missingEquiomentWindow.DataContext as MissingEquipmentViewModel;
                    foreach (var missingEquipment in viewModel.Equioments)
                    {
                        var equipment = order.Equipments.Select(e => e).Where(e => e.Name == missingEquipment.Name)
                            .SingleOrDefault();
                        if (equipment.Count == missingEquipment.NotEnough)
                        {
                            order.Equipments.Remove(equipment);
                        }
                        else
                        {
                            equipment.Count -= missingEquipment.NotEnough;
                        }
                        order.Equipments.Add(new EquipmentFromOrder()
                        {
                             Name = $"{missingEquipment.Name} (партнера - {missingEquipment.PartnerName})",
                             Category = equipment.Category,
                             Count = missingEquipment.NotEnough,
                             IsPartnerEquipment = true,
                             PartnerName = missingEquipment.PartnerName,
                             StartDate = equipment.StartDate,
                             EndDate = equipment.EndDate,
                        });
                    }
                    await OrderManager.CreateOrderAsync(order);
                    window.Close();
                    MessageBox.Show("Заказ успешно создан", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
