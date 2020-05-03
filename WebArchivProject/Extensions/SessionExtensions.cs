using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace WebArchivProject.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObj<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetObj<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
