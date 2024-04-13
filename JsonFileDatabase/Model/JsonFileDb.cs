using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonFileDatabase.Model
{
    public class JsonFileDb
    {
        private static JsonFileDb? instance;
        public static JsonFileDb Instance
        {
            get 
            {
                instance ??= new();
                return instance;
            }
        }

        private readonly JsonSerializer _serializer;

        private JsonFileDb()
        {
            _serializer = new();
            _serializer.Converters.Add(new JavaScriptDateTimeConverter());
            _serializer.NullValueHandling = NullValueHandling.Ignore;
            _serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            _serializer.Formatting = Formatting.Indented;
            _serializer.PreserveReferencesHandling = PreserveReferencesHandling.All;
            _serializer.TypeNameHandling = TypeNameHandling.All;
        }

        public Cup FindCup(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException();

            using StreamReader sr = new(path);
            using JsonTextReader reader = new(sr);

            var cup = _serializer.Deserialize<Cup>(reader);

            return cup ?? throw new NullReferenceException();
        }

        public void SaveCup(Cup cup, string path)
        {
            using StreamWriter sw = new(path);
            using JsonTextWriter writer = new(sw);

            _serializer.Serialize(writer, cup);
        }
    }
}