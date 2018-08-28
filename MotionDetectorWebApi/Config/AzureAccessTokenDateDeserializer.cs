using System;
using Newtonsoft.Json;

namespace MotionDetectorWebApi.Config
{
    public class AzureAccessTokenDateDeserializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(DateTime) && !double.TryParse(reader.Value.ToString(), out double d))
                return reader.Value;

            var dateString = (string)reader.Value;
            var milliseconds = Convert.ToDouble(dateString) * 1000;
            return new DateTime(1970, 1, 1).Add(TimeSpan.FromMilliseconds(milliseconds)).ToLocalTime();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override bool CanWrite { get; } = false;
    }
}
