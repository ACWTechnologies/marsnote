using System;
using System.IO;
using System.Windows.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MarsNote
{
    public static class JsonHelper
    {
        /// <summary>
        /// Perform a deep clone of the object, using Json as a serialisation method.
        /// </summary>
        /// <typeparam name="T">The type of object being cloned.</typeparam>
        /// <param name="source">The object instance to clone.</param>
        /// <returns>The cloned object.</returns>
        public static T DeepClone<T>(T source)
        {
            if (ReferenceEquals(source, null))
            {
                return default(T);
            }

            // Without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }

        public static T DeserializePath<T>(string jsonPath, JsonSerializerSettings settings = null)
        {
            if (!File.Exists(jsonPath)) { throw new ArgumentException("Json path does not exist.", nameof(jsonPath)); }
            return DeserializeString<T>(File.ReadAllText(jsonPath), settings);
        }

        public static T DeserializeString<T>(string json, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }

    /// <summary>
    /// Converts a <see cref="System.Windows.Media.Brush"/> to and from its RGB hex string value in the format #RRGGBB.
    /// Uses #00FFFFFF if the brush is fully transparent.
    /// </summary>
    public sealed class RGBHexBrushAllowFullyTransparentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Brush).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            if (reader.TokenType != JsonToken.String) { throw new JsonSerializationException("RGB must be of type String."); }
            var input = (string)reader.Value;

            if (input.StartsWith("#"))
            {
                input = input.Substring(1);
            }

            var transparent = false;

            if (input.Length == 3)
            {
                // #123 --> #112233
                char[] sixDigit = {input[0], input[0], input[1], input[1], input[2], input[2]};
                input = new string(sixDigit);
            }
            else if (input.Length != 6)
            {
                if (input.Length == 8 && input[0] == '0' && input[1] == '0')
                {
                    // #00XXXXXX
                    transparent = true;
                }
                else
                {
                    throw new JsonSerializationException("RGB not valid length.");
                }
            }

            Brush brush;
            try
            {
                brush = new SolidColorBrush(
                    transparent
                    ? Color.FromArgb(0, 255, 255, 255)
                    : Color.FromRgb(
                        byte.Parse(input.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(input.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                        byte.Parse(input.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)
                    )
                );
            }
            catch (FormatException fe)
            {
                throw new JsonSerializationException("RGB not valid hexadecimal.", fe);
            }

            return brush;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var brush = value as Brush;
            if (brush != null)
            {
                Color color = ((SolidColorBrush) brush).Color;
                string hex = color.A == 0
                    ? "#00FFFFFF"
                    : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                new JValue(hex).WriteTo(writer);
            }
            else
            {
                serializer.Serialize(writer, null);
            }
        }
    }
}
