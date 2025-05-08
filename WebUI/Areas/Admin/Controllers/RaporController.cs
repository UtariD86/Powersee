using Application.Helpers.Concrete;
using Application.Services.Abstract;
using Core.Enums;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.Areas.Admin.Models.Department;
using WebUI.Areas.Admin.Models.Rapor;


namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RaporController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IPositionService _positionService;
        private readonly IPersonelService _personelService;
        private readonly IKesintiService _kesintiService;
        private readonly IIzinService _izinService;
        private readonly IPlanlanmisVardiyaService _planlanmisVardiyaService;
        private readonly IVardiyaService _vardiyaService;
        private readonly ISubeService _subeService;
        private readonly ITalepService _talepService;
        private readonly IPlanlanmisVardiyaPersonelService _planlanmisVardiyaPersonelService;

        public RaporController(IDepartmentService departmentService, IPositionService positionService, IPersonelService personelService, IKesintiService kesintiService, IIzinService izinService, IPlanlanmisVardiyaService planlanmisVardiyaService, IVardiyaService vardiyaService, ISubeService subeService, ITalepService talepService, IPlanlanmisVardiyaPersonelService planlanmisVardiyaPersonelService)
        {
            _departmentService = departmentService;
            _positionService = positionService;
            _personelService = personelService;
            _kesintiService = kesintiService;
            _izinService = izinService;
            _planlanmisVardiyaService = planlanmisVardiyaService;
            _vardiyaService = vardiyaService;
            _subeService = subeService;
            _talepService = talepService;
            _planlanmisVardiyaPersonelService = planlanmisVardiyaPersonelService;
        }


        [Route("{area}/departmanBazindaPersonelSayisi")]
        [HttpPost]
        public async Task<IActionResult> DepartmanBazindaPersonelSayisi()
        {
            var model = new DepartmanBazindaPersonelSayisiDto();
               model.Liste = await _departmentService.GetAllDepartentsWithPersonelCounts();

            return PartialView(model);
        }

        [Route("{area}/pozisyonBazindaPersonelSayisi")]
        [HttpPost]
        public async Task<IActionResult> PozisyonBazindaPersonelSayisi()
        {
            var model = new PozisyonBazindaPersonelSayisiDto();
               model.Liste = await _positionService.GetAllPositionsWithPersonelCounts();

            return PartialView(model);
        }

    }
}
