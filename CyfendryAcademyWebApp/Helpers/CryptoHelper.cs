using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CyfendryAcademyWebApp.Helpers
{
    public class CryptoHelper
    {
        public string Hash(string secret, string salt)
        {
            HashAlgorithm hashAlgorithm = SHA512.Create();
            byte[] byteArray = new byte[32];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(byteArray);

            List<byte> pass = new List<byte>(Encoding.Unicode.GetBytes(secret + salt));
            pass.AddRange(byteArray);
            return Convert.ToBase64String(hashAlgorithm.ComputeHash(pass.ToArray()));
        }
        public string RetrieveHash(string secret, string salt)
        {
            HashAlgorithm hashAlgorithm = SHA512.Create();
            byte[] computedHash = null;

            byte[] Salt = Encoding.ASCII.GetBytes(salt);
            List<byte> buffer = new List<byte>(Encoding.Unicode.GetBytes(secret));
            buffer.AddRange(Salt);
            computedHash = hashAlgorithm.ComputeHash(buffer.ToArray());
            return Convert.ToBase64String(computedHash);

        }
    }
}