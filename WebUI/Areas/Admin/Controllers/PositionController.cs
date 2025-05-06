using Application.Helpers.Concrete;
using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using WebUI.Areas.Admin.Models.Position;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IDepartmentService _departmentService;
        private readonly IPersonelService _personelService;
        public PositionController(IPositionService positionService,IDepartmentService departmentService, IPersonelService personelService)
        {
            _positionService = positionService;
            _departmentService = departmentService;
            _personelService = personelService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{area}/get-positions")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await _positionService.GetToGrid(pageRequest);
            return Json(data.Data);
        }

        [Route("{area}/get-position")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            var personelResult = await _personelService.GetAll();

            var departments = await _departmentService.GetAll(); // Tüm departmanları çek

            var model = new PositionDto();
          

            if (id.HasValue)
            {
                var result = await _positionService.GetById(id.Value);
                var entity = result.Data;

                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.Id = entity.Id;
                    model.Name = entity.Name;
                    model.Salary = entity.Salary.ToString("F", CultureInfo.InvariantCulture);
                    model.Active = entity.Active ?? false;
                    model.ManagerId = entity.ManagerId;
                    model.Code = entity.Code;
                    model.ManagerList = new SelectList(
                          personelResult.Data.Select(p => new SelectListItem
                          {
                              Value = p.Id.ToString(),
                              Text = $"{p.isim} {p.soyisim}" // Veya $"{p.isim} - {p.unvan}" gibi
                          }),
                             "Value", "Text", entity.ManagerId);
                    model.DepartmentList = new SelectList(departments.Data, "Id", "Name", entity.DepartmentId);
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "Pozisyon bulunamadı.");
                    model.ManagerList = new SelectList(
                        personelResult.Data.Select(p => new SelectListItem
                        {
                            Value = p.Id.ToString(),
                            Text = $"{p.isim} {p.soyisim}" // Veya $"{p.isim} - {p.unvan}" gibi
                        }),
                           "Value", "Text");
                    model.DepartmentList = new SelectList(departments.Data, "Id", "Name"); // BURAYA EKLEDİM
                }
            }
            else
            {
                model.ManagerList = new SelectList(personelResult.Data.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = $"{p.isim} {p.soyisim}" }), "Value", "Text");
                model.DepartmentList = new SelectList(departments.Data, "Id", "Name"); // BURAYA EKLEDİM
            }



            return PartialView(model);
        }
        [Route("{area}/save-position")]
        [HttpPost]
        public async Task<IActionResult> Edit(PositionDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Hatalar varsa BadRequest döner
            }
                
            decimal salary = 0;

            // Salary'nin geçerli bir decimal olup olmadığını kontrol et
            if (!string.IsNullOrEmpty(model.Salary))
            {
                if (!decimal.TryParse(model.Salary, NumberStyles.Any, CultureInfo.InvariantCulture, out salary))
                {
                    ModelState.AddModelError("Salary", "Lütfen geçerli bir maaş formatı giriniz.");
                    return BadRequest(ModelState); // Geçerli olmayan maaş formatı hatasıyla BadRequest döner
                }
            }
            else
            {
                ModelState.AddModelError("Salary", "Maaş alanı boş olamaz.");
                return BadRequest(ModelState); // Maaş boş olduğunda BadRequest döner
            }

            var position = new Position
            {
                Id = model.Id,
                Name = model.Name!,
                Salary = salary, // Salary burada geçerli bir decimal olarak atanır
                Active = model.Active.HasValue ? model.Active.Value : false,
                Code = model.Code,
                ManagerId = model.ManagerId,
                DepartmentId = model.DepartmentId ?? 0

            };

            // Servis üzerinden pozisyonu düzenleme işlemi
            var result = _positionService.Edit(position);
            var message = string.IsNullOrEmpty(result.Result.Message) ? "resultMessage" : result.Result.Message;

            return Json(message); // Geriye mesajı döner
        }






        [Route("{area}/delete-position")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int? id)
        {
            string message = string.Empty;

            if (id.HasValue && id > 0)
            {
                var result = await _positionService.Delete(id.Value);
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
