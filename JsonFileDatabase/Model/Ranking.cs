namespace JsonFileDatabase.Model
{
    public class Ranking(Cup c, Knockout finale) : IRound
    {
        public int Id { get; private set; } = Guid.NewGuid().GetHashCode();

        public List<Player> Players { get; set; } = [];
        public Knockout Finale { get; set; } = finale;
        public Cup Cup { get; set; } = c;

        public List<Player> SortPlayersResult()
        {
            Players = [];
            SortByFinale();
            SortByKnockout();
            SortByGroup();

            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].FinalPlacement = i + 1;
            }

            return Players;
        }

        private void SortByFinale()
        {
            var finale = Finale.Rounds[0];
            var bronze = Finale.Rounds[1];

            //sort finale
            Players.Add(finale.AVundet ? finale.A : finale.B);
            Players.Add(finale.AVundet ? finale.B : finale.A);

            //sort bronze
            Players.Add(bronze.AVundet ? bronze.A : bronze.B);
            Players.Add(bronze.AVundet ? bronze.B : bronze.A);
        }

        private void SortByKnockout()
        {
            var noneFinalists = Cup.Finales[0].Players.Except(Players).ToList();

            var ranked = noneFinalists.OrderByDescending(x => x.FinalWins)
                                      .ThenByDescending(x => x.TotalAveragePercent)
                                      .ThenByDescending(x => x.OrderedTotalSerie[0])
                                      .ThenByDescending(x => x.OrderedTotalSerie[1])
                                      .ToList();

            Players.AddRange(ranked);
        }

        private void SortByGroup()
        {
            var noneKnockouters = Cup.Players.Except(Players).ToList();

            var ranked = noneKnockouters.OrderBy(x => x.GroupPlacement)
                                        .ThenByDescending(x => x.TotalAveragePercent)
                                        .ThenByDescending(x => x.OrderedTotalSerie[0])
                                        .ThenByDescending(x => x.OrderedTotalSerie[1])
                                        .ToList();
            Players.AddRange(ranked);
        }

        public override string ToString() => "Resultat";

        public void ClearAll()
        {
            //Do nothing;
        }
    }
}