
using System;
using System.Security.Cryptography;

namespace BulkyBookApp.Common
{
    public class Encryption
    {
        public static byte[] GenerateKey()
        {
            return Convert.FromBase64String("J6hfA3eRA1+ZoHoQvTz/YUQd/y/5T3B7A8/MZGgN1Bc=");
        }
        public static byte[] GenerateIV()
        {
            return Convert.FromBase64String("dJd+PjCEkGmb2GfGZ1b8+Q==");
        }

        public static string Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt =  new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter smEncrypt = new StreamWriter(csEncrypt))
                        {
                            smEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

    }
}
