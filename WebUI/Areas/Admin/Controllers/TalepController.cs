using Application.Helpers.Concrete;
using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Security.Claims;
using WebUI.Areas.Admin.Models.Personel;
using WebUI.Areas.Admin.Models.Talep;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TalepController : Controller
    {
        private readonly ITalepService _talepService;
        private readonly IPersonelService _personelService;
        private readonly UserManager<IdentityUser> _userManager;
        public TalepController(ITalepService talepService, IPersonelService personelService, UserManager<IdentityUser> userManager)
        {
            _talepService = talepService;
            _personelService = personelService;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("{area}/get-taleps")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await _talepService.GetToGrid(pageRequest);
            return Json(data.Data);
        }
        [Route("{area}/get-talep")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            var personelResult = await _personelService.GetAll();
            var model = new TalepDto();

            if (id.HasValue)
            {
                var result = await _talepService.GetById(id.Value);
                var entity = result.Data;

                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.Id = entity.Id;
                    model.planlanmisVardiyaId = entity.planlanmisVardiyaId;
                    model.aciklama = entity.aciklama;
                    model.aliciId = entity.aliciId;
                    model.Durum = entity.Durum;
                    model.TalepTuru = entity.TalepTuru; 
                    // Alici seçili yapılacak
                    model.aliciSel = new SelectList(personelResult.Data, "Id", "isim", entity.aliciId);

                    // GondericiId'yi manuel olarak set et
                    model.gondericiId = entity.gondericiId;

                    // Enum'ları seçili olarak ayarla
                    model.TalepTurusel = new SelectList(
                        Enum.GetValues(typeof(TalepTuru))
                            .Cast<TalepTuru>()
                            .Select(e => new SelectListItem
                            {
                                Value = ((int)e).ToString(),
                                Text = e.ToString()
                            }),
                        "Value", "Text", ((int)entity.TalepTuru).ToString() // Seçili olan değeri buraya ver
                    );

                    model.Durumsel = new SelectList(
                        Enum.GetValues(typeof(Durum))
                            .Cast<Durum>()
                            .Select(e => new SelectListItem
                            {
                                Value = ((int)e).ToString(),
                                Text = e.ToString()
                            }),
                        "Value", "Text", ((int)entity.Durum).ToString() // Seçili olan durumu buraya ver
                    );
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "Talep bulunamadı.");
                }
            }
            else
            {

                model.aliciSel = new SelectList(personelResult.Data, "Id", "isim"); // Alici Sel boş
                model.TalepTurusel = EnumHelper.ToSelectList<TalepTuru>();
                model.Durumsel = EnumHelper.ToSelectList<Durum>();
                model.Durum = Durum.Beklemede;
            }

            return PartialView(model);
        }



        [Route("{area}/save-talep")]
[HttpPost]
public async Task<IActionResult> Edit(TalepDto model)
{
            model.gondericiId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ModelState.Remove("gondericiId");

            if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return StatusCode(500, "Kullanıcı bilgisi alınamadı.");
            }

            var user = await _userManager.FindByNameAsync(userName);

            var talep = new Talep()
    {
        Id = model.Id,
        aciklama = model.aciklama,
        aliciId = model.aliciId,
        Durum = model.Durum,
        TalepTuru = model.TalepTuru,
        gondericiId = user.Id,
        planlanmisVardiyaId=model.planlanmisVardiyaId
    };

    var result = _talepService.Edit(talep);
    string message = result?.Result?.Message ?? "İşlem tamamlandı.";
    return Json(message);
}

        //Departman silme işlemleri için kullanacağımız endpoint
        [Route("{area}/delete-talep")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int? id)
        {
            string message = string.Empty;
            if (id.HasValue && id > 0)
            {
                var result = await _talepService.Delete(id.Value);
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