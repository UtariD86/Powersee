using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using WebUI.Areas.Admin.Models.PlanlanmisVardiya;

namespace WebUI.Areas.Admin.Controllers
{

    [Area("Admin")]


    [Authorize]



    public class PlanlanmisVardiyaController : Controller
    {

        private readonly IPlanlanmisVardiyaService dm;
        private readonly IPersonelService _personelService;
        private readonly IVardiyaService _vardiyaService;
        private readonly IPlanlanmisVardiyaPersonelService _planlanmisVardiyaPersonelService;
        public PlanlanmisVardiyaController(IPlanlanmisVardiyaService _dm, IPersonelService personelService, IVardiyaService vardiyaService, IPlanlanmisVardiyaPersonelService planlanmisVardiyaPersonelService)
        {
            dm = _dm;
            _personelService = personelService;
            _vardiyaService = vardiyaService;
            _planlanmisVardiyaPersonelService = planlanmisVardiyaPersonelService;
        }

        //Index Sayfamıza yönlendirme yapar
        public IActionResult Index()
        {
            return View();
        }


        [Route("{area}/get-planlanmisvardiyalar")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await dm.GetToGrid(pageRequest);
            return Json(data.Data);
        }


        [Route("{area}/get-planlanmisvardiya")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            var model = new PlanlanmisVardiyaDto();
            if (id.HasValue)
            {
                var result = await dm.GetById(id.Value);
                var entity = result.Data;
                
                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.Id = entity.Id;
                    //model.personelId = entity.personelId;
                    model.vardiyaId = entity.vardiyaId;
                    model.baslangicZamani = entity.baslangicZamani;
                    model.bitisZamani = entity.bitisZamani;
                    //model.girisZamani = entity.girisZamani;
                    //model.cikisZamani = entity.cikisZamani;
                    //model.hedefUcret = entity.hedefUcret;
                    //model.kazanilanUcret = entity.kazanilanUcret;
                    model.Personels = await _planlanmisVardiyaPersonelService.GetAllPersonelIds(entity.Id);
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "Planlanmış varidiya bulunamadı.");
                }

            }
            else
            {
                model.baslangicZamani = DateTime.Now;
                model.bitisZamani = DateTime.Now;
            }
            var personelResult = await _personelService.GetAll();
            var personelData = personelResult?.Data?.Select(p => new { Id = p.Id.ToString(), FullName = $"{p.isim} {p.soyisim}" });
            var vardiyaResult = await _vardiyaService.GetAll();
            model.personelIdSel = new SelectList(personelData, "Id","FullName");
            model.vardiyaIdSel = new SelectList(vardiyaResult?.Data, "Id", "vardiyaIsmi");
            model.Personels = new();
            return PartialView(model);
        }


        [Route("{area}/save-planlanmisvardiya")]
        [HttpPost]
        public async Task<IActionResult> Edit(PlanlanmisVardiyaDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var message = "resultMessage";
            var id = model.Id;
            var planlanmisvardiya = new PlanlanmisVardiya()
            {
                Id = model.Id,
                //personelId = model.personelId.Value,
                vardiyaId = model.vardiyaId.Value,
                baslangicZamani = model.baslangicZamani.Value,
                bitisZamani = model.bitisZamani.Value,
                //girisZamani = model.girisZamani,
                //cikisZamani = model.cikisZamani,
                //hedefUcret = model.hedefUcret,
                //kazanilanUcret = model.kazanilanUcret,


            };
            var result = await dm.Edit(planlanmisvardiya);
            model.personelIds = model.personelIdsStr?.Split(',')
                       .Select(s => int.Parse(s))
                       .ToList();
            var atamaResult = await _planlanmisVardiyaPersonelService.UpdateVardiyaPersonel(result.Data.Id, model.personelIds);

            if (!string.IsNullOrEmpty(atamaResult.Message))
            {
                message = atamaResult.Message;
            }

            return Json(message);
        }

        [Route("{area}/delete-planlanmisvardiya")]
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
        [HttpPost]
        [Route("{area}/get-all-vardiya-events")]
        public async Task<IActionResult> GetEvents()
        {
            var vardiyalarResult = await dm.GetAll();
            var personellerResult = await _personelService.GetAll();
            var vardiyaTanimlariResult = await _vardiyaService.GetAll();

            var personeller = personellerResult.Data.ToDictionary(p => p.Id);
            var vardiyaTanimlari = vardiyaTanimlariResult.Data.ToDictionary(v => v.Id);

            var events = vardiyalarResult.Data.Select(v =>
            {
                //var personel = personeller.ContainsKey(v.personelId) ? personeller[v.personelId] : null;
                var vardiya = vardiyaTanimlari.ContainsKey(v.vardiyaId) ? vardiyaTanimlari[v.vardiyaId] : null;

                return new PlanlanmisVardiyaEventDto
                {
                    id = v.Id.ToString(),
                    title = $"{vardiya?.vardiyaIsmi}",
                    start = v.baslangicZamani.ToString("s"),
                    end = v.bitisZamani.ToString("s"),
                    description = $"Giriş: {v.baslangicZamani.ToString("HH:mm")} - Çıkış: {v.bitisZamani.ToString("HH:mm")}",
                    className = "bg-soft-success"
                };
            });

            return Json(events);
        }


    }
}
