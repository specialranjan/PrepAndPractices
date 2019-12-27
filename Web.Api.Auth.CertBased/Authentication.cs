using System;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace Web.Api.Auth.CertBased
{
    public class Authentication
    {
        public bool IsAuthorized()
        {
            var certificates = this.GetCertificatesByThumbprint("");
            var certificate = certificates[0];
            var validator = X509CertificateValidator.ChainTrust;
            validator.Validate(certificate);
            return true;
        }

        private X509Certificate2Collection GetCertificatesByThumbprint(string thumbprint)
        {
            var localCertificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            localCertificateStore.Open(OpenFlags.ReadOnly);
            var certificateCollection = localCertificateStore.Certificates.Find(
                X509FindType.FindByThumbprint,
                thumbprint,
                false);

            if (certificateCollection.Count == 0)
            {
                var personalCertificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                personalCertificateStore.Open(OpenFlags.ReadOnly);
                certificateCollection = personalCertificateStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    thumbprint,
                    false);
            }

            return certificateCollection;
        }
    }
}