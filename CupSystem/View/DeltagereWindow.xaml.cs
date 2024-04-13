using CupSystem.ViewModel;
using JsonFileDatabase.Model;
using System.Windows;
using System.Windows.Controls;

namespace CupSystem.View
{
    /// <summary>
    /// Interaction logic for DeltagereWindow.xaml
    /// </summary>
    public partial class DeltagereWindow : Window
    {
        public delegate void PlayersUpdated(List<Player> p);
        public event PlayersUpdated? OnPlayersUpdated;
        public DeltagereWindow(List<Player> players, string n)
        {
            InitializeComponent();

            (DataContext as DeltagereViewModel)!.SetPlayers(players);
            (DataContext as DeltagereViewModel)!.SetCupName(n);

            Closed += OnClose;
        }

        private void OnClose(object? sender, EventArgs e)
        {
            List<Player> players = [.. (DataContext as DeltagereViewModel)!.Players];
            OnPlayersUpdated?.Invoke(players);
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
            var box = (ListBox)sender;
            var p = (Player)box.SelectedItem;
            if(p is not null)
                (DataContext as DeltagereViewModel)!.ChangeSelection(p!);
        }
    }
}