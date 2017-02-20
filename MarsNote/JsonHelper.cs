using Newtonsoft.Json;
using System;
using System.IO;

namespace MarsNote
{
    public static class JsonHelper
    {
        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T DeserializeString<T>(string json, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static T DeserializePath<T>(string jsonPath, JsonSerializerSettings settings = null)
        {
            if (!File.Exists(jsonPath)) { throw new ArgumentException("Json path does not exist.", nameof(jsonPath)); }
            return DeserializeString<T>(File.ReadAllText(jsonPath), settings);
        }

        /// <summary>
        /// Perform a deep clone of the object, using Json as a serialisation method.
        /// </summary>
        /// <typeparam name="T">The type of object being cloned.</typeparam>
        /// <param name="source">The object instance to clone.</param>
        /// <returns>The cloned object.</returns>
        public static T DeepClone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default(T);

            // Without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }
    }
}
