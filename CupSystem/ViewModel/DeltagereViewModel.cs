using CupSystem.Helper;
using JsonFileDatabase.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace CupSystem.ViewModel
{
    public class DeltagereViewModel : ViewModelBase
    {
        private string _cupName = string.Empty;
        public ObservableCollection<Player> Players { get; set; } = [];
        public Player SelectedPlayer { get; set; } = new();

        public RelayCommand CreateCmd { get; set; }
        public RelayCommand UpdateCmd { get; set; }
        public RelayCommand DeleteCmd { get; set; }
        public RelayCommand PrintCmd { get; set; }
        public RelayCommand PrintByClubCmd { get; set; }

        public DeltagereViewModel()
        {
            CreateCmd = new RelayCommand(Create);
            UpdateCmd = new RelayCommand(Update);
            DeleteCmd = new RelayCommand(Delete);
            PrintCmd = new RelayCommand(PrintByAvg);
            PrintByClubCmd = new RelayCommand(PrintByClub);
        }

        private void Delete()
        {
            Players.Remove(SelectedPlayer);

            SelectedPlayer = new();
            Players = [.. Players];
            OnPropertyChanged(nameof(Players));
            OnPropertyChanged(nameof(SelectedPlayer));
        }

        private void PrintByClub()
        {
            string filePath = $"C:\\Cups\\{_cupName}\\Players-Klubsort.txt";

            var sb = new StringBuilder();
            foreach (Player player in Players.OrderByDescending(x => x.ClubName).ThenBy(x => x.Name))
            {
                sb.AppendLine(player.ToString());
            }

            FileInfo file = new(filePath);
            file.Directory?.Create();
            File.WriteAllText(file.FullName, sb.ToString());
        }

        private void PrintByAvg()
        {
            string filePath = $"C:\\Cups\\{_cupName}\\Players.txt";

            var sb = new StringBuilder();
            foreach (Player player in Players.OrderByDescending(x => x.Average))
            {
                sb.AppendLine(player.ToString());
            }

            FileInfo file = new(filePath);
            file.Directory?.Create();
            File.WriteAllText(file.FullName, sb.ToString());
        }

        private void Update()
        {
            Players = [.. Players];
            OnPropertyChanged(nameof(Players));
        }

        private void Create()
        {
            var p = new Player
            { 
                Name = SelectedPlayer.Name,
                ClubName = SelectedPlayer.ClubName,
                Average = SelectedPlayer.Average,
                GroupId = SelectedPlayer.GroupId
            };

            Players.Add(p);
        }

        public void SetPlayers(List<Player> players)
        {
            Players = [.. players];
            OnPropertyChanged(nameof(Players));
        }

        public void ChangeSelection(Player p)
        {
            SelectedPlayer = p;
            OnPropertyChanged(nameof(SelectedPlayer));
        }
        public void SetCupName(string n) => _cupName = n;
    }
}
