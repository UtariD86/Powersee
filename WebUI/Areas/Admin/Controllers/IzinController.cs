using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WebUI.Areas.Admin.Models.Izin;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class IzinController : Controller
    {
        private readonly IIzinService dm;
        public IzinController(IIzinService _dm)
        {
            dm = _dm;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{area}/get-izinler")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await dm.GetToGrid(pageRequest);
            return Json(data.Data);
        }

        [Route("{area}/get-izin")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            var model = new IzinDto();
            if (id.HasValue)
            {
                var result = await dm.GetById(id.Value);
                var entity = result.Data;
                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.PersonelId = entity.PersonelId.ToString();

                    model.Id = entity.Id;
                    model.BaslangicTarihi = entity.BaslangicTarihi;
                    model.BitisTarihi = entity.BitisTarihi;
                    model.Aciklama = entity.Aciklama;
                    model.IzinTuruEnum = entity.IzinTuruEnum;
                    model.UcretTuruEnum = entity.UcretTuruEnum;
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "İzin bulunamadı.");
                }
            }
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
                BaslangicTarihi = model.BaslangicTarihi,
                BitisTarihi = model.BitisTarihi,
                Aciklama = model.Aciklama,
                IzinTuruEnum = model.IzinTuruEnum,
                UcretTuruEnum = model.UcretTuruEnum
            };

            var result = dm.Edit(izin);
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