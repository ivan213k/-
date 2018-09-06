using System;
using System.Collections.Generic;
using System.Windows;
using Управление_заказами.Models.Core;
using Управление_заказами.Models.Core.Abstractions;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            IOrderManager manager = new OrderManager();
            //manager.CreateOrder(new Order()
            //{
            //    Equipments = new List<EquipmentFromOrder>()
            //    {
            //        new EquipmentFromOrder()
            //        {
            //            Category = "Стулья",
            //            Count = 112,
            //            Name = "Стул ISO",
            //            StartDate = new DateTime(2018,09,10,16,30,0),
            //            EndDate = new DateTime(2018,09,15,16,30,0),
            //        },
            //        new EquipmentFromOrder()
            //        {
            //        Category = "Дополнительное оборудование",
            //        Count = 69,
            //        Name = "Конус дорожный",
            //        StartDate =  new DateTime(2018,09,10,16,30,0),
            //        EndDate = new DateTime(2018,09,15,16,30,0),
            //        }
            //    },
            //    Adress = "Popova",
            //    CreateDate = new DateTime(2018, 09, 10, 16, 30, 0),
            //    ReturnDate = new DateTime(2018, 09, 15, 16, 30, 0),
            //    CustomerName = "Ivan",
            //    Manager = "Igor",
            //    Status = OrderStatus.Open,
            //    MobilePhone = "380",
            //    Note = "Note"
            //});
            manager.CancelOrder(6);
        }
    }
}
