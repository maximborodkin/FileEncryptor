using FileEncryptor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using DocumentFormat.OpenXml;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace FileEncryptor.Controllers
{
    public class MainController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Update(EncryptionViewModel model) => model == null ? RedirectToAction("Index") : View("Index", model);

        [HttpPost]
        public IActionResult FileUpload(IFormFile file, string key, bool isEncrypted)
        {
            var model = new EncryptionViewModel(null, key, isEncrypted);
            if (file != null && file.Length > 0 && key != null)
            {
                try 
                {
                    var text = ExtractText(file);
                    model.Text = text;
                } 
                catch (NotSupportedException)
                {
                    model.ErrorMessage = EncryptionViewModel.fileExtensionError;
                } 
                catch (Exception)
                {
                    model.ErrorMessage = EncryptionViewModel.fileReadingError;
                }
            } 
            else
            {
                model.ErrorMessage = EncryptionViewModel.emptyFileError;
            }
            return View("Index", model);
        }

        private string ExtractText(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);
            switch (fileExtension)
            {
                case ".txt":
                    using (var fileStream = file.OpenReadStream())
                    {
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        var encoding = Encoding.GetEncoding(1251);
                        
                        using var streamReader = new StreamReader(fileStream, encoding);
                        fileStream.Position = 0;
                        return streamReader.ReadToEnd();
                    }
                case ".docx":
                    using (var fileStream = file.OpenReadStream())
                    {
                        WordprocessingDocument document = WordprocessingDocument.Open(fileStream, false);
                        var body = document.MainDocumentPart.Document.Body;
                        return body.InnerText;
                    }
                default:
                    throw new NotSupportedException("Unsupported file extension");
            }
        }

        public IActionResult DownloadTXT(EncryptionViewModel model)
        {
            try
            { 
                var memoryStream = new MemoryStream();
                var streamWriter = new StreamWriter(memoryStream);

                streamWriter.Write(model.Result);
                streamWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return File(memoryStream, "text/plan", "result.txt");
            } 
            catch (Exception)
            {
                model.ErrorMessage = EncryptionViewModel.generateFileError;
                return View("Index", model);
            }
        }

        public IActionResult DownloadDOCX(EncryptionViewModel model)
        {
            try
            {
                var memoryStream = new MemoryStream();

                var document = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document, true);
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                new Document(new Body()).Save(mainPart);
                Body body = mainPart.Document.Body;
                body.Append(new Paragraph(new Run(new Text(model.Result))));

                mainPart.Document.Save();
                document.Close();
                memoryStream.Position = 0;

                return File(memoryStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "result.docx");
            }
            catch (Exception)
            {
                model.ErrorMessage = EncryptionViewModel.generateFileError;
                return View("Index", model);
            }
        }
    }
}
