using JsonFileDatabase.Model;
using CupSystem.Helper;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CupSystem.View;
using System.Text;
using System.IO;

namespace CupSystem.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public Cup Current { get; set; } = new();

        public ObservableCollection<IRound> CurrentRounds { get; set; } = [];

        public RelayCommand LoadCupCmd { get; set; }
        public RelayCommand SaveCupCmd { get; set; }
        public RelayCommand<bool> EditCupCmd { get; set; }
        public RelayCommand AddPlayersCmd { get; set; }
        public RelayCommand CreateGroupCmd { get; set; }
        public RelayCommand PrintGroupCmd { get; set; }
        public RelayCommand<int> StartFinalsCmd { get; set; }
        public RelayCommand<int> OpenRoundCmd { get; set; }
        public RelayCommand<int> DeleteRoundCmd { get; set; }

        public MainViewModel()
        {
            LoadCupCmd = new RelayCommand(LoadCup);
            SaveCupCmd = new RelayCommand(SaveCup);
            EditCupCmd = new RelayCommand<bool>(EditCup);
            AddPlayersCmd = new RelayCommand(AddPlayers);
            CreateGroupCmd = new RelayCommand(CreateGroup);
            OpenRoundCmd = new RelayCommand<int>(OpenRound);
            StartFinalsCmd = new RelayCommand<int>(StartFinals);
            PrintGroupCmd = new RelayCommand(PrintGroup);
            DeleteRoundCmd = new RelayCommand<int>(DeleteRound);

            CurrentRounds.CollectionChanged += CurrentGroupsColChange;
        }

        private void DeleteRound(int id)
        {
            var roundToDelete = CurrentRounds.First(x => x.Id == id);

            roundToDelete.ClearAll();

            if (roundToDelete is Group g)
                Current.Groups.Remove(g);
            else if (roundToDelete is Ranking r)
                Current.Rankings = null;
            else
                Current.Finales.Remove(roundToDelete);

            CurrentRounds = [.. Current.Groups.OrderBy(x => x.Id), .. Current.Finales];
        }

        private void PrintGroup()
        {
            var sb = new StringBuilder();
            foreach (var group in Current.Groups)
            {
                sb.AppendLine($"Pulje: {group.Id}");
                sb.AppendLine($"{"#",-2} {"Klub",-10} {"Navn",-15} {"Dist",-4} {"Score",-5} {"Indg.",-5} {"Gns",-6} {"Pct.",-6} {"MP",4}");

                int plac = 1;
                foreach (var p in group.SortPlayersResult())
                {
                    sb.AppendLine($"{plac++,-2} {Truncate(p.ClubName, 8),-10} {Truncate(p.Name, 14),-15} {p.Distance,-4} {p.GroupPoint,-5} {p.GroupInnings,-5} {p.GroupAverage,5:#.00}  {p.GroupAveragePercent,5:#.0}% {p.GroupMP,4}");
                }
                sb.AppendLine("________________________________________________________________");
            }

            string filePath = $"C:\\Cups\\{Current.Name}\\Groups.txt";

            FileInfo file = new(filePath);
            file.Directory?.Create();
            File.WriteAllText(file.FullName, sb.ToString());
        }
        public static string? Truncate(string? value, int maxLength, string truncationSuffix = "…")
            => value?.Length > maxLength
                ? string.Concat(value.AsSpan(0, maxLength), truncationSuffix)
                : value;

        private void StartFinals(int id)
        {
            var round = CurrentRounds.First(x => x.Id == id);
            if (round is Group)
                FirstRound();
            else if (round is Knockout k2 && k2.IsFinale)
                Result(k2);
            else if (round is Knockout k && k.Players.Count == 4)
                LastRound(round);
            else NextRound(round);

            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(CurrentRounds));
        }

        private void Result(Knockout finale)
        {
            var result = new Ranking(Current, finale);

            Current.Rankings = result;
            CurrentRounds = [.. Current.Groups.OrderBy(x => x.Id), .. Current.Finales, Current.Rankings];
        }

        private void LastRound(IRound lastRound)
        {
            var finalists = lastRound.SortPlayersResult();
            var theRest = lastRound.Players.Except(finalists);

            if (Current.AllowLoserBracket)
            {
                var round = new Knockout([.. finalists], lastRound.Type, true);
                Current.Finales.Add(round);
            }
            else
            {
                var round = new Knockout([finalists[0], .. theRest, finalists[1]], lastRound.Type, true);
                Current.Finales.Add(round);
            }

            CurrentRounds = [.. Current.Groups.OrderBy(x => x.Id), .. Current.Finales];
        }

        private void NextRound(IRound lastRound)
        {
            var result = lastRound.SortPlayersResult();
            var round = new Knockout(result, lastRound.Type);

            Current.Finales.Add(round);
            CurrentRounds = [.. Current.Groups.OrderBy(x => x.Id), .. Current.Finales];
        }

        private void FirstRound()
        {
            IRound round;
            var ranked = RankGroupPlayersAndTakeSize(Current.Groups, Current.SizeOfFinale);
            if (IsPowerOfTwo(ranked.Count))
                round = new Knockout(ranked);
            else
                round = new Playoff(LowestPowerOfTwoSize(Current.SizeOfFinale), ranked);

            Current.Finales.Add(round);

            if (Current.AllowLoserBracket)
            {
                IRound lRound;
                var losers = RankGroupPlayersAndSkipSize(Current.Groups, Current.SizeOfFinale);

                if (IsPowerOfTwo(losers.Count))
                    lRound = new Knockout(losers, "B");
                else
                    lRound = new Playoff(LowestPowerOfTwoSize(losers.Count), losers, "B");

                Current.Finales.Add(lRound);
            }
            
            CurrentRounds = [.. Current.Groups.OrderBy(x => x.Id), .. Current.Finales];
        }

        private static bool IsPowerOfTwo(int x) => (x & (x - 1)) == 0;
        private static int LowestPowerOfTwoSize(int x)
        {
            int powered = 0;
            for (int i = 0; i < 10; i++)
            {
                var ipowered = (int)Math.Pow(2, i);
                if (ipowered > x) break;
                powered = ipowered;
            }
            return powered;
        }

        private static List<Player> RankGroupPlayersAndTakeSize(List<Group> groups, int take)
        {
            var ranked = SelectSeeds(groups, 1);
            ranked.AddRange(SelectSeeds(groups, 2));
            ranked.AddRange(SelectSeeds(groups, 3));
            //ranked.AddRange(SelectSeeds(groups, 4));
            return [.. ranked.Take(take)];
        }
        private static List<Player> RankGroupPlayersAndSkipSize(List<Group> groups, int skip)
        {
            var ranked = SelectSeeds(groups, 1);
            ranked.AddRange(SelectSeeds(groups, 2));
            ranked.AddRange(SelectSeeds(groups, 3));
            //ranked.AddRange(SelectSeeds(groups, 4));
            return [.. ranked.Skip(skip)];
        }

        private static List<Player> SelectSeeds(List<Group> groups, int number)
        {
            List<Player> ranked = [.. groups.Select(x => x.SortPlayersResult()
                                           .Skip(number - 1)
                                           .First())
                             .OrderByDescending(x => x.GroupAveragePercent)
                             .ThenByDescending(x => x.OrderedGroupSerie.FirstOrDefault())
                             .ThenByDescending(x => x.OrderedGroupSerie.Skip(1).FirstOrDefault())
                             .ThenByDescending(x => x.OrderedGroupSerie.Skip(2).FirstOrDefault())];

            foreach (var player in ranked)
            {
                player.GroupPlacement = number;
            }

            return ranked;
        }

        private void CurrentGroupsColChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentRounds));
        }

        private void OpenRound(int id)
        {
            var round = CurrentRounds.First(x => x.Id == id);

            if (round is Group g)
            {
                var window = new GroupView(g, Current.Players);
                window.Show();

                window.Closed += OnClose;
            }
            else if (round is Playoff p)
            {
                var window = new PlayoffView(p, Current.Name);
                window.Show();

                window.Closed += OnClose;
            }
            else if (round is Knockout k)
            {
                var window = new KnockoutView(k, Current.Name);
                window.Show();

                window.Closed += OnClose;
            }
            else if (round is Ranking r)
            {
                var window = new RankingView(r, Current.Name);
                window.Show();
            }
        }

        private void OnClose(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(CurrentRounds));
        }

        private void CreateGroup()
        {
            Current.Groups = [];
            Current.CalculatePlayerDistances();
            foreach (var id in Current.Players.Select(x => x.GroupId).Distinct()) 
            {
                var players = Current.Players.Where(x => x.GroupId == id).ToList();
                var group = new Group(id, players);
                Current.Groups.Add(group);
            }
            Current.Groups = [.. Current.Groups.OrderBy(x => x.Id)];
            CurrentRounds = [.. Current.Groups.OrderBy(x => x.Id), ..Current.Finales];
            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(CurrentRounds));
        }

        private void AddPlayers()
        {
            DeltagereWindow window = new(Current.Players, Current.Name);
            window.OnPlayersUpdated += OnPlayersUpdated;
            window.Show();

            OnPropertyChanged(nameof(Current));
        }

        private void OnPlayersUpdated(List<Player> p) => Current.Players = p;

        private void LoadCup()
        {
            var fileDialog = new OpenFileDialog
            {
                Title = "Åbn Cup",
                DefaultExt = "Json",
                Filter = "Json|*.json",
                Multiselect = false
            };

            if (fileDialog.ShowDialog() == true)
                Current = JsonFileDb.Instance.FindCup(fileDialog.FileName);

            CurrentRounds = [.. Current.Groups.OrderBy(x => x.Id), .. Current.Finales];
            if(Current.Rankings != null) CurrentRounds.Add(Current.Rankings);
            
            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(CurrentRounds));
        }

        private void SaveCup()
        {
            var fileDialog = new SaveFileDialog
            {
                Title = "Gem Cup",
                DefaultExt = "Json",
                Filter = "Json|*.json"
            };

            if (fileDialog.ShowDialog() == true)
                JsonFileDb.Instance.SaveCup(Current, fileDialog.FileName);

            OnPropertyChanged(nameof(Current));
        }

        private void EditCup(bool isNew)
        {
            Cup c = Current;
            if (isNew)
                c = new Cup();

            var window = new CupView(c);

            window.CupSavedEvent += OnCupSaved;

            window.Show();
        }

        private void OnCupSaved(Cup c)
        {
            Current = c;

            OnPropertyChanged(nameof(Current));
        }
    }
}
