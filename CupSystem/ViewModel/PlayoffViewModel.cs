using CupSystem.Helper;
using JsonFileDatabase.Model;
using System.IO;
using System.Text;

namespace CupSystem.ViewModel
{
    public class PlayoffViewModel : ViewModelBase
    {
        private string _cupName = string.Empty;
        public Playoff? Current { get; set; }

        public RelayCommand PrintCmd { get; set; }

        public PlayoffViewModel()
        {
            PrintCmd = new RelayCommand(Print);
        }

        private void Print()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Oversiddere:");
            foreach (var r in Current!.Rounds!.Where(x => x.B.Name == string.Empty))
            {
                var p = r.A;
                sb.AppendLine($"{Truncate(r.A.ClubName, 13),-15} {Truncate(r.A.Name, 18),-20}");
            }
            sb.AppendLine("________________________________________________________________");

            foreach (var r in Current!.Rounds!.Where(x => x.B.Name != string.Empty)) 
            {
                sb.AppendLine($"{"Klub",-15} {"Navn",-20} {"Dist",-4} {"point",-5} {"indg",-4} Vinder");
                sb.AppendLine($"{Truncate(r.A.ClubName, 13),-15} {Truncate(r.A.Name,18),-20} {r.A.Distance,-4} {r.ScoreA,-5} {r.Innings,-4} {(r.AVundet ? "X" : " ")}");
                sb.AppendLine($"{Truncate(r.B.ClubName, 13),-15} {Truncate(r.B.Name,18),-20} {r.B.Distance,-4} {r.ScoreB,-5} {r.Innings,-4} {(r.BVundet ? "X" : " ")}");
                sb.AppendLine();
                sb.AppendLine();
            }


            string filePath = $"C:\\Cups\\{_cupName}\\{Current}.txt";
            FileInfo file = new(filePath);
            file.Directory?.Create();
            File.WriteAllText(file.FullName, sb.ToString());
        }

        public static string? Truncate(string? value, int maxLength, string truncationSuffix = "…")
            => value?.Length > maxLength
                ? string.Concat(value.AsSpan(0, maxLength), truncationSuffix)
                : value;

        public void SetPlayoff(Playoff p)
        {
            Current = p;
            OnPropertyChanged(nameof(Current));
        }
        public void SetCupName(string c) => _cupName = c;
    }
}
