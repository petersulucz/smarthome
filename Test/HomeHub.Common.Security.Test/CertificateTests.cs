namespace HomeHub.Common.Security.Test
{
    using HomeHub.Common.Security.Data;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CertificateTests
    {
        [TestMethod]
        public void TestCreateCert()
        {
            CertificateHelper.InvokeWithDummyCert(
                (fingerprint) =>
                    {
                        using (var store = new CertificateStore(fingerprint))
                        {
                            Assert.IsNotNull(store.FingerPrint);
                            Assert.AreEqual(fingerprint, store.FingerPrint);
                        }
                    });
        }

        [TestMethod]
        public void TestRoundTrip()
        {
            const string TestString = "TEST STRING";
            CertificateHelper.InvokeWithDummyCert(
                (fingerprint) =>
                    {
                        var encrypted = DataTransform.Encrypt(fingerprint, TestString);
                        var decrypted = DataTransform.Decrypt(encrypted);

                        Assert.AreEqual(decrypted, TestString);
                    });
        }

        [TestMethod]
        public void TestRoundTrip2()
        {
            const string TestString =
                "asdfasdfas;dlffkjpowQYB93YRY7982UO1HQW AHAUI FACPWUH F CAHGUIWEA RY93QRCHWEFCS;DHFVOAWGOSE7R HAE";
            CertificateHelper.InvokeWithDummyCert(
                (fingerprint) =>
                {
                    var encrypted = DataTransform.Encrypt(fingerprint, TestString);
                    var decrypted = DataTransform.Decrypt(encrypted);

                    Assert.AreEqual(decrypted, TestString);
                });
        }

        [TestMethod]
        public void TestRoundTripeEmpty()
        {
            const string TestString = "";
            CertificateHelper.InvokeWithDummyCert(
                (fingerprint) =>
                {
                    var encrypted = DataTransform.Encrypt(fingerprint, TestString);
                    var decrypted = DataTransform.Decrypt(encrypted);

                    Assert.AreEqual(decrypted, TestString);
                });
        }

    }
}
