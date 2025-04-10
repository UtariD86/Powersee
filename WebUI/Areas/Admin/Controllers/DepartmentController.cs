using Application.Helpers.Concrete;
using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using WebUI.Areas.Admin.Models.Department;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DepartmentController : Controller
    {
        [ActivatorUtilitiesConstructor]
        private readonly IDepartmentService dm;

        public DepartmentController(IDepartmentService _dm)
        {
            dm = _dm;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{area}/get-departments")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await dm.GetToGrid(pageRequest);
            return Json(data.Data);
        }

        [Route("{area}/get-department")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            var model = new DepartmentDto();

            var dummyManagers = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Ahmet Yılmaz" },
                new SelectListItem { Value = "2", Text = "Elif Demir" },
                new SelectListItem { Value = "3", Text = "Mehmet Kaya" }
            };

            if (id.HasValue)
            {
                var result = await dm.GetById(id.Value);
                var entity = result.Data;
                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.Id = entity.Id;
                    model.Name = entity.Name;
                    model.Description = entity.Description;
                    model.Active = entity.Active;
                    model.Managerid = entity.Managerid;
                    model.ManagerList = new SelectList(dummyManagers, "Value", "Text", entity.Managerid);
                    model.Adres = entity.Adres;
                    model.CalismaTuru = entity.CalismaTuru;
                    model.UniqueCode = entity.UniqueCode;
                    
                    
                    // ✅ Enum dropdown verisi burada oluşturuluyor
                    model.CalismaTurusel = new SelectList(
                        Enum.GetValues(typeof(CalismaTuru))
                            .Cast<CalismaTuru>()
                            .Select(e => new SelectListItem
                            {
                                Value = ((int)e).ToString(),
                                Text = e.ToString()
                            }),
                        "Value",
                        "Text",
                        ((int)entity.CalismaTuru).ToString()
                    );
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "Departman bulunamadı.");
                    model.ManagerList = new SelectList(dummyManagers, "Value", "Text");
                    model.CalismaTurusel = EnumHelper.ToSelectList<CalismaTuru>();
                }
            }
            else
            {
                model.ManagerList = new SelectList(dummyManagers, "Value", "Text");
                model.CalismaTurusel = EnumHelper.ToSelectList<CalismaTuru>();
            }

            return PartialView(model);
        }

        [Route("{area}/save-department")]
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = new Department()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Adres = model.Adres,
                Managerid = model.Managerid,
                Active = model.Active,
                CalismaTuru = model.CalismaTuru,
                UniqueCode = model.UniqueCode,
            };

            var result = dm.Edit(department);
            var message = result.Result.Message ?? "İşlem başarılı";
            if (result.Result.ResultStatus == ResultStatus.Error)
                return BadRequest(result.Result.Message);
            return Json(message);
        }

        [Route("{area}/delete-department")]
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
