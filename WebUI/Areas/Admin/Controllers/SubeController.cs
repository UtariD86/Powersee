using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WebUI.Areas.Admin.Models.Sube;

namespace WebUI.Areas.Admin.Controllers
{
    //Bu satır arealar arasında farklı izinlere sahip endpointler oluşturabilmemizi sağlar.
    [Area("Admin")]

    //Bu satır yalnızca yetkilendirilmiş kullanıcıların bu controller sınıfındaki işlemleri yapabilmesini sağlar.
    [Authorize]
    //Bunu daha sonra role bazında olacak şekilde düzenleyeceğiz.


    //Controller sınıflarımız web uygulamamızda kullanacağımız endpointlerin yönlendirilmesini sağlar.
    //Sayfalar, json veriler vb işlemlerin yönetimi ve gösterimi için kullanılır.
    public class SubeController : Controller
    {
        //Tanımladığımız servis sınıfına erişim sağlamak için bir değişken tanımlıyoruz.
        private readonly ISubeService dm;
        public SubeController(ISubeService _dm)
        {
            dm = _dm;
        }

        //Index Sayfamıza yönlendirme yapar
        public IActionResult Index()
        {
            return View();
        }

        //Departmanlarımızı listelemek için kullanacağımız endpoint Gride getirir
        [Route("{area}/get-subeler")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await dm.GetToGrid(pageRequest);
            return Json(data.Data);
        }

        //Departman ekleme ve güncelleme işlemleri için kullanacağımız endpoint modalı açar
        [Route("{area}/get-sube")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody]int? id)
        {
            var model = new SubeDto();
            if (id.HasValue)
            {
                var result = await dm.GetById(id.Value);
                var entity = result.Data;
                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.Id = entity.Id;
                    model.Subeisim = entity.Subeisim;
                    model.Adres = entity.Adres;
                    model.TelefonNumarasi1 = entity.TelefonNumarasi1;
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "Şube bulunamadı.");
                }

            }
            return PartialView(model);
        }

        //Departman ekleme ve güncelleme işlemleri için kullanacağımız endpoint kaydı yapar
        [Route("{area}/save-sube")]
        [HttpPost]
        public async Task<IActionResult> Edit(SubeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Hatalar varsa BadRequest döner
            }
            var message = "resultMessage";
            var id = model.Id;
            var sube = new Sube()
            {
                Id = model.Id,
                Subeisim = model.Subeisim,
                Adres = model.Adres,
                TelefonNumarasi1 = model.TelefonNumarasi1,
            };
            sube.Id = id;
            var result = dm.Edit(sube);
            if (!string.IsNullOrEmpty(result.Result.Message))
            {
                message = result.Result.Message;
            }

            return Json(message);
        }

        //Departman silme işlemleri için kullanacağımız endpoint
        [Route("{area}/delete-sube")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int? id)
        {
            string message = string.Empty;
            if (id.HasValue && id > 0)
            {
                var result = await dm.Delete(id.Value);
                message = result.Message;
            }
            else
            {
                ModelState.AddModelError("ErrorDetail", "Silinirken bir hata oluştu!");
            }
            return Json(message);
        }
    }
}
