namespace JsonFileDatabase.Model
{
    public interface IRound
    {
        int Id { get; }
        List<Player> Players { get; }
        List<Player> SortPlayersResult();
    }
}