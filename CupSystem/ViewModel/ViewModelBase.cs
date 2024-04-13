using CupSystem.Helper;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CupSystem.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public static void Close(IClosable window) => window.Close();
    }
}
