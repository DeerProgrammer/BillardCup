using CupSystem.Helper;
using JsonFileDatabase.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace CupSystem.ViewModel
{
    public class RankingViewModel : ViewModelBase
    {
        private string _cupName = "";
        public ObservableCollection<Player> Players { get; set; } = [];

        public RelayCommand PrintCmd { get; set; }

        public RankingViewModel()
        {
            PrintCmd = new RelayCommand(Print);
        }

        private void Print()
        {
            string filePath = $"C:\\Cups\\{_cupName}\\Result.txt";

            var sb = new StringBuilder();
            sb.AppendLine($"{"#",-2} {"Klub",-10} {"Navn",-15} {"Dist",-4} {"Score",-5} {"Indg.",-5} {"Gns",-6} {"Pct.",-6} {"Serie",4}");
            foreach (Player p in Players)
            {
                var line = $"{p.FinalPlacement,-2} {Truncate(p.ClubName, 8),-10} {Truncate(p.Name, 14),-15} ";
                line += $"{p.Distance,-4} {p.TotalPoints,-5} {p.TotalInnings,-5} {p.TotalAverage,-6:#.00} ";
                line += $"{p.TotalAveragePercent,5:#.0}% {p.OrderedTotalSerie[0],4}";
                sb.AppendLine(line);
            }
            sb.AppendLine("_________________________________________________________");
            var totalInn = Players.Sum(x => x.TotalInnings);
            var totalPoint = Players.Sum(x => x.TotalPoints);
            var totalAvg = totalPoint / totalInn;
            sb.Append($"Turnering snit: {totalPoint} / {totalInn} = {totalAvg,-5:#.00}");

            FileInfo file = new(filePath);
            file.Directory?.Create();
            File.WriteAllText(file.FullName, sb.ToString());
        }
        public static string? Truncate(string? value, int maxLength, string truncationSuffix = "…")
            => value?.Length > maxLength
                ? string.Concat(value.AsSpan(0, maxLength), truncationSuffix)
                : value;

        public void SetPlayers(Ranking r)
        {
            Players = [.. r.SortPlayersResult()];

            OnPropertyChanged(nameof(Players));
        }
        public void SetCupName(string c) => _cupName = c;
    }
}
