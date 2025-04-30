using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WebUI.Areas.Admin.Models.Vardiya;

namespace WebUI.Areas.Admin.Controllers
{
    
    [Area("Admin")]

    
    [Authorize]
    



    public class VardiyaController : Controller
    {
        
        private readonly IVardiyaService dm;
        public VardiyaController(IVardiyaService _dm)
        {
            dm = _dm;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        
        [Route("{area}/get-vardiyalar")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await dm.GetToGrid(pageRequest);
            return Json(data.Data);
        }

        
        [Route("{area}/get-vardiya")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            var model = new VardiyaDto();
            if (id.HasValue)
            {
                var result = await dm.GetById(id.Value);
                var entity = result.Data;
                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.Id = entity.Id;
                    model.vardiyaIsmi = entity.vardiyaIsmi;
                    model.baslangicSaati = entity.baslangicSaati;
                    model.calismaSuresi = entity.calismaSuresi;
                    model.aciklama = entity.aciklama;
                    model.listelenecek = entity.listelenecek;
                    model.ucretKatsayisi = entity.ucretKatsayisi.ToString();
                    model.esneklikPayiSuresi = entity.esneklikPayiSuresi;
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "Vardiya bulunamadı.");
                }

            }
            return PartialView(model);
        }

        
        [Route("{area}/save-vardiya")]
        [HttpPost]
        public async Task<IActionResult> Edit(VardiyaDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            var message = "resultMessage";
            var id = model.Id;
            var vardiya = new Vardiya()
            {
                Id = model.Id,
                vardiyaIsmi = model.vardiyaIsmi,
                baslangicSaati = model.baslangicSaati,
                calismaSuresi = model.calismaSuresi,
                aciklama = model.aciklama,
                listelenecek = model.listelenecek,
                ucretKatsayisi = decimal.TryParse(model.ucretKatsayisi, out decimal ucretKatsayisiDec) ? ucretKatsayisiDec : 0,
                esneklikPayiSuresi = model.esneklikPayiSuresi,
            };
            vardiya.Id = id;
            var result = dm.Edit(vardiya);
            if (!string.IsNullOrEmpty(result.Result.Message))
            {
                message = result.Result.Message;
            }

            return Json(message);
        }

        [Route("{area}/delete-vardiya")]
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
