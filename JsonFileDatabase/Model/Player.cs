using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JsonFileDatabase.Model
{
    public class Player : INotifyPropertyChanged
    {
        public string Name { get; set; } = string.Empty;
        public string ClubName { get; set; } = string.Empty;
        public double Average { get; set; } = 0.000;
        public int Distance { get; set; } = 0;

        public int GroupId { get; set; }
        public int GroupMP { get; set; }
        public int GroupPoint { get; set; }
        public int GroupInnings { get; set; }
        public int GroupPlacement { get; set; }
        public double GroupAverage => (double)GroupPoint / GroupInnings;
        public double GroupAveragePercent => (GroupAverage / Average) * 100;
        public List<int> GroupSerie { get; set; } = [];
        public List<int> OrderedGroupSerie => [.. GroupSerie.OrderByDescending(x => x)];

        public int FinalWins { get; set; }
        public int FinalPoint { get; set; }
        public int FinalInnings { get; set; }

        public int TotalPoints => GroupPoint + FinalPoint;
        public int TotalInnings => GroupInnings + FinalInnings;
        public double TotalAverage => (double)TotalPoints / TotalInnings;
        public double TotalAveragePercent => (TotalAverage / Average) * 100;
        public List<int> TotalSerie { get; set; } = [];
        public List<int> OrderedTotalSerie => [.. TotalSerie.OrderByDescending(x => x)];
        public int FinalPlacement { get; set; }

        public override string ToString() => $"[{ClubName}] {Name} - {Average:0.###}";


        public event PropertyChangedEventHandler? PropertyChanged;

        public void Updated()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(ClubName));
            OnPropertyChanged(nameof(Average));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ClearData()
        {
            GroupMP = 0;
            GroupPoint = 0;
            GroupInnings = 0;
            GroupPlacement = 0;

            FinalWins = 0;
            FinalPoint = 0;
            FinalInnings = 0;
            FinalPlacement = 0;

            GroupSerie = [];
            TotalSerie = [];
        }
    }
}