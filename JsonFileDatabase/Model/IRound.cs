namespace JsonFileDatabase.Model
{
    public interface IRound
    {
        int Id { get; }
        string Type { get; }
        List<Player> Players { get; }

        void ClearAll();
        List<Player> SortPlayersResult();
    }
}