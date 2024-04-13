using CupSystem.Helper;
using JsonFileDatabase.Model;

namespace CupSystem.ViewModel
{
    public class GroupViewModel : ViewModelBase
    {
        public Group? Current { get; set; }
        public List<Player> Players { get; set; } = [];
        public Player? Selected { get; set; }
        public List<Player> SortedPlayers 
        {
            get 
            {
                if (Current is null) return [];
                else return [.. Current.SortPlayersResult()];
            }
        }

        public RelayCommand AddCmd { get; set; }
        public RelayCommand<Match> RegisterMatchCmd { get; set; }
        public RelayCommand<Match> ClearMatchCmd { get; set; }
        public RelayCommand<Player> ClearPlayerCmd { get; set; }


        public GroupViewModel()
        {
            AddCmd = new RelayCommand(Add);
            RegisterMatchCmd = new RelayCommand<Match>(RegisterMatch);
            ClearMatchCmd = new RelayCommand<Match>(ClearMatch);
            ClearPlayerCmd = new RelayCommand<Player>(ClearPlayerData);
        }

        private void ClearPlayerData(Player player)
        {
            player.ClearData();

            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(SortedPlayers));
        }

        private void ClearMatch(Match match)
        {
            match.Clear();

            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(SortedPlayers));
        }

        private void RegisterMatch(Match m)
        {
            m.RegisterMatch();

            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(SortedPlayers));
        }

        private void Add()
        {
            if(Selected != null && Current != null)
                Current.Players.Add(Selected);

            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(SortedPlayers));
        }

        public void SetGroup(Group g)
        {
            Current = g;
            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(SortedPlayers));
        }

        public void SetPlayers(List<Player> p)
        {
            Players = p;
            OnPropertyChanged(nameof(Players));
        }
    }
}
