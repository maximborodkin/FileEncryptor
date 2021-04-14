using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileEncryptor.Extensions
{
    public static class SessionSerializationExtension
    {
        public static void SetSerializable(this ISession session, string key, object obj)
        {
            string json = JsonSerializer.Serialize(obj);
            session.Set(key, Encoding.UTF8.GetBytes(json));
        }

        public static T GetSerializable<T>(this ISession session, string key)
        {
            try
            {
                byte[] bytes = session.Get(key);
                string json = Encoding.UTF8.GetString(bytes);
                return JsonSerializer.Deserialize<T>(json);
            } catch(Exception)
            {
                return default(T);
            }
        }
    }
}
