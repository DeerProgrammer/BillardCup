using CupSystem.ViewModel;
using JsonFileDatabase.Model;
using System.Windows;

namespace CupSystem.View
{
    /// <summary>
    /// Interaction logic for RankingView.xaml
    /// </summary>
    public partial class RankingView : Window
    {
        public RankingView(Ranking r, string c)
        {
            InitializeComponent();
            (DataContext as RankingViewModel)!.SetPlayers(r);
            (DataContext as RankingViewModel)!.SetCupName(c);
        }
    }
}
