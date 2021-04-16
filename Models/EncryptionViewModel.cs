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

        public EncryptionViewModel(string text, string key, bool isEncrypted, string errorMessage = null)
        {
            Text = text;
            Key = key;
            IsEncrypted = isEncrypted;
            ErrorMessage = errorMessage;
        }

        [Required]
        public string Text { get; set; }
       
        [Required]
        public string Key { get; set; }
        
        [Required]
        public bool IsEncrypted { get; set; }

        private string result;
        public string Result {
            get 
            { 
                if (result == null)
                {
                    result = GetGeneratedText();
                }
                return result;
            }
            set => result = value;
        }

        public string ErrorMessage { get; set; }
        public const string emptyTextError = "Необхдим исходный текст";
        public const string emptyKeyError = "Необхдим ключ";
        public const string keyCharactersError = "Ключ должен состоять из букв кириллического алфавита";
        public const string cipherError = "Во время преобразования возникла ошибка";
        public const string emptyFileError = "Необходим непустой файл";
        public const string fileReadingError = "Ошибка чтения из файла";
        public const string fileExtensionError = "Неподдерживаемый формат файла";
        public const string generateFileError = "Ошибка при создании файла";

        private static readonly List<char> alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray().ToList();

        private string GetGeneratedText()
        {
            if (!Validate()) return null;
            try
            {
                var encryption = new StringBuilder();
                int keyIndex = 0;
                foreach (char oldChar in Text)
                {
                    if (alphabet.Contains(char.ToLower(oldChar)))
                    {
                        if (keyIndex == Key.Length) keyIndex = 0;
                        int offset = alphabet.IndexOf(char.ToLower(Key[keyIndex++]));
                        int oldCharIndex = alphabet.IndexOf(char.ToLower(oldChar));
                        int newCharIndex = 
                            IsEncrypted ? (oldCharIndex - offset < 0 ? alphabet.Count + (oldCharIndex - offset) : oldCharIndex - offset)
                            : ((oldCharIndex + offset) % alphabet.Count);
                        char newChar = char.IsLower(oldChar) ? alphabet[newCharIndex] : char.ToUpper(alphabet[newCharIndex]);
                        encryption.Append(newChar);
                    }
                    else
                    {
                        encryption.Append(oldChar);
                    }
                }
                return encryption.ToString();
            }
            catch (Exception)
            {
                ErrorMessage = cipherError;
                return string.Empty;
            }
        }

        public bool Validate()
        {
            if (ErrorMessage != null) return false;

            if(Text == null || Text.Trim().Length == 0)
            {
                ErrorMessage = emptyTextError;
                return false;
            } else if(Key == null || Key.Trim().Length == 0)
            {
                ErrorMessage = emptyKeyError;
                return false;
            } else if (!Key.All(c => alphabet.Contains(char.ToLower(c))))
            {
                ErrorMessage = keyCharactersError;
                return false;
            } else
            {
                return true;
            }
        }
    }
}
