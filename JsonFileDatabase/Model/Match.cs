
namespace JsonFileDatabase.Model
{
    public class Match(Player a, Player b, int id)
    {
        public int Id => id;

        public Player A { get; set; } = a;
        public Player B { get; set; } = b;

        public int ScoreA { get; set; }
        public int ScoreB { get; set; }
        public int Innings { get; set; }
        public int MpA { get; set; }
        public int MpB { get; set; }
        public int SerieA { get; set; }
        public int SerieB { get; set; }

        public virtual void RegisterMatch()
        {
            MpA = A.Distance == ScoreA ? 2 : 0;
            MpB = B.Distance == ScoreB ? 2 : 0;
            if (MpB == MpA) MpA = MpB = 1;

            RegisterForPlayers();
        }

        private void RegisterForPlayers()
        {
            A.GroupPoint += ScoreA;
            A.GroupMP += MpA;
            A.GroupInnings += Innings;
            A.GroupSerie.Add(SerieA);
            A.TotalSerie.Add(SerieA);

            B.GroupPoint += ScoreB;
            B.GroupMP += MpB;
            B.GroupInnings += Innings;
            B.GroupSerie.Add(SerieB);
            B.TotalSerie.Add(SerieB);
        }

        public override string ToString() => $"{Id}";

        public void Clear()
        {
            A.GroupPoint -= ScoreA;
            A.GroupMP -= MpA;
            A.GroupInnings -= Innings;
            A.GroupSerie.Remove(SerieA);
            A.TotalSerie.Remove(SerieA);

            B.GroupPoint -= ScoreB;
            B.GroupMP -= MpB;
            B.GroupInnings -= Innings;
            B.GroupSerie.Remove(SerieB);
            B.TotalSerie.Remove(SerieB);


            ScoreA = 0;
            ScoreB = 0;
            SerieA = 0;
            SerieB = 0;
            Innings = 0;
            MpA = 0;
            MpB = 0;
        }
    }

    public class KnockoutMatch(int _seed, int round, Player a, Player b, int id) : Match(a, b, id)
    {
        public int BSeed => round - _seed;
        public int ASeed => _seed + 1;
        public bool AVundet { get; set; } = false;
        public bool BVundet { get; set; } = false;
        public override void RegisterMatch()
        {
            A.FinalPoint += ScoreA;
            A.FinalInnings += Innings;
            if (AVundet)
                A.FinalWins++;
            if (BVundet)
                B.FinalWins++;
            B.FinalPoint += ScoreB;
            B.FinalInnings += Innings;
        }
    }
}
