using CupSystem.Helper;
using JsonFileDatabase.Model;

namespace CupSystem.ViewModel
{
    public class GroupViewModel : ViewModelBase
    {
        public Group? Current { get; set; }
        public List<Player> Players { get; set; } = [];
        List<Match> PlayerMatches { get; set; } = [];
        public List<Player> SortedPlayers
        {
            get
            {
                if (Current is null) return [];
                else return [.. Current.SortPlayersResult()];
            }
        }

        public RelayCommand<Player> SelectCmd { get; set; }
        public RelayCommand<Match> RegisterMatchCmd { get; set; }
        public RelayCommand<Match> ClearMatchCmd { get; set; }
        public RelayCommand<Player> ClearPlayerCmd { get; set; }


        public GroupViewModel()
        {
            SelectCmd = new RelayCommand<Player>(Select);
            RegisterMatchCmd = new RelayCommand<Match>(RegisterMatch);
            ClearMatchCmd = new RelayCommand<Match>(ClearMatch);
            ClearPlayerCmd = new RelayCommand<Player>(ClearPlayerData);
        }

        private void Select(Player player)
        {
            PlayerMatches = [.. Current!.Matches.Where(m => m.A.Equals(player) || m.B.Equals(player))];

            OnPropertyChanged(nameof(PlayerMatches));
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
            OnPropertyChanged(nameof(Current.Matches));
            OnPropertyChanged(nameof(SortedPlayers));
        }

        private void RegisterMatch(Match m)
        {
            m.RegisterMatch();

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
