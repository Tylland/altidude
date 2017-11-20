using System.Security.Cryptography;
using System.Text;

namespace Altidude.Infrastructure
{
    public static class KeyGenerator
    {
        public static string GetUniqueKey(int maxSize = 30)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }

            var  result = new StringBuilder(maxSize);

            foreach (byte b in data)
                result.Append(chars[b % (chars.Length)]);

            return result.ToString();
        }
    }
}
