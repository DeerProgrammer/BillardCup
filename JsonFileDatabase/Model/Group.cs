namespace JsonFileDatabase.Model
{
    public class Group : IRound
    {
        public int Id { get; private set; }
        public List<Player> Players { get; } = [];
        public List<Match> Matches { get; set; } = [];

        public Group(int id, List<Player> players)
        {
            Id = id;
            Players = players;
            GenerateMatches();
        }

        public List<Player> SortPlayersResult()
            => [.. Players.OrderByDescending(x => x.GroupMP)
                          .ThenByDescending(x => x.GroupAveragePercent)
                          .ThenByDescending(x => x.OrderedGroupSerie.FirstOrDefault())
                          .ThenByDescending(x => x.OrderedGroupSerie.Skip(1).FirstOrDefault())
                          .ThenByDescending(x => x.OrderedGroupSerie.Skip(2).FirstOrDefault())];

        private void GenerateMatches()
        {
            for (int i = 0; i < Players.Count; i++) 
            {
                var player = Players[i];
                for (int j = i + 1; j < Players.Count; j++)
                {
                    var opponent = Players[j];

                    Matches.Add(new Match(player, opponent, Guid.NewGuid().GetHashCode()));
                }
            }
        }

        public override string ToString() => $"Pulje {Id}";

        public void ClearAll()
        {
            foreach (var match in Matches)
            {
                match.Clear();
            }
        }
    }
}