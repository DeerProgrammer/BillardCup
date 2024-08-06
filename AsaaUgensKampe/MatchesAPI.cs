using Flurl.Http;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AsaaUgensKampe
{
    public class MatchesAPI
    {
        private const string url = "https://asaabillard.dk/api/scraper/getallmatches";

        public async Task<List<Match>> GetMatches()
        { 
            var json = await url.GetStringAsync();

            var matches = JsonSerializer.Deserialize<List<Match>>(json);

            return [];
        }

        public async Task<List<Match>> GetMatchesForWeekOfDateTime(DateTime dateTime)
        {
            var week = GetWeek(dateTime);
            var matches = await GetMatches();

            return [.. matches.Where(x => x.Dato.Between(week.Start, week.End))];
        }

        private (DateTime Start, DateTime End) GetWeek(DateTime dateTime)
        {
            DateTime firstday = dateTime.AddDays(-(int)dateTime.DayOfWeek);
            DateTime endday = firstday.AddDays(6);

            return (firstday.AddDays(1), endday.AddDays(1));
        }
    }
    public static class Extensions
    {
        public static bool Between(this DateTime? dt, DateTime start, DateTime end)
            => dt > start && dt < end;
    }
}
