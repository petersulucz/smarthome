// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecurityConfiguration.cs" company="">
//   
// </copyright>
// <summary>
//   The security configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HomeHub.Common.Security.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// The security configuration.
    /// </summary>
    public class SecurityConfiguration : ConfigurationSection
    {
        private const string ConfigurationSectionName = "SecurityConfiguration";

        private const string CertificateFingerprintName = "CertificateFingerprint";

        public static SecurityConfiguration Configuration
            => (SecurityConfiguration)ConfigurationManager.GetSection(SecurityConfiguration.ConfigurationSectionName);

        [ConfigurationProperty(SecurityConfiguration.CertificateFingerprintName, IsRequired = true)]
        public string CertificateFingerprint => (string)this[SecurityConfiguration.CertificateFingerprintName];
    }
}
