using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JsonFileDatabase.Model
{
    public class Cup : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name { get; set; } = string.Empty;
        public int Add { get; set; } = 30;
        public int Multiply { get; set; } = 30;
        public int MinDistance { get; set; } = 80;
        public int SizeOfFinale { get; set; } = 32;
        public bool AllowLoserBracket { get; set; } = false;

        public List<Player> Players { get; set; } = [];
        public List<Group> Groups { get; set; } = [];
        public List<IRound> Finales { get; set; } = [];
        public Ranking? Rankings { get; set; } = null;


        public void CalculatePlayerDistances()
        {
            foreach (var player in Players)
            {
                var distance = (player.Average * Multiply) + Add;

                player.Distance = distance % 2 > 0 ? (int)++distance : (int)distance;

                if (player.Distance % 2 > 0)
                    player.Distance++;

                if(player.Distance < MinDistance)
                    player.Distance = MinDistance;
            }
        }

        public void Notify(string name)
        {
            OnPropertyChanged(name);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}