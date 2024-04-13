using CupSystem.Helper;
using JsonFileDatabase.Model;

namespace CupSystem.ViewModel
{
    public class CupViewModel : ViewModelBase
    {
        public delegate void CupSaved(Cup c);
        public event CupSaved? CupSavedEvent;
        public Cup Current { get; set; } = new();

        public RelayCommand<IClosable> SaveCmd { get; set; }
        
        public CupViewModel()
        {
            SaveCmd = new RelayCommand<IClosable>(Save);
        }

        private void Save(IClosable window)
        {
            CupSavedEvent?.Invoke(Current);
            window.Close();
        }

        public void SetCup(Cup cup)
        {
            Current = cup;
            OnPropertyChanged(nameof(Current));
        }
    }
}
