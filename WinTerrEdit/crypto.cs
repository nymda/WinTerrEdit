using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WinTerrEdit
{
    /// <summary>
    /// 
    /// handles all the cryptography for the .plr files
    /// 
    /// </summary>
    class crypto
    {
        private const string EncryptionKey = "h3y_gUyZ";
        public FileInfo fi;

        public byte[] decryptFile(string path)
        {
            byte[] enc = File.ReadAllBytes(path);
            fi = new FileInfo(path);
            byte[] decrypted = decryptNew(enc, (int)fi.Length);
            return decrypted;
        }
        public byte[] decryptNew(byte[] encrypted, int length)
        {
            using (var rijndaelManaged = new RijndaelManaged { Padding = PaddingMode.None })
            using (var memoryStream = new MemoryStream(encrypted))
            {
                var bytes = new UnicodeEncoding().GetBytes(EncryptionKey);
                using (var cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read))
                using (var reader = new BinaryReader(cryptoStream))
                {
                    return reader.ReadBytes(length);
                }
            }
        }
        public void encryptAndSave(byte[] decryptedData, string path)
        {
            byte[] encrypted;
            using (var rijndaelManaged = new RijndaelManaged { Padding = PaddingMode.None })
            using (var memoryStream = new MemoryStream(decryptedData))
            {
                var bytes = new UnicodeEncoding().GetBytes(EncryptionKey);
                using (var cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(bytes, bytes), CryptoStreamMode.Read))
                using (var reader = new BinaryReader(cryptoStream))
                {
                    encrypted = reader.ReadBytes(decryptedData.Length);
                }
            }
            File.WriteAllBytes(path, encrypted);
        }
    }
}
