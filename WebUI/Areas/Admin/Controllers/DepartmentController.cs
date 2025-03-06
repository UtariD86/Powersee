using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WebUI.Areas.Admin.Models.Department;

namespace WebUI.Areas.Admin.Controllers
{
    //Bu satır arealar arasında farklı izinlere sahip endpointler oluşturabilmemizi sağlar.
    [Area("Admin")]

    //Bu satır yalnızca yetkilendirilmiş kullanıcıların bu controller sınıfındaki işlemleri yapabilmesini sağlar.
    [Authorize]
    //Bunu daha sonra role bazında olacak şekilde düzenleyeceğiz.


    //Controller sınıflarımız web uygulamamızda kullanacağımız endpointlerin yönlendirilmesini sağlar.
    //Sayfalar, json veriler vb işlemlerin yönetimi ve gösterimi için kullanılır.
    public class DepartmentController : Controller
    {
        //Tanımladığımız servis sınıfına erişim sağlamak için bir değişken tanımlıyoruz.
        private readonly IDepartmentService dm;
        public DepartmentController(IDepartmentService _dm)
        {
            dm = _dm;
        }

        //Index Sayfamıza yönlendirme yapar
        public IActionResult Index()
        {
            return View();
        }

        //Departmanlarımızı listelemek için kullanacağımız endpoint Gride getirir
        [Route("{area}/get-departments")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await dm.GetToGrid(pageRequest);
            return Json(data.Data);
        }

        //Departman ekleme ve güncelleme işlemleri için kullanacağımız endpoint modalı açar
        [Route("{area}/get-department")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody]int? id)
        {
            var model = new DepartmentDto();
            if (id.HasValue)
            {
                var result = await dm.GetById(id.Value);
                var entity = result.Data;
                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.Id = entity.Id;
                    model.Name = entity.Name;
                    model.Description = entity.Description;
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "Departman bulunamadı.");
                }

            }
            return PartialView(model);
        }

        //Departman ekleme ve güncelleme işlemleri için kullanacağımız endpoint kaydı yapar
        [Route("{area}/save-department")]
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Hatalar varsa BadRequest döner
            }
            var message = "resultMessage";
            var id = model.Id;
            var department = new Department()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
            };
            department.Id = id;
            var result = dm.Edit(department);
            if (!string.IsNullOrEmpty(result.Result.Message))
            {
                message = result.Result.Message;
            }

            return Json(message);
        }

        //Departman silme işlemleri için kullanacağımız endpoint
        [Route("{area}/delete-department")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await dm.Delete(id.Value);
                if (!string.IsNullOrEmpty(result.Message))
                {
                    TempData["PopupMessage"] = result.Message;
                }
            }
            else
            {
                ModelState.AddModelError("ErrorDetail", "Silinirken bir hata oluştu!");
            }
            return RedirectToAction("Index");
        }
    }
}
