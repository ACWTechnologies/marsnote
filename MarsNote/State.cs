using System.ComponentModel;
using Newtonsoft.Json;

namespace MarsNote
{
    public sealed class State
    {
        [JsonConstructor]
        public State(string profile, string folder)
        {
            Profile = profile;
            Folder = folder;
        }

        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 2)]
        public string Folder { get; set; }

        [DefaultValue(null)]
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Populate, Order = 1)]
        public string Profile { get; set; }

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
