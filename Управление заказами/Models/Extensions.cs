using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Управление_заказами.Models
{
    static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            return new ObservableCollection<T>(list);
        }
    }
}
