using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace M8ScoreTool.Extensions;

public static class SessionExtensions
{
    public static void SetObject<T>(this ISession session, string key, T value)
    {
        string json = JsonSerializer.Serialize(value);
        session.SetString(key, json);
    }

    public static T GetObject<T>(this ISession session, string key)
    {
        string json = session.GetString(key);
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json);
    }
}
