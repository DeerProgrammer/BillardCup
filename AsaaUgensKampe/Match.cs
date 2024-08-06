using System.Text.Json.Serialization;

namespace AsaaUgensKampe
{
    public class Match
    {
        public DateTime? Dato { get; set; }
        public string? Turnering { get; set; }
        public string? Kampnr { get; set; }
        public string? Hjemmehold { get; set; }
        public string? Udehold { get; set; }
        public string? Resultat { get; set; }
        public string? Edited { get; set; }
    }
}
