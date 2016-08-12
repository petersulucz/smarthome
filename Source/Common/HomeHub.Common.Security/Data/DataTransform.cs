using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeHub.Common.Security.Data
{
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Xml;
    using System.Xml.Linq;
    using System.Security.Cryptography.X509Certificates;

    using HomeHub.Common.Security.Configuration;

    /// <summary>
    /// The data transform.
    /// </summary>
    public static class DataTransform
    {
        public static string Encrypt(string payload)
        {
            var key = SecurityConfiguration.Configuration.CertificateFingerprint;
            return DataTransform.Encrypt(key, payload);
        }

        internal static string Encrypt(string fingerprint, string payload)
        {
            using (var store = new CertificateStore(fingerprint))
            {
                var cert = store.Certificate;
                var pk = cert.GetRSAPublicKey();
                
                using (var aes = new AesManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    using (var transform = aes.CreateEncryptor())
                    {
                        var keyBin = pk.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA512);
                        var keyEncrypted = Convert.ToBase64String(keyBin);
                        var initializationVector = Convert.ToBase64String(aes.IV);

                        using (var memStream = new MemoryStream())
                        {
                            using (var crypto = new CryptoStream(memStream, transform, CryptoStreamMode.Write))
                            {
                                var bytes = Encoding.Unicode.GetBytes(payload);
                                crypto.Write(bytes, 0, bytes.Length);
                            }

                            var encrypted = Convert.ToBase64String(memStream.ToArray());

                            return
                                new XDocument(
                                    new XElement(
                                        "root",
                                        new XElement(
                                            "Meta",
                                            new XElement("FingerPrint", fingerprint),
                                            new XElement("Key", keyEncrypted),
                                            new XElement("Iv", initializationVector)),
                                        new XElement("Payload", encrypted))).ToString();
                        }
                    }
                }
            }
        }

        public static string Decrypt(string payload)
        {
            var doc = XDocument.Parse(payload);
            var meta = doc.Root.Element("Meta");
            var fingerprint = meta.Element("FingerPrint").Value;
            var enckey = Convert.FromBase64String(meta.Element("Key").Value);
            var iv = Convert.FromBase64String(meta.Element("Iv").Value);
            var encrypted = doc.Root.Element("Payload").Value;

            using (var store = new CertificateStore(fingerprint))
            {
                var cert = store.Certificate;
                var pk = cert.GetRSAPrivateKey();
                using (var aes = new AesManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;

                    var key = pk.Decrypt(enckey, RSAEncryptionPadding.OaepSHA512);

                    using (var transform = aes.CreateDecryptor(key, iv))
                    {
                        using (var memStream = new MemoryStream(Convert.FromBase64String(encrypted)))
                        {
                            using (var crypto = new CryptoStream(memStream, transform, CryptoStreamMode.Read))
                            {
                                using (var reader = new StreamReader(crypto, Encoding.Unicode))
                                {
                                    return reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
