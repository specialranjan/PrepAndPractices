namespace Common.Helpers
{
    using System;

    /// <summary>
    /// The AzureKeyVaultClient interface.
    /// </summary>
    public interface IAzureKeyVaultClient : IDisposable
    {
        /// <summary>
        /// The get secret async.
        /// </summary>
        /// <param name="secretName">
        /// The secret name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string GetSecret(string secretName);
    }
}
