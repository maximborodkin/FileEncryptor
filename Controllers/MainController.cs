using FileEncryptor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;

namespace FileEncryptor.Controllers
{
    public class MainController : Controller
    {
        private EncryptionViewModel m = new EncryptionViewModel("бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!! у ъящэячэц ъэюоык, едщ бдв саэацкшгнбяр гчеа кчфцшубп цу ьгщпя вщвсящ, эвэчрысй юяуъщнщхо шпуъликугбз чъцшья с цощъвчщ ъфмес ю лгюлэ ёъяяр! с моыящш шпмоец щаярдш цяэубфъ аьгэотызуа дщ, щръ кй юцкъщчьуац уыхэцэ ясч юбюяуяг ыовзсгюамщщ. внютвж тхыч эядкъябе цн юкъль, мэсццогл шяьфыоэьь ть эщсщжнашанэ ыюцен, уёюяыцчан мах гъъьуун шпмоыъй ч яяьпщъхэтпык яущм бпйэае! чэьюмуд, оээ скфч саьбрвчёыа эядуцйт ъ уьгфщуяяёу фси а эацэтшцэч юпапёи, ьь уъубфмч ысь хффы ужц чьяцнааущ эгъщйаъф, ч п эиттпьк ярвчг гмубзньцы! щб ьшяо шачюрэсч FirstLineSoftware ц ешчтфщацдпбр шыыь, р ыоф ячцсвкрщве бттй а ядсецсцкюкх эшашёрэсуъ якжще увюгщр в# уфн ысвчюпжзцж! чй ёюычъ бщххыибй еьюхечр п хкъмэншёцч юятщвфцшчщ с хчю ъэ ч аачсюсчыщачрняун в шъюьэжцясиьццч агфуо ацаьяычсцы .Net, чэбф ыуюбпьщо с чыдпяхбцйг щктрж!", "скорпион", false);
        [HttpGet]
        public IActionResult Index() => View(m);

        [HttpPost]
        public IActionResult Update(EncryptionViewModel model)
        {
            if(model != null)
            {
                return View("Index", model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult FileUpload(IFormFile file, string key, bool isEncrypted)
        {
            if(file != null && file.Length > 0 && key != null)
            {
                try 
                { 
                    using var fileStream = file.OpenReadStream();
                    using var streamReader = new StreamReader(fileStream);
                    fileStream.Position = 0;
                    string text = streamReader.ReadToEnd();
                    var model = new EncryptionViewModel(text, key, isEncrypted);
                    return View("Index", model);
                } 
                catch (Exception)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
