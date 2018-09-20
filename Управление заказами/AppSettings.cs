using System.Collections.Generic;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами
{
    static class AppSettings
    {
        static AppSettings()
        {
           GoogleCalendarColors = new Dictionary<string, string>()
         {
             {"1","#a4bdfc"},
             {"2","#7ae7bf"},
             {"3","#dbadff"},
             {"4","#ff887c"},
             {"5","#fbd75b"},
             {"6","#ffb878"},
             {"7","#46d6db"},
             {"8","#e1e1e1"},
             {"9","#5484ed"},
             {"10","#51b749"},
             {"11","#dc2127"}
         };     
        }
        public static string CurrentUserName { get; set; }

        public static string GoogleCalendarColorId { get; set; } = "1";

        public static Dictionary<string,string> GoogleCalendarColors { get; set; }

        public static AccountType AccountType { get; set; }
    }
}
