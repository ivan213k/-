using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Models.Core
{
    class GoogleCalendar
    {
        static readonly string[] Scopes = { CalendarService.Scope.Calendar };
        static readonly string ApplicationName = "Google Calendar API .NET OrderManagment";

        async Task<UserCredential> Authorize()
        {
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Управление заказами", "token_json");
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }
        }

        public async Task ReAuthorizeAsync()
        {
            string credPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Управление заказами", "token_json");
            DirectoryInfo info = new DirectoryInfo(credPath);
            info.Delete(true);
            await Authorize();
        }

        public async Task<string> AddEvent(Order order)
        {
            CalendarService service = await GetService();

            Event startEvent = CreateEvent(order, order.GoogleCalendarColorId);

            return (await service.Events.Insert(startEvent, "primary").ExecuteAsync()).Id;
        }

        public async Task<string> AddReturnEvent(Order order)
        {
            CalendarService service = await GetService();

            Event returnEvent = CreateReturnEvent(order, order.GoogleCalendarColorId);

            return (await service.Events.Insert(returnEvent, "primary").ExecuteAsync()).Id;
        }

        public async Task<string> AddFullTimeEvent(Order order)
        {
            CalendarService service = await GetService();

            Event fullTimeEvent = CreateFullTimeEvent(order, order.GoogleCalendarColorId);

            return (await service.Events.Insert(fullTimeEvent, "primary").ExecuteAsync()).Id;
        }

        public async Task<string> UpdateEvent(Order oldOrder, Order newOrder)
        {
            var service = await GetService();
            
            await service.Events.Delete("primary", oldOrder.EventId).ExecuteAsync();
            Event startEvent = CreateEvent(newOrder, newOrder.GoogleCalendarColorId);
            return (await service.Events.Insert(startEvent, "primary").ExecuteAsync()).Id;
        }
        public async Task<string> UpdateReturnEvent(Order oldOrder, Order newOrder)
        {
            var service = await GetService();

            service.Events.Delete("primary", oldOrder.ReturnEventId).Execute();
            Event returnEvent = CreateReturnEvent(newOrder, newOrder.GoogleCalendarColorId);
            return (await service.Events.Insert(returnEvent, "primary").ExecuteAsync()).Id;
        }
        private async Task<CalendarService> GetService()
        {
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = await Authorize(),
                ApplicationName = ApplicationName,
            });
        }
        private Event CreateEvent(Order order, string colorId)
        {
            StringBuilder equipments = new StringBuilder();

            foreach (var equipment in order.Equipments)
            {
                equipments.Append($"{equipment.Name} {equipment.Count} шт. \n");
            }
            return new Event
            {
                Start = new EventDateTime()
                {
                    DateTime = order.CreateDate
                },
                End = new EventDateTime()
                {
                    DateTime = order.CreateDate.AddHours(2)
                },
                Summary = $"Заказ {order.CustomerName} {order.MobilePhone}",
                Location = order.Adress,
                Description = $"{equipments.ToString()} \n {order.CreateDate.ToShortDateString()} - {order.ReturnDate.ToShortDateString()} \n {order.Note}",
                ColorId = colorId,

            };
        }

        private Event CreateReturnEvent(Order order, string colorId)
        {
            StringBuilder equipments = new StringBuilder();

            foreach (var equipment in order.Equipments)
            {
                equipments.Append($"{equipment.Name} {equipment.Count} шт. \n");
            }
            return new Event()
            {
                Start = new EventDateTime()
                {
                    DateTime = order.ReturnDate
                },
                End = new EventDateTime()
                {
                    DateTime = order.ReturnDate.AddHours(2)
                },
                Summary = order.Adress == "Самовывоз" ? $"Возврат {order.CustomerName} {order.MobilePhone}" : $"Забрать {order.CustomerName} {order.MobilePhone}",
                Location = order.Adress,
                Description = $"{equipments} \n {order.CreateDate.ToShortDateString()} - {order.ReturnDate.ToShortDateString()} \n {order.Note}",
                ColorId = colorId,
            };
        }

        private Event CreateFullTimeEvent(Order order, string colorId)
        {
            StringBuilder equipments = new StringBuilder();

            foreach (var equipment in order.Equipments)
            {
                equipments.Append($"{equipment.Name} {equipment.Count} шт. \n");
            }

            return new Event()
            {
                Start = new EventDateTime()
                {
                    DateTime = order.CreateDate
                },
                End = new EventDateTime()
                {
                    DateTime = order.ReturnDate
                },
                Summary = $"{order.CustomerName} {order.MobilePhone}",
                Location = order.Adress,
                Description = $"{equipments} \n {order.CreateDate.ToShortDateString()} - {order.ReturnDate.ToShortDateString()} \n {order.Note}",
                ColorId = colorId,
            };
        }
    }
}
