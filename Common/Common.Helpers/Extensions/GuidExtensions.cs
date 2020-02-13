namespace Common.Helpers.Extensions
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The Guid Extensions
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Converts string to Guid
        /// </summary>
        /// <param name="value">The string value</param>
        /// <returns>The Guid value</returns>
        public static Guid StringToGuid(this string value)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            return new Guid(data);
        }
    }
}
