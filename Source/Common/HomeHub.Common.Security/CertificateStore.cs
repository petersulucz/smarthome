// --------------------------------------------------------------------------------------------------------------------
namespace HomeHub.Common.Security
{
    using System;
    using System.Security.Cryptography.X509Certificates;

    using HomeHub.Common.Exceptions;

    /// <summary>
    /// The certificate store.
    /// </summary>
    internal sealed class CertificateStore : IDisposable
    {
        /// <summary>
        /// The certificate store
        /// </summary>
        private readonly X509Store certStore = null;

        public readonly string FingerPrint = null;

        public readonly X509Certificate2 Certificate;

        internal CertificateStore(string fingerprint)
        {
            this.certStore= new X509Store(StoreName.My, StoreLocation.CurrentUser);
            this.certStore.Open(OpenFlags.ReadOnly);
            var certs = this.certStore.Certificates.Find(X509FindType.FindByThumbprint, fingerprint, false);

            if (certs.Count != 1)
            {
                ExceptionUtility.ThrowFailureException($"Certificate load failure.");
            }

            // There can only be one
            this.Certificate = certs[0];
            this.FingerPrint = this.Certificate.GetCertHashString();
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.certStore?.Close();
        }
    }
}
