using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WebUI.Areas.Admin.Controllers;

namespace WebUI.Areas.Admin.Controllers
{
    //Bu satır arealar arasında farklı izinlere sahip endpointler oluşturabilmemizi sağlar.
    [Area("Admin")]

    //Bu satır yalnızca yetkilendirilmiş kullanıcıların bu controller sınıfındaki işlemleri yapabilmesini sağlar.
    [Authorize]
    //Bunu daha sonra role bazında olacak şekilde düzenleyeceğiz.


    //Controller sınıflarımız web uygulamamızda kullanacağımız endpointlerin yönlendirilmesini sağlar.
    //Sayfalar, json veriler vb işlemlerin yönetimi ve gösterimi için kullanılır.
    public class PlanlanmisVardiyaController : Controller
    {
        //Tanımladığımız servis sınıfına erişim sağlamak için bir değişken tanımlıyoruz.
        private readonly ISubeService dm;
        public PlanlanmisVardiyaController(ISubeService _dm)
        {
            dm = _dm;
        }

        //Index Sayfamıza yönlendirme yapar
        public IActionResult Index()
        {
            return View();
        }
    }
    }