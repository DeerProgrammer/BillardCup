namespace JsonFileDatabase.Model
{
    public class Playoff() : IRound
    {
        public int Id { get; set; } = Guid.NewGuid().GetHashCode();
        public string Type { get; set; } = "";

        public List<Player> Players { get; set; } = [];
        public List<KnockoutMatch> Rounds { get; set; } = [];
        
        public Playoff(int downToSize, List<Player> qualified, string type = "A") : this()
        {
            Type = type;
            Players = qualified;
            int playAs = downToSize * 2;
            List<Player?> empties = [];
            var diff = playAs - qualified.Count;
            for (int i = 0; i < diff; i++)
            {
                empties.Add(null);
            }

            List<Player?> addedEmpties = [.. qualified, .. empties];

            GenerateMatches(addedEmpties);
        }

        private void GenerateMatches(List<Player?> players)
        {
            for (int i = 0; i < players.Count / 2; i++)
            {
                var a = players[i];
                var b = players[^(i + 1)] ?? new Player();

                var match = new KnockoutMatch(i, players.Count, a!, b!, Guid.NewGuid().GetHashCode());

                if (match.B.Name == string.Empty)
                    match.AVundet = true;

                Rounds.Add(match);
            }
        }

        public List<Player> SortPlayersResult()
        {
            var result = new List<Player>();
            foreach (var match in Rounds)
            {
                var winner = match.AVundet ? match.A : match.B;

                result.Add(winner);
            }
            return result;
        }
        public override string ToString() => $"{Type}-Playoff";

        public void ClearAll()
        {
            foreach (var round in Rounds)
            {
                round.Clear();
            }
        }
    }
}
