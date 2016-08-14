namespace HomeHub.Administration.Common.Communication
{
    using System.ServiceModel;
    using System.ServiceModel.Configuration;

    public static class BindingFactory
    {
        /// <summary>
        /// The get tcp binding.
        /// </summary>
        /// <returns>
        /// The <see cref="NetTcpBinding"/>.
        /// </returns>
        public static NetTcpBinding GetTcpBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            return binding;
        }

        /// <summary>
        /// The get mex binding.
        /// </summary>
        /// <returns>
        /// The <see cref="NetTcpBinding"/>.
        /// </returns>
        public static BasicHttpBinding GetMexBinding()
        {
            var binding = new BasicHttpBinding();

            return binding;
        }
    }
}
