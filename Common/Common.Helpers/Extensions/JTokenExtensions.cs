using System;
using Newtonsoft.Json.Linq;

namespace Common.Helpers.Extensions
{
    public static class JTokenExtensions
    {
        public static void Rename(this JToken token, string newName)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var parent = token.Parent;
            if (parent == null)
            {
                throw new InvalidOperationException("The parent is missing.");
            }
            var newToken = new JProperty(newName, token);
            parent.Replace(newToken);
        }

        public static void Upsert(this JObject jsonObject, string propertyName, JValue value)
        {
            if (jsonObject == null)
            {
                throw new ArgumentNullException(nameof(jsonObject));
            }

            if (jsonObject.Property(propertyName) == null)
            {
                jsonObject.Add(propertyName, value);
            }
            else
            {
                jsonObject[propertyName] = value;
            }
        }

        public static void Upsert(this JObject jsonObject, JProperty prop)
        {
            if (jsonObject == null)
            {
                throw new ArgumentNullException(nameof(jsonObject));
            }

            if (prop == null)
            {
                throw new ArgumentNullException(nameof(prop));
            }

            if (jsonObject.Property(prop.Name) == null)
            {
                jsonObject.Add(prop);
            }
            else
            {
                jsonObject[prop.Name] = prop.Value;
            }
        }

    }
}
