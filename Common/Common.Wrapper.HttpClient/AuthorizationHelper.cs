namespace Common.Wrapper.HttpClient
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    using Newtonsoft.Json;

    /// <summary>
    /// The JWT token helper.
    /// </summary>
    public static class AuthorizationHelper
    {
        /// <summary>
        /// The expiration time key.
        /// </summary>
        private const string ExpirationTimeKey = "exp";

        /// <summary>
        /// The epoch.
        /// </summary>
        private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The create JWT token.
        /// </summary>
        /// <param name="authenticationKey">
        /// The authentication Key.
        /// </param>
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <param name="minExpiryTimeForTokenInMinutes">
        /// The min Expiry Time For Token In Minutes.
        /// </param>
        /// <param name="issuer">
        /// The issuer.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreateJwtToken(string authenticationKey, IDictionary<string, string> claims, double minExpiryTimeForTokenInMinutes, string issuer)
        {
            if (string.IsNullOrEmpty(authenticationKey))
            {
                throw new ArgumentException("The authentication key is missing");
            }

            const string HeaderValue = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
            var base64Header = Base64UrlEncode(GetBytes(HeaderValue));

            if (claims == null)
            {
                claims = new Dictionary<string, string>();
            }

            AddOrReplaceInDictionary(claims, "iss", issuer);
            AddOrReplaceInDictionary(claims, ExpirationTimeKey, ToUnixTime(DateTime.UtcNow.AddMinutes(minExpiryTimeForTokenInMinutes)).ToString(CultureInfo.InvariantCulture));

            var tokenPayload = JsonConvert.SerializeObject(claims);
            var base64Payload = Base64UrlEncode(GetBytes(tokenPayload));

            var jwt = base64Header + "." + base64Payload;

            var bytesToSign = GetBytes(string.Join(".", base64Header, base64Payload));

            var secret = GetBytes(authenticationKey);

            string computedSignature;
            using (var alg = new HMACSHA256(secret))
            {
                var hash = alg.ComputeHash(bytesToSign);
                computedSignature = Base64UrlEncode(hash);
            }

            return "Bearer " + jwt + "." + computedSignature;
        }

        /// <summary>
        /// The validate JWT token 01.
        /// </summary>
        /// <param name="authenticationKey">
        /// The authentication key.
        /// </param>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <param name="enforceLifetime">
        /// The enforce lifetime.
        /// </param>
        /// <returns>
        /// The <see cref="ClaimsPrincipal"/>.
        /// </returns>
        public static ClaimsPrincipal ValidateJwtToken(string authenticationKey, string token, bool enforceLifetime)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(authenticationKey))
            {
                return null;
            }

            var tokenParts = token.Split('.');
            const int TokenPartsLengthCheck = 3;
            if (tokenParts.Length != TokenPartsLengthCheck)
            {
                return null;
            }

            var bytesToSign = GetBytes(string.Join(".", tokenParts[0], tokenParts[1]));

            var secret = GetBytes(authenticationKey);
            string computedSignature;
            using (var alg = new HMACSHA256(secret))
            {
                var hash = alg.ComputeHash(bytesToSign);
                computedSignature = Base64UrlEncode(hash);
            }

            const int TokenPartsIndexForComputedSignature = 2;
            if (computedSignature != tokenParts[TokenPartsIndexForComputedSignature])
            {
                return null;
            }

            var decodeClaimsPrincipal = new ClaimsPrincipal(DecodeClaimsFromToken(tokenParts[1]));

            if (enforceLifetime && IsTokenExpired(decodeClaimsPrincipal))
            {
                return null;
            }

            return decodeClaimsPrincipal;
        }

        /// <summary>
        /// The is token expired.
        /// </summary>
        /// <param name="decodeClaimsPrincipal">
        /// The decode claims principal.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsTokenExpired(ClaimsPrincipal decodeClaimsPrincipal)
        {
            var expirationTimeClaim = decodeClaimsPrincipal?.Claims?.FirstOrDefault(o => o.Type == ExpirationTimeKey)?.Value;

            if (string.IsNullOrEmpty(expirationTimeClaim))
            {
                return true;
            }

            long expirationDateTime;
            var isExpirationTimeCorrect = long.TryParse(expirationTimeClaim, out expirationDateTime);
            if (!isExpirationTimeCorrect)
            {
                return true;
            }

            return DateTime.UtcNow > FromUnixTime(expirationDateTime);
        }

        /// <summary>
        /// The from UNIX time.
        /// </summary>
        /// <param name="unixTime">
        /// The UNIX time.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        private static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// The to UNIX time.
        /// </summary>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        private static long ToUnixTime(DateTime date)
        {
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        /// <summary>
        /// The decode claims from token.
        /// </summary>
        /// <param name="tokenPayloadBase64Format">
        /// The token payload base 64 format.
        /// </param>
        /// <returns>
        /// The <see cref="ClaimsIdentity"/>.
        /// </returns>
        private static ClaimsIdentity DecodeClaimsFromToken(string tokenPayloadBase64Format)
        {
            var tokenPayload = Encoding.UTF8.GetString(Convert.FromBase64String(Base64UrlDecode(tokenPayloadBase64Format)));
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenPayload);
            if (values == null)
            {
                return null;
            }

            var claims = values.Select(item => new Claim(item.Key, item.Value)).ToList();

            return new ClaimsIdentity(claims);
        }

        /// <summary>
        /// The base 64 URL encode.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        /// <summary>
        /// The base 64 URL decode.
        /// </summary>
        /// <param name="tokenPayloadBase64Format">
        /// The token payload base 64 format.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string Base64UrlDecode(string tokenPayloadBase64Format)
        {
            tokenPayloadBase64Format = tokenPayloadBase64Format.Replace('_', '/'); // 63rd char of encoding
            tokenPayloadBase64Format = tokenPayloadBase64Format.Replace('-', '+'); // 62nd char of encoding
            var mod4 = tokenPayloadBase64Format.Length % 4;
            const int PayloadLengthCheck = 0;
            if (mod4 > PayloadLengthCheck)
            {
                const int Mode4RemainderDiff = 4;
                tokenPayloadBase64Format += new string('=', Mode4RemainderDiff - mod4);
            }

            return tokenPayloadBase64Format;
        }

        /// <summary>
        /// The get bytes.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/>.
        /// </returns>
        private static byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// The add or replace in dictionary.
        /// </summary>
        /// <param name="dic">
        /// The dictionary.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        private static void AddOrReplaceInDictionary(IDictionary<string, string> dic, string key, string value)
        {
            string existingValue;
            var valueExists = dic.TryGetValue(key, out existingValue);
            if (valueExists)
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }
    }
}
