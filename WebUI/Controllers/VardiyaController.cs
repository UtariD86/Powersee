using Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models.Vardiya;

namespace WebUI.Controllers
{
    public class VardiyaController : Controller
    {
        private readonly IPlanlanmisVardiyaService _planlanmisVardiyaService;
        private readonly IPersonelService _personelService;
        private readonly IPlanlanmisVardiyaPersonelService _planlanmisVardiyaPersonelService;

        public VardiyaController(IPlanlanmisVardiyaService planlanmisVardiyaService, IPersonelService personelService, IPlanlanmisVardiyaPersonelService planlanmisVardiyaPersonelService)
        {
            _planlanmisVardiyaService = planlanmisVardiyaService;
            _personelService = personelService;
            _planlanmisVardiyaPersonelService = planlanmisVardiyaPersonelService;
        }


        [Route("vardiya/{vardiyaId}/giris")]
        public async Task<IActionResult> VardiyaGiris(int vardiyaId)
        {
            VardiyaGirisDto model = new();

            model.VardiyaId = vardiyaId;
            return View(model);
        }


        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode(string kod1, int vardiyaId, bool isGiris)
        {
            if (string.IsNullOrEmpty(kod1))
            {
                return Json(new { success = false, message = "Kod boş olamaz!" });
            }

            var personel = await _personelService.GetByCode(kod1);

            if (personel.ResultStatus == Core.Enums.ResultStatus.Error)
            {
                return Json(new { success = false, message = "Personel Bu vardiyaya kayıtlı değil!" });
            }

            // 'TryOperation' metodunun async olduğu için 'await' kullanıyoruz
            var planlanmisVardiyaPersonelResult = await _planlanmisVardiyaPersonelService.TryOperation(isGiris, vardiyaId, personel.Data.Id);

            // Eğer işlem başarısızsa
            if (planlanmisVardiyaPersonelResult.ResultStatus == Core.Enums.ResultStatus.Error)
            {
                return Json(new { success = false, message = planlanmisVardiyaPersonelResult.Message });
            }

            // Başarılı olduğunda
            return Json(new { success = true });
        }


        [Route("vardiya/{vardiyaId}/cikis")]
        public async Task<IActionResult> VardiyaCikis(int vardiyaId)
        {
            VardiyaGirisDto model = new();

            model.VardiyaId = vardiyaId;
            return View(model);
        }



    }
}
