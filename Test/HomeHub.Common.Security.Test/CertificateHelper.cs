namespace HomeHub.Common.Security.Test
{
    using System;
    using System.Management.Automation;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    ///     The helpers.
    /// </summary>
    internal class CertificateHelper : IDisposable
    {
        private readonly X509Certificate2 cert;

        private const string CertificateName = "HomeHubTestCert";

        /// <summary>
        ///     The store.
        /// </summary>
        private readonly X509Store store;

        private CertificateHelper()
        {
            this.store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            this.store.Open(OpenFlags.MaxAllowed);
            var found = this.store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, $"CN={CertificateHelper.CertificateName}", false);

            if (found.Count == 0)
            {
                CertificateHelper.CreateX509Certificate();
                found = this.store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, $"CN={CertificateHelper.CertificateName}", false);
            }

            if (found.Count != 1)
            {
                throw new Exception("CERTIFICATE FAILURE");
            }

            this.cert = found[0];
        }

        /// <summary>
        /// The invoke with dummy cert.
        /// </summary>
        /// <param name="action">
        /// The action, which takes the cert fingerprint
        /// </param>
        public static void InvokeWithDummyCert(Action<string> action)
        {
            using (var helper = new CertificateHelper())
            {
                var fingerprint = helper.cert.GetCertHashString();
                action(fingerprint);
            }
        }

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            this.store.Remove(this.cert);
            this.store.Close();
        }

        private static X509Certificate2 CreateX509Certificate()
        {
            using (var pshelly = PowerShell.Create())
            {
                pshelly.AddScript(
                    $"New-SelfSignedCertificate -FriendlyName \"{CertificateName}\" -Subject \"CN={CertificateName}\" -CertStoreLocation \'Cert:\\CurrentUser\\My\' -NotAfter (Get-date).AddYears(5)");
                pshelly.Invoke();
            }

            return null;
        }
    }
}