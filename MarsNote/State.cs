using Newtonsoft.Json;

namespace MarsNote
{
    public class State
    {
        public string Profile { get; set; }
        public string Folder { get; set; }

        [JsonConstructor]
        public State(string profile, string folder)
        {
            Profile = profile;
            Folder = folder;
        }

        public static State Load()
        {
            try
            {
                return JsonHelper.DeserializePath<State>(FileHelper.StateFileLocation,
                    new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        NullValueHandling = NullValueHandling.Include
                    });
            }
            catch
            {
                return new State(null, null);
            }
        }

        public static void Save(string profile, string folder)
        {
            var s = new State(profile, folder);
            string json = JsonHelper.Serialize(s);

            FileHelper.Write(json, FileHelper.StateFileLocation);
        }
    }
}
