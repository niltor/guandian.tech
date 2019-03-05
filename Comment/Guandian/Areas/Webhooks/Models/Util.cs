using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace GithubWebhook
{
    public class GithubInconsistentDateTimeTypeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var date = new DateTime();

                if (DateTime.TryParse(reader.Value.ToString(), out date))
                {
                    return date;
                }
                else
                {
                    return reader.Value.ToString();
                }
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                var ticks = (long)reader.Value;

                var date = new DateTime(1970, 1, 1);
                date = date.AddSeconds(ticks);

                return date;
            }
            else
            {
                return reader.Value;
            }
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None
        };
    }
}
