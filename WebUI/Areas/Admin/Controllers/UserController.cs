using Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUI.Models.Vardiya;

namespace WebUI.Areas.Admin.Controllers
{
        [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IPersonelService _personelService;

        public UserController(IPersonelService personelService)
        {
            _personelService = personelService;
        }



        [Route("{area}/personel/{personelId}/profil")]
        public async Task<IActionResult> Profile(int personelId)
        {
            var personel = await _personelService.GetById(personelId);
            return View(personel);
        }

    }
}
