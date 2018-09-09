using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Управление_заказами.ViewModels
{
    class BaseViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        
        protected void OnePropertyChanged([CallerMemberName] string propName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
