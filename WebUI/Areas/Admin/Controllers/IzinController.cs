using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using WebUI.Areas.Admin.Models.Izin;

// Buradan aşağıdakiler deneme
using Domain.Enums;
using WebUI.Areas.Admin.Models.Department;
using WebUI.Areas.Admin.Models.Personel;
using WebUI.Areas.Admin.Models.Position;
using WebUI.Areas.Admin.Models.Sube;
// Buraya kadar

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class IzinController : Controller
    {
        private readonly IIzinService _izinService;
        private readonly IPersonelService _personelService;
        public IzinController(IIzinService izinService, IPersonelService personelService)
        {
            _izinService = izinService;
            _personelService = personelService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{area}/get-izinler")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await _izinService.GetToGrid(pageRequest);
            return Json(data.Data);
        }

        [Route("{area}/get-izin")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            var model = new IzinDto();
            if (id.HasValue)
            {
                var result = await _izinService.GetById(id.Value);
                var entity = result.Data;
                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.Id = entity.Id;
                    model.PersonelId = entity.PersonelId.ToString();
                    model.Aciklama = entity.Aciklama;
                    model.BaslangicTarihi = entity.BaslangicTarihi;
                    model.BitisTarihi = entity.BitisTarihi;
                    model.IzinTuruEnum = entity.IzinTuruEnum;
                    model.UcretTuruEnum = entity.UcretTuruEnum;
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "İzin bulunamadı.");
                }
            }

            var personelResult = await _personelService.GetAll();
           

            model.personelResultSel = new SelectList(personelResult?.Data, "Id", "İsim", "Soyisim");
            

            return PartialView(model);
        }

        [Route("{area}/save-izin")]
        [HttpPost]
        public async Task<IActionResult> Edit(IzinDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = "resultMessage";
            var id = model.Id;
            var izin = new Izin()
            {
                Id = model.Id,
                PersonelId = int.TryParse(model.PersonelId, out int PersonelIdInt) ? PersonelIdInt : 227,
                Aciklama = model.Aciklama,
                BaslangicTarihi = model.BaslangicTarihi,
                BitisTarihi = model.BitisTarihi,
                IzinTuruEnum = model.IzinTuruEnum,
                UcretTuruEnum = model.UcretTuruEnum
            };
            izin.Id = id;
            var result = _izinService.Edit(izin);
            if (!string.IsNullOrEmpty(result.Result.Message))
            {
                message = result.Result.Message;
            }

            return Json(message);
        }

        [Route("{area}/delete-izin")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int? id)
        {
            string message = string.Empty;
            if (id.HasValue && id > 0)
            {
                var result = await _izinService.Delete(id.Value);
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