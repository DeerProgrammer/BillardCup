﻿namespace JsonFileDatabase.Model
{
    public class Knockout() : IRound
    {
        public int Id { get; set; } = Guid.NewGuid().GetHashCode();

        public List<Player> Players { get; set; } = [];
        public List<KnockoutMatch> Rounds { get; set; } = [];
        public bool IsFinale { get; set; } = false;
        public string Type { get; set; } = "";

        public Knockout(List<Player> qualified, string type = "A", bool isFinale = false) : this()
        {
            IsFinale = isFinale;
            Players = [.. qualified];
            Type = type;
            GenerateMatches();
        }

        private void GenerateMatches()
        {
            for (int i = 0; i < Players.Count / 2; i++)
            {
                var a = Players[i];
                var b = Players[^(i + 1)] ?? new Player();

                var match = new KnockoutMatch(i, Players.Count, a!, b!, Guid.NewGuid().GetHashCode());

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

        public override string ToString()
            => Players.Count switch
            {
                2 => $"{Type}-Finale",
                4 => IsFinale ? $"{Type}-Finale" : $"{Type}-Semifinale",
                8 => $"{Type}-Kvartfinale",
                _ => $"{Type}-Sidste {Players.Count}"
            };

        public void ClearAll()
        {
            foreach (var round in Rounds)
            {
                round.Clear();
            }
        }
    }
}
