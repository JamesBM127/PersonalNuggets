using Microsoft.Extensions.Configuration;

namespace DeserializeJson
{
    public static class JsonToObject
    {
        public static TObj ToObject<TObj>(this IConfiguration configuration, string jsonSection) where TObj : class, new()
        {
            TObj obj = new();

            configuration.GetSection(jsonSection).Bind(obj);

            return obj;
        }
    }
}