using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Common.Data.AzureStorage.Utils
{
    /// <summary>
    /// The purpose of this class is to validate the Repository methods inputs values
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// Throw ArgumentNullException if the object is null
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="parameterName"></param>
        public static void Null(object parameterValue, string parameterName)
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException(nameof(parameterValue));
            }
        }

        /// <summary>
        /// Throw ArgumentNullException if the Stream object is null
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="parameterName"></param>
        public static void Stream(Stream parameterValue, string parameterName)
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException(parameterName ?? "", "Parameter must not be null");
            }
        }

        /// <summary>
        /// Throw ArgumentNullException if the byte array object is null
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="parameterName"></param>
        public static void ByteArray(byte[] parameterValue, string parameterName)
        {
            if (parameterValue == null)
            {
                throw new ArgumentNullException(parameterName ?? "", "Parameter must not be null");
            }
        }

        /// <summary>
        /// Throw ArgumentException if the string is null or empty
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="parameterName"></param>
        public static void String(string parameterValue, string parameterName)
        {
            Null(parameterValue, parameterName);
            if (parameterValue.Length == 0)
            {
                throw new ArgumentException("Parameter must have length greater than zero.", parameterName ?? "");
            }
        }

        /// <summary>
        /// Throw ArgumentException if the table name is null, empty or not conform to the AzureStorage Table name rules
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="parameterName"></param>
        public static void TableName(string parameterValue, string parameterName)
        {
            Null(parameterValue, parameterName);

            var regex = new Regex("^[A-Za-z][A-Za-z0-9]{2,62}$");
            if (!regex.IsMatch(parameterValue))
            {
                throw new ArgumentException("Table names must conform to these rules: " +
                    "May contain only alphanumeric characters. " +
                    "Cannot begin with a numeric character. " +
                    "Are case-insensitive. " +
                    "Must be from 3 to 63 characters long.", parameterName ?? "");
            }
        }

        /// <summary>
        /// Throw ArgumentException if the table property name is null, empty or not conform to the AzureStorage Table property name rules
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="parameterName"></param>
        public static void TablePropertyValue(string parameterValue, string parameterName)
        {
            Null(parameterValue, parameterName);

            var regex = new Regex(@"^[^/\\#?]{0,1024}$");
            if (!regex.IsMatch(parameterValue))
            {
                throw new ArgumentException("Table property values must conform to these rules: " +
                    "Must not contain the forward slash (/), backslash (\\), number sign (#), or question mark (?) characters. " +
                    "Must be from 1 to 1024 characters long.", parameterName ?? "");
            }
        }

        /// <summary>
        /// Throw ArgumentException if the blob container name is null, empty or not conform to the AzureStorage Blob container name rules
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="parameterName"></param>
        public static void BlobContainerName(string parameterValue, string parameterName)
        {
            Null(parameterValue, parameterName);

            var regex = new Regex("^(?-i)(?:[a-z0-9]|(?<=[0-9a-z])-(?=[0-9a-z])){3,63}$");
            if (!regex.IsMatch(parameterValue))
            {
                throw new ArgumentException("Blob container names must conform to these rules: " +
                    "Must start with a letter or number, and can contain only letters, numbers, and the dash (-) character. " +
                    "Every dash (-) character must be immediately preceded and followed by a letter or number; consecutive dashes are not permitted in container names. " +
                    "All letters in a container name must be lowercase. " +
                    "Must be from 3 to 63 characters long.", parameterName ?? "");
            }
        }

        /// <summary>
        /// Throw ArgumentException if the blob name is null, empty or not conform to the AzureStorage blob name rules
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="parameterName"></param>
        public static void BlobName(string parameterValue, string parameterName)
        {
            String(parameterValue, parameterName);
            if (parameterValue == null)
            {
                throw new ArgumentNullException(nameof(parameterValue));
            }

            const int ParameterLengthCheck = 1024;
            if (parameterValue.Length > ParameterLengthCheck)
            {
                throw new ArgumentException("Blob names must conform to these rules: " +
                    "Must be from 1 to 1024 characters long.", parameterName ?? "");
            }
        }

        /// <summary>
        /// Throw ArgumentException if the Queue name is null, empty or not conform to the AzureStorage Queue name rules
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="parameterName"></param>
        public static void QueueName(string parameterValue, string parameterName)
        {
            Null(parameterValue, parameterName);

            var regex = new Regex("^(?-i)(?:[a-z0-9]|(?<=[0-9a-z])-(?=[0-9a-z])){3,63}$");
            if (!regex.IsMatch(parameterValue))
            {
                throw new ArgumentException("Queue names must conform to these rules: " +
                    "Must start with a letter or number, and can contain only letters, numbers, and the dash (-) character. " +
                    "The first and last letters in the queue name must be alphanumeric. The dash (-) character cannot be the first or last character. Consecutive dash characters are not permitted in the queue name. " +
                    "All letters in a queue name must be lowercase. " +
                    "Must be from 3 to 63 characters long.", parameterName ?? "");
            }
        }

        /// <summary>
        /// The URI.
        /// </summary>
        /// <param name="parameterValue">
        /// The parameter value.
        /// </param>
        /// <param name="parameterName">
        /// The parameter name.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The Argument Exception.
        /// </exception>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        public static Uri ValidateUri(string parameterValue, string parameterName)
        {
            Null(parameterValue, parameterName);
            Uri result;
            if (!Uri.TryCreate(parameterValue, UriKind.Absolute, out result))
            {
                throw new ArgumentException("In appropriate string for URI", parameterName ?? "");
            }

            return result;
        }
    }
}
