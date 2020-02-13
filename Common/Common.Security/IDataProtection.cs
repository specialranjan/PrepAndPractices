namespace Common.Security
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// The DataProtection interface
    /// </summary>
    public interface IDataProtection
    {
        /// <summary>
        /// Protects the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>byte array</returns>
        byte[] Protect(Stream stream, DataProtectionScope scope);

        /// <summary>
        /// Uns the protect.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>byte array</returns>
        byte[] UnProtect(Stream stream, DataProtectionScope scope);

        /// <summary>
        /// Protects the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>byte array</returns>
        byte[] Protect(byte[] data, DataProtectionScope scope);

        /// <summary>
        /// Uns the protect.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>byte array</returns>
        byte[] UnProtect(byte[] data, DataProtectionScope scope);

        /// <summary>
        /// Protects the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="certificate">The certificate.</param>
        /// <returns>the string value</returns>
        string Protect(string plainText, X509Certificate2 certificate);

        /// <summary>
        /// Uns the protect.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <param name="certificate">The certificate.</param>
        /// <returns>the string value</returns>
        string UnProtect(string encryptedText, X509Certificate2 certificate);
    }
}
