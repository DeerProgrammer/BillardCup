namespace JsonFileDatabase.Model
{
    public interface IRound
    {
        int Id { get; }
        List<Player> Players { get; }

        void ClearAll();
        List<Player> SortPlayersResult();
    }
}