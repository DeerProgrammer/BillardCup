using CupSystem.Helper;
using CupSystem.ViewModel;
using JsonFileDatabase.Model;
using System.Windows;

namespace CupSystem.View
{
    public partial class CupView : Window, IClosable
    {
        public delegate void CupSaved(Cup c);
        public event CupSaved? CupSavedEvent;
        public CupView(Cup c)
        {
            InitializeComponent();

            (DataContext as CupViewModel)!.SetCup(c);

            (DataContext as CupViewModel)!.CupSavedEvent += OnCupSaved;
        }

        private void OnCupSaved(Cup c)
            => CupSavedEvent?.Invoke(c);
    }
}