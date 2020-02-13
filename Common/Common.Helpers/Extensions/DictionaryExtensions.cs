namespace Common.Helpers.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the value of dictionary corresponding to class T
        /// </summary>
        /// <typeparam name="T">The class T</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key</param>
        /// <returns>The object</returns>
        public static T GetValue<T>(this Dictionary<string, object> dictionary, string key)
        {
            if (dictionary == null || !dictionary.ContainsKey(key))
            {
                return default(T);
            }

            if (!(dictionary[key] is T))
            {
                throw new InvalidCastException();
            }

            return (T)dictionary[key];
        }

        /// <summary>
        /// Returns if the type is Default type of the T struct
        /// </summary>
        /// <typeparam name="T">The struct</typeparam>
        /// <param name="value">The value</param>
        /// <returns>true or false</returns>
        public static bool IsDefault<T>(this T value) where T : struct
        {
            var isDefault = value.Equals(default(T));

            return isDefault;
        }
    }
}
