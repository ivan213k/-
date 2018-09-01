using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
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
                string credPath = "token.json";
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }
        }

        public async Task AddEvent(Order order, string colorId)
        {
           throw new NotImplementedException();
        }

        public async Task AddReturnEvent(Order order, string colorId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateEvent(Order order, string colorId)
        {
            throw new NotImplementedException();
        }
        public async Task UpdateReturnEvent(Order order, string colorId)
        {
            throw new NotImplementedException();
        }
    }
}
