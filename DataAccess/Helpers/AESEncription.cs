using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Helpers
{
    public class AESEncription
    {
        byte[] Key = Encoding.UTF8.GetBytes("550B87FFBD246C1C9219879BF3154EE1");
        byte[] IV = Encoding.UTF8.GetBytes("2BCCA58849229ADS");

        public string EncryptMensage(string msg)
        {
            var Encryp_Message = EncryptStringToBytes_Aes(msg);
            var Encrypt_Base64 = Convert.ToBase64String(Encryp_Message);
            return Encrypt_Base64;
        }


        public string DecryptMensage(string msg)
        {
            var Decrypt_Base64 = Convert.FromBase64String(msg);
            var Decrypt_Message = DecryptStringFromBytes_Aes(Decrypt_Base64);
            return Decrypt_Message;
        }

        private byte[] EncryptStringToBytes_Aes(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = this.Key;
                aesAlg.IV = this.IV;
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        private string DecryptStringFromBytes_Aes(byte[] cipherText)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold the decrypted text.       
            string plaintext = null;
            // Create an Aes object with the specified key and IV.           
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = this.Key;
                aesAlg.IV = this.IV;
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

    }
}
