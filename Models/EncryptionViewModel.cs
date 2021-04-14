using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FileEncryptor.Models
{
    [Serializable]
    public class EncryptionViewModel
    {
        public EncryptionViewModel() { }

        public EncryptionViewModel(string text, string key, bool isEncrypted)
        {
            Text = text;
            Key = key;
            IsEncrypted = isEncrypted;
        }

        [Required(ErrorMessage = "Необхдим исходный текст")]
        public string Text { get; set; }
       
        [Required(ErrorMessage = "Необхдим ключ")]
        public string Key { get; set; }
        
        [Required]
        public bool IsEncrypted { get; set; }

        private static readonly List<char> alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray().ToList(); 

        // not working
        private string EncryptText()
        {
            var encryption = new StringBuilder();
            try
            {
                int keyIndex = 0;
                foreach (char oldChar in Text)
                {
                    if (alphabet.Contains(oldChar))
                    {
                        if (keyIndex == Key.Length) keyIndex = 0;
                        int offset = alphabet.IndexOf(Key[keyIndex++]);
                        int oldCharIndex = alphabet.IndexOf(oldChar);
                        int newCharIndex = oldCharIndex + offset % alphabet.Count;
                        char newChar = char.IsLower(oldChar) ? alphabet[newCharIndex] : char.ToUpper(alphabet[newCharIndex]);
                        encryption.Append(newChar);
                    }
                    else
                    {
                        encryption.Append(oldChar);
                    }
                }
            }
            catch (Exception)
            {
                //return "Во время преобразования возникла ошибка";
            }
            return encryption.ToString();
        }

        private string DecryptText()
        {
            var decryption = new StringBuilder();
            try
            {
                int keyIndex = 0;
                foreach (char oldChar in Text)
                {
                    if (alphabet.Contains(oldChar))
                    {
                        if (keyIndex == Key.Length) keyIndex = 0;
                        int offset = alphabet.IndexOf(Key[keyIndex++]);
                        int oldCharIndex = alphabet.IndexOf(oldChar);
                        int newCharIndex = oldCharIndex - offset < 0 ? alphabet.Count + (oldCharIndex - offset) : oldCharIndex - offset;
                        char newChar = char.IsLower(oldChar) ? alphabet[newCharIndex] : char.ToUpper(alphabet[newCharIndex]);
                        decryption.Append(newChar);
                    }
                    else
                    {
                        decryption.Append(oldChar);
                    }
                }
            }
            catch (Exception)
            {
                return "Во время преобразования возникла ошибка";
            }
            return decryption.ToString();
        }

        public string GetGeneratedText() => IsEncrypted ? DecryptText() : EncryptText();

        public bool IsValid() => !string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Key);
    }
}
