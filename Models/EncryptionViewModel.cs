using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        public string Text { get; set; }
       
        public string Key { get; set; }
        
        public bool IsEncrypted { get; set; }
        
        public string GetGeneratedText() => IsEncrypted ? DecryptText() : EncryptText();

        private string EncryptText()
        {
            return "encrypted text " + Text;
        }

        private string DecryptText()
        {
            return "decrypted text " + Text;
        }

        public bool IsValid() => !string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Key);
    }
}
