using Microsoft.Extensions.Configuration;

namespace JBM.DeserializeJson
{
    public static class JsonToCSharp
    {
        public static TObj ToCSharp<TObj>(this IConfiguration configuration, string jsonSection) where TObj : class, new()
        {
            TObj obj = new();

            configuration.GetSection(jsonSection).Bind(obj);

            return obj;
        }
    }
}