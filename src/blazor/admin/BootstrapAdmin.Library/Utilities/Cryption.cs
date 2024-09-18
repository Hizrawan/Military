using System.Security.Cryptography;
using System.Text;

namespace BootstrapAdmin.Library.Utilities
{
    public static class CryptionFunction
    {
        /// <summary>
        /// 使用 SHA512 雜湊函數加密文字
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns></returns>
        public static string GetCipherTextBySHA512HashFunction(this string plainText)
        {
            byte[] plaimTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] hashBytes = SHA512.HashData(plaimTextBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
