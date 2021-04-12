using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileEncryptor.Models
{
    public class File
    {
        private File(string encryptedText, string decryptedText, string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length != 43) 
                throw new ArgumentException("Invalid key");

            if (string.IsNullOrEmpty(encryptedText) && string.IsNullOrEmpty(decryptedText))
                throw new ArgumentException("One of encrypted text or decrypted text must not be null");

            EncryptedText = encryptedText;
            DecryptedText = decryptedText;
            Key = key;
        }

        private string encryptedText;
        public string EncryptedText 
        {
            get => encryptedText ?? EncryptText();
            private set => encryptedText = value;
        }

        private string decryptedText;
        public string DecryptedText
        {
            get => decryptedText ?? DecryptText();
            private set => decryptedText = value;
        }

        public string Key { get; private set; }

        private string EncryptText()
        {
            return "encrypted text";
        }

        private string DecryptText()
        {
            return "decrypted text";
        }

        public static File GenerateEncryptedFile(string encryptedText, string key)
        {
            return new File(encryptedText, null, key);
        }

        public static File GenerateDecryptedFile(string decryptedText, string key)
        {
            return new File(null, decryptedText, key);
        }
    }
}
