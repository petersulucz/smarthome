namespace HomeHub.Common.Security
{
    using System;

    using Exceptions;

    /// <summary>
    /// The account token.
    /// </summary>
    public class AccountToken
    {
        /// <summary>
        /// The binary form.
        /// </summary>
        public readonly byte[] Bytes;

        /// <summary>
        /// The base 64 encoded token.
        /// </summary>
        public readonly string Base64;

        public AccountToken(string token)
        {
            try
            {
                this.Bytes = Convert.FromBase64String(token);
            }
            catch (Exception)
            {
                throw new BadTokenException("The token has been provided in an improper format. Base64 encoding is required.");
            }
            
            this.Base64 = token;
            this.Validate();
        }

        /// <summary>
        /// Validate this token
        /// </summary>
        private void Validate()
        {
            if (this.Bytes.Length != 64)
            {
                throw new BadTokenException("The token length is invalid. 512 bit expected.");
            }
        }
    }
}
