using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Common.Security.Exceptions;

namespace Common.Security.Certificate
{
    public sealed class Cryptor : IDataProtection
    {
        public Cryptor()
        {
        }

        public byte[] Protect(Stream stream, DataProtectionScope scope)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            stream.Position = 0;
            var dataBuffer = new byte[stream.Length];
            var byteCount = stream.Read(dataBuffer, 0, (int)stream.Length);
            if (byteCount == 0)
            {
                throw new CertificateDataException("Cannot read data from stream.");
            }

            return this.Protect(dataBuffer, scope);
        }

        public byte[] UnProtect(Stream stream, DataProtectionScope scope)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            stream.Position = 0;
            var dataBuffer = new byte[stream.Length];
            var byteCount = stream.Read(dataBuffer, 0, (int)stream.Length);
            if (byteCount == 0)
            {
                throw new CertificateDataException("Cannot read data from stream.");
            }

            return this.UnProtect(dataBuffer, scope);
        }

        public byte[] Protect(byte[] data, DataProtectionScope scope)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return ProtectedData.Protect(data, null, scope);
        }

        public byte[] UnProtect(byte[] data, DataProtectionScope scope)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return ProtectedData.Unprotect(data, null, scope);
        }

        public string Protect(string plainText, X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (plainText == null)
            {
                throw new ArgumentNullException(nameof(plainText));
            }

            var PlainString = plainText.Trim();
            var cipherbytes = Encoding.UTF8.GetBytes(PlainString);
            var rsa = (RSACryptoServiceProvider)certificate.PublicKey.Key;
            var cipher = rsa.Encrypt(cipherbytes, false);
            return Convert.ToBase64String(cipher);
        }

        public string UnProtect(string atcPushUserName, object dataProtectionScope)
        {
            throw new NotImplementedException();
        }

        public string UnProtect(string encryptedText, X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (certificate.HasPrivateKey)
            {
                var cipherbytes = Convert.FromBase64String(encryptedText);
                var rsa = (RSACryptoServiceProvider)certificate.PrivateKey;
                var plainbytes = rsa.Decrypt(cipherbytes, false);
                var enc = new UTF8Encoding();
                return enc.GetString(plainbytes);
            }
            else
            {
                throw new CertificateDataException("Certificate used for has no private key.");
            }
        }

    }
}
