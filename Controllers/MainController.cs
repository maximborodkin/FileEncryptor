using FileEncryptor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace FileEncryptor.Controllers
{
    public class MainController : Controller
    {
        private EncryptionViewModel m = new EncryptionViewModel("бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!! у ъящэячэц ъэюоык, едщ бдв саэацкшгнбяр гчеа кчфцшубп цу ьгщпя вщвсящ, эвэчрысй юяуъщнщхо шпуъликугбз чъцшья с цощъвчщ ъфмес ю лгюлэ ёъяяр! с моыящш шпмоец щаярдш цяэубфъ аьгэотызуа дщ, щръ кй юцкъщчьуац уыхэцэ ясч юбюяуяг ыовзсгюамщщ.внютвж тхыч эядкъябе цн юкъль, мэсццогл шяьфыоэьь ть эщсщжнашанэ ыюцен, уёюяыцчан мах гъъьуун шпмоыъй ч яяьпщъхэтпык яущм бпйэае! чэьюмуд, оээ скфч саьбрвчёыа эядуцйт ъ уьгфщуяяёу фси а эацэтшцэч юпапёи, ьь уъубфмч ысь хффы ужц чьяцнааущ эгъщйаъф, ч п эиттпьк ярвчг гмубзньцы! щб ьшяо шачюрэсч FirstLineSoftware ц ешчтфщацдпбр шыыь, р ыоф ячцсвкрщве бттй а ядсецсцкюкх эшашёрэсуъ якжще увюгщр в# уфн ысвчюпжзцж! чй ёюычъ бщххыибй еьюхечр п хкъмэншёцч юятщвфцшчщ с хчю ъэ ч аачсюсчыщачрняун в шъюьэжцясиьццч агфуо ацаьяычсцы .Net, чэбф ыуюбпьщо с чыдпяхбцйг щктрж!", "скорпион", true);
        [HttpGet]
        public IActionResult Index() => View(m);

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
            MemoryStream memoryStream = null;
            StreamWriter streamWriter = null;
            try
            { 
                memoryStream = new MemoryStream();
                streamWriter = new StreamWriter(memoryStream);

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
            finally
            {
                memoryStream?.Close();
                streamWriter?.Close();
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
