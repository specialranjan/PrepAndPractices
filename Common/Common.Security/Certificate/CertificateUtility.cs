using System;
using System.Security.Cryptography.X509Certificates;
using Common.Security.Exceptions;

namespace Common.Security.Certificate
{
    public static class CertificateUtility
    {
        /// <summary>
        /// Gets the certificate from thumbprint.
        /// </summary>
        /// <param name="name">The certificate store name</param>
        /// <param name="location">The certificate store location</param>
        /// <param name="thumbprint">The certificate thumbprint you search for</param>
        /// <returns></returns>
        public static X509Certificate2 GetCertificateByThumbprint(StoreName name, StoreLocation location, string thumbprint)
        {
            var store = new X509Store(name, location);
            X509Certificate2Collection certificates = null;
            store.Open(OpenFlags.ReadOnly);
            if (thumbprint == null)
            {
                throw new ArgumentNullException(nameof(thumbprint));
            }

            try
            {
                X509Certificate2 result = null;

                certificates = store.Certificates;

                for (int i = 0; i < certificates.Count; i++)
                {
                    var cert = certificates[i];

                    if (cert.Thumbprint.Equals(thumbprint.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (result != null)
                        {
                            throw new CertificateDataException(string.Format("There are multiple certificates for thumbprint {0}", thumbprint));
                        }

                        result = new X509Certificate2(cert);
                    }
                }

                if (result == null)
                {
                    throw new CertificateDataException(string.Format("No certificate was found for thumbprint {0}", thumbprint));
                }

                return result;
            }
            finally
            {
                if (certificates != null)
                {
                    for (int i = 0; i < certificates.Count; i++)
                    {
                        var cert = certificates[i];
                        cert.Reset();
                    }
                }
                store.Close();
            }
        }
    }
}
