namespace Common.Helpers
{
    using System;
    using System.Runtime.Caching;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Common.Caching;
    using Microsoft.Azure.KeyVault;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// The azure key vault client.
    /// </summary>
    public class AzureKeyVaultClient : IAzureKeyVaultClient
    {
        #region Fields

        /// <summary>
        /// The vault url.
        /// </summary>
        private readonly string vaultUrl;

        /// <summary>
        /// The client id.
        /// </summary>
        private readonly string clientId;

        /// <summary>
        /// The client secret.
        /// </summary>
        private readonly string clientSecret;

        /// <summary>
        /// The client certificate.
        /// </summary>
        private readonly ClientAssertionCertificate clientCertificate;

        /// <summary>
        /// The key vault client.
        /// </summary>
        private readonly KeyVaultClient keyVaultClient;

        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly InMemoryCacheManager cacheManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureKeyVaultClient"/> class.
        /// </summary>
        /// <param name="vaultUrl">
        /// The vault url.
        /// </param>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <param name="certificate">
        /// The client certificate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The parameters can not be empty
        /// </exception>
        public AzureKeyVaultClient(string vaultUrl, string clientId, X509Certificate2 certificate)
        {
            if (string.IsNullOrEmpty(vaultUrl))
            {
                throw new ArgumentNullException(nameof(vaultUrl));
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            this.vaultUrl = vaultUrl;
            this.clientId = clientId;
            this.clientCertificate = new ClientAssertionCertificate(this.clientId, certificate);
            this.cacheManager = new InMemoryCacheManager();

            this.keyVaultClient = new KeyVaultClient(this.GetTokenByCertificateAsync);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureKeyVaultClient"/> class.
        /// </summary>
        /// <param name="vaultUrl">
        /// The vault url.
        /// </param>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <param name="clientSecret">
        /// The client secret.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The parameters can not be null or empty.
        /// </exception>
        public AzureKeyVaultClient(string vaultUrl, string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(vaultUrl))
            {
                throw new ArgumentNullException(nameof(vaultUrl));
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (clientSecret == null)
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            this.vaultUrl = vaultUrl;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.cacheManager = new InMemoryCacheManager();

            this.keyVaultClient = new KeyVaultClient(this.GetTokenBySecretAsync);
        }

        #endregion

        // To decide the authentication mode used in instantiating the keyvault client
        public enum KeyVaultAuthenticationMode
        {
            /// <summary>
            /// The secret key.
            /// </summary>
            SecretKey,

            /// <summary>
            /// The certificate.
            /// </summary>
            Certificate
        }

        #region Public methods

        /// <summary>
        /// The get secret async.
        /// </summary>
        /// <param name="secretName">
        /// The secret name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetSecret(string secretName)
        {
            return this.keyVaultClient.GetSecretAsync(this.vaultUrl, secretName).Result.Value;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.cacheManager != null)
            {
                this.cacheManager.Dispose();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The get token by certificate.
        /// </summary>
        /// <param name="authority">
        /// The authority.
        /// </param>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Failed to obtain the token.
        /// </exception>
        private async Task<string> GetTokenByCertificateAsync(string authority, string resource, string scope)
        {
            if (this.cacheManager.Get<string>("Constants.KeyVaultTokenCacheKeyName") == null)
            {
                var authenticationContext = new AuthenticationContext(authority);
                var result = await authenticationContext.AcquireTokenAsync(resource, this.clientCertificate).ConfigureAwait(false);

                if (result == null)
                {
                    throw new InvalidOperationException("Failed to obtain the token.");
                }

                this.SaveAccessToken(result);
                return result.AccessToken;
            }

            return this.GetAccessToken();
        }

        /// <summary>
        /// The get token by secret.
        /// </summary>
        /// <param name="authority">
        /// The authority.
        /// </param>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <param name="scope">
        /// The scope.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Failed to obtain the token.
        /// </exception>
        private async Task<string> GetTokenBySecretAsync(string authority, string resource, string scope)
        {
            if (this.cacheManager.Get<string>("Constants.KeyVaultTokenCacheKeyName") == null)
            {
                var credential = new ClientCredential(this.clientId, this.clientSecret);
                var authenticationContext = new AuthenticationContext(authority);
                var result = await authenticationContext.AcquireTokenAsync(resource, credential).ConfigureAwait(false);

                if (result == null)
                {
                    throw new InvalidOperationException("Failed to obtain the token.");
                }

                this.SaveAccessToken(result);
                return result.AccessToken;
            }

            return this.GetAccessToken();
        }

        /// <summary>
        /// The get access token.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetAccessToken()
        {
            return this.cacheManager.Get<string>("Constants.KeyVaultTokenCacheKeyName");
        }

        /// <summary>
        /// The save access token.
        /// </summary>
        /// <param name="auth">
        /// The authentication result.
        /// </param>
        private void SaveAccessToken(AuthenticationResult auth)
        {
            const double ExpireTime = (double)(-30);
            this.cacheManager.AddOrUpdate(
                            "Constants.KeyVaultTokenCacheKeyName",
                            auth.AccessToken,
                            new CacheItemPolicy() { AbsoluteExpiration = auth.ExpiresOn.AddSeconds(ExpireTime) });
        }

        #endregion
    }
}
