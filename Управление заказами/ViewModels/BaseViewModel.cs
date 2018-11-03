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

        protected void DisableProgressBar()
        {
            IsEnabled = true;
            IsDeterminate = false;
        }

        protected void EnableProgressBar()
        {
            IsEnabled = false;
            IsDeterminate = true;
        }
    }
}
