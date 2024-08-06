using System.Globalization;
using System.Text;

namespace AsaaUgensKampe
{
    public class FacebookWriter
    {
        public string WriteText(string sponsor, DateTime ugeDato, List<Match> matches)
        {
            var bane = matches.FirstOrDefault()?.Hjemmehold?.Contains("Asaa BK") ?? false ? "hjemme" : "ude";
            var dfI = DateTimeFormatInfo.CurrentInfo;
            var ugeNr = dfI.Calendar.GetWeekOfYear(ugeDato, dfI.CalendarWeekRule, dfI.FirstDayOfWeek);
            var textWriter = new StringBuilder($"""
                Ugens {bane}bane sponsor {sponsor} præsenterer følgende kampe i Sparekassen Danmark arena Asaa BillardKlub i uge {ugeNr}
                
                """);

            if (bane == "hjemme")
            {
                textWriter.AppendLine("Alle kampe kan ses direkte fra https://asaabillard.dk/live");
                textWriter.AppendLine();
            }
            textWriter.AppendLine();

            var matchesPerDay = OrderMatchesByDay(matches);

            foreach (var matchday in matchesPerDay)
            {
                textWriter.AppendLine($"{matchday.Key.ToString()}:");
                foreach (var match in matchesPerDay[matchday.Key])
                {
                    var kl = StartTimeForDay(matchday.Key);
                    if(match.Turnering == "Keglebillard - Søgaard Ligaen")
                    {
                        kl = matchday.Key is DayOfWeek.Sunday ? "10.00" : "13.00";
                    }
                    textWriter.AppendLine($"{match.Hjemmehold} mod {match.Udehold} kl. {kl}");
                }
            }

            if (bane == "ude")
            {
                textWriter.AppendLine($"""

                    HUSK: hvis en kamp er flyttet at give besked til Jørgen Nielsen eller Niels Thy så vi kan få rettet det inden næste uges opslag.
                    """);
            }

            return textWriter.ToString();
        }

        private string StartTimeForDay(DayOfWeek key)
            => key switch
            {
                DayOfWeek.Sunday or
                DayOfWeek.Saturday => "11.00",
                DayOfWeek.Monday or
                DayOfWeek.Tuesday or
                DayOfWeek.Wednesday or
                DayOfWeek.Thursday or
                DayOfWeek.Friday => "19.00",
                _ => ""
            };

        private static Dictionary<DayOfWeek, List<Match>> OrderMatchesByDay(List<Match> matches)
        {
            var matchesPerDay = new Dictionary<DayOfWeek, List<Match>>();
            foreach (var match in matches)
            {
                if (match.Dato is null) continue;

                matchesPerDay.TryAdd(match.Dato.Value.DayOfWeek, []);

                matchesPerDay[match.Dato.Value.DayOfWeek].Add(match);
            }
            return matchesPerDay;
        }
    }
}
