using FileEncryptor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using FileEncryptor.Extensions;

namespace FileEncryptor.Controllers
{
    public class MainController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = HttpContext.Session.GetSerializable<EncryptionViewModel>("model");
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(EncryptionViewModel model)
        {
            if(model != null)
            {
                HttpContext.Session.SetSerializable("model", model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult FileUpload(IFormFile file, string key, bool isEncrypted)
        {
            if(file != null && file.Length > 0 && key != null)
            {
                using var fileStream = file.OpenReadStream();
                using var streamReader = new StreamReader(fileStream);
                fileStream.Position = 0;
                string text = streamReader.ReadToEnd();
                var model = new EncryptionViewModel(text, key, isEncrypted);

                HttpContext.Session.SetSerializable("model", model);
            }
            return RedirectToAction("Index");
        }
    }
}
