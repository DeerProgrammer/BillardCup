using CupSystem.ViewModel;
using JsonFileDatabase.Model;
using System.Windows;

namespace CupSystem.View
{
    /// <summary>
    /// Interaction logic for KnockoutView.xaml
    /// </summary>
    public partial class KnockoutView : Window
    {
        public KnockoutView(Knockout k, string c)
        {
            InitializeComponent();
            (DataContext as KnockoutViewModel)!.SetKnockout(k);
            (DataContext as KnockoutViewModel)!.SetCupName(c);
        }
    }
}
