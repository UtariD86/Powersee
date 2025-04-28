using Application.Helpers.Concrete.Filtering;
using Application.Services.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using WebUI.Areas.Admin.Models.Department;
using WebUI.Areas.Admin.Models.Personel;
using WebUI.Areas.Admin.Models.Position;
using WebUI.Areas.Admin.Models.Sube;

namespace WebUI.Areas.Admin.Controllers
{
  [Area("Admin")]
 [Authorize]
public class PersonelController : Controller
    {
        private readonly IPersonelService _personelService;
        private readonly IDepartmentService _departmentService;
        private readonly ISubeService _subeService;
        private readonly IPositionService _positionService;
        private readonly IWebHostEnvironment _env;
        public PersonelController(IPersonelService personelService, IDepartmentService departmentService, ISubeService subeService, IPositionService positionService, IWebHostEnvironment env)
        {
            _env = env;
            _personelService = personelService;
            _departmentService = departmentService;
            _subeService = subeService;
            _positionService = positionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{area}/get-personels")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await _personelService.GetToGrid(pageRequest);
            return Json(data.Data);
        }

        [Route("{area}/get-personel")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            var model = new PersonelDto();

            if (id.HasValue)
            {
                var result = await _personelService.GetById(id.Value);
                var entity = result.Data;

                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    model.Id = entity.Id;
                    model.isim = entity.isim;
                    model.soyisim = entity.soyisim;
                    model.adres = entity.adres;
                    model.telefonNumarasi1 = entity.telefonNumarasi1;
                    model.telefonNumarasi2 = entity.telefonNumarasi2;
                    model.tcKimlik = entity.tcKimlik;
                    model.bankaHesapNo = entity.bankaHesapNo;
                    model.vergiNo = entity.vergiNo;
                    model.vergiDairesiAdi = entity.vergiDairesiAdi;
                    model.aciklama = entity.aciklama;
                    model.departmanId = entity.departmanId.ToString();
                    model.pozisyonId = entity.pozisyonId.ToString();
                    model.subeId = entity.subeId.ToString();
                    model.yillikIzinGunSayisi = entity.yillikIzinGunSayisi.ToString();
                    model.performansNotu = entity.performansNotu.ToString();
                    model.sgkSicilNo = entity.sgkSicilNo.ToString();
                    model.haftalikSaat = entity.haftalikSaat.ToString();
                    model.saatlikUcret = entity.saatlikUcret.ToString();
                    model.dogumTarihi = entity.dogumTarihi;
                    model.baslangicTarihi = entity.baslangicTarihi;
                    model.bitisTarihi = entity.bitisTarihi;
                    model.fazlaMesaiUygun = entity.fazlaMesaiUygun;
                    model.CalismaTipi = entity.CalismaTipi;
                    model.Cinsiyet = entity.Cinsiyet;
                    model.VardiyaTuru = entity.VardiyaTuru;
                    model.profilFotografiUrl = entity.profilFotografiUrl;
                }
                else
                {
                    ModelState.AddModelError("ErrorDetail", "Personel bulunamadı.");
                }
            }

            // SelectList'leri yükle
            var departmentResult = await _departmentService.GetAll();
            var subeResult = await _subeService.GetAll();
            var positionResult = await _positionService.GetAll();

            model.DepartmentSel = new SelectList(departmentResult?.Data, "Id", "Name");
            model.SubeSel = new SelectList(subeResult?.Data, "Id", "Subeisim");
            model.PozisyonSel = new SelectList(positionResult?.Data, "Id", "Name");

            return PartialView(model);
        }

        [Route("{area}/save-personel")]
        [HttpPost]
        public async Task<IActionResult> Edit(PersonelDto model)

        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Profil fotoğrafı yüklenmişse dosyayı kaydet
            if (model.profilFotografi != null)
            {
                // Eski fotoğraf varsa sil
                if (!string.IsNullOrEmpty(model.profilFotografiUrl))
                {
                    FileHelper.DosyaSil(model.profilFotografiUrl, _env);
                }

                var yeniYol = FileHelper.DosyaKaydet(model.profilFotografi, "uploads/profilePhotos", _env);
                model.profilFotografiUrl = yeniYol;
            }
            //yüklemiş ama kaydetmedi başka bir alanın eksikliğinden mi
            //bilmiyorumda tam nerde değişiklik yaptın kanka orayı kaçırdım
            //başka neresi eksik olabilir
            // Entity’ye verileri aktar
            var personel = new Personel()
            {
                Id = model.Id,
                isim = model.isim,
                soyisim = model.soyisim,
                adres = model.adres,
                telefonNumarasi1 = model.telefonNumarasi1,
                telefonNumarasi2 = model.telefonNumarasi2,
                tcKimlik = model.tcKimlik,
                bankaHesapNo = model.bankaHesapNo,
                vergiNo = model.vergiNo,
                vergiDairesiAdi = model.vergiDairesiAdi,
                aciklama = model.aciklama,
                departmanId = int.TryParse(model.departmanId, out int departmanIdInt) ? departmanIdInt : 227,
                pozisyonId = int.TryParse(model.pozisyonId, out int pozisyonIdInt) ? pozisyonIdInt : 227,
                subeId = int.TryParse(model.subeId, out int subeIdInt) ? subeIdInt : 227,
                yillikIzinGunSayisi = int.TryParse(model.yillikIzinGunSayisi, out int yillikIzinGunSayisiInt) ? yillikIzinGunSayisiInt : 0,
                performansNotu = int.TryParse(model.performansNotu, out int performansNotuInt) ? performansNotuInt : 0,
                sgkSicilNo = int.TryParse(model.sgkSicilNo, out int sgkSicilNoInt) ? sgkSicilNoInt : 0,
                haftalikSaat = decimal.TryParse(model.haftalikSaat, out decimal haftalikSaatDec) ? haftalikSaatDec : 0,
                saatlikUcret = decimal.TryParse(model.saatlikUcret, out decimal saatlikUcretDec) ? saatlikUcretDec : 0,
                dogumTarihi = model.dogumTarihi,
                baslangicTarihi = model.baslangicTarihi,
                bitisTarihi = model.bitisTarihi,
                fazlaMesaiUygun = model.fazlaMesaiUygun,
                CalismaTipi = model.CalismaTipi,
                Cinsiyet = model.Cinsiyet,
                VardiyaTuru = model.VardiyaTuru,
                profilFotografiUrl = model.profilFotografiUrl
            };

            var result = _personelService.Edit(personel);

            string message = result?.Result?.Message ?? "İşlem tamamlandı.";
            return Json(message);
        }

        //Departman silme işlemleri için kullanacağımız endpoint
        [Route("{area}/delete-personel")]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int? id)
        {
            string message = string.Empty;
            if (id.HasValue && id > 0)
            {
                var result = await _personelService.Delete(id.Value);
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

//using Application.Helpers.Concrete.Filtering;
//using Application.Services.Abstract;
//using Core.Dtos.Concrete;
//using Core.Enums;
//using DocumentFormat.OpenXml.Office2010.Excel;
//using DocumentFormat.OpenXml.Spreadsheet;
//using Domain.Entities;
//using Domain.Enums;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Dynamic;
//using WebUI.Areas.Admin.Models.Department;
//using WebUI.Areas.Admin.Models.Personel;
//using WebUI.Areas.Admin.Models.Position;
//using WebUI.Areas.Admin.Models.Sube;

//namespace WebUI.Areas.Admin.Controllers
//{
//    //Bu satır arealar arasında farklı izinlere sahip endpointler oluşturabilmemizi sağlar.
//    [Area("Admin")]

//    //Bu satır yalnızca yetkilendirilmiş kullanıcıların bu controller sınıfındaki işlemleri yapabilmesini sağlar.
//    [Authorize]
//    //Bunu daha sonra role bazında olacak şekilde düzenleyeceğiz.


//    //Controller sınıflarımız web uygulamamızda kullanacağımız endpointlerin yönlendirilmesini sağlar.
//    //Sayfalar, json veriler vb işlemlerin yönetimi ve gösterimi için kullanılır.
//    public class PersonelController : Controller
//    {
//        //Tanımladığımız servis sınıfına erişim sağlamak için bir değişken tanımlıyoruz.
//        private readonly IPersonelService _personelService;
//        private readonly IDepartmentService _departmentService;
//        private readonly ISubeService _subeService;
//        private readonly IPositionService _positionService;
//        private readonly IWebHostEnvironment _env;
//        public PersonelController(IPersonelService personelService, IDepartmentService departmentService, ISubeService subeService, IPositionService positionService, IWebHostEnvironment env)
//        {
//            _env = env;
//            _personelService = personelService;
//            _departmentService = departmentService;
//            _subeService = subeService;
//            _positionService = positionService;
//        }

//        //Index Sayfamıza yönlendirme yapar
//        public IActionResult Index()
//        {
//            return View();
//        }

//        //Departmanlarımızı listelemek için kullanacağımız endpoint Gride getirir
//        [Route("{area}/get-personels")]
//        [HttpPost]
//        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
//        {
//            var data = await _personelService.GetToGrid(pageRequest);
//            return Json(data.Data);
//        }

//        //Departman ekleme ve güncelleme işlemleri için kullanacağımız endpoint modalı açar
//        [Route("{area}/get-personel")]
//        [HttpPost]
//        public async Task<IActionResult> Edit([FromBody] int? id)
//        {




//            var model = new PersonelDto();
//            if (id.HasValue)
//            {

//                if (model.profilFotografi != null)
//                {
//                    var dosyaYolu = FileHelper.DosyaKaydet(model.profilFotografi, "uploads/profilePhotos", _env);
//                    model.profilFotografiUrl = dosyaYolu; // DTO’ya/resme yol ekleniyor
//                }



//                var result = await _personelService.GetById(id.Value);
//                var entity = result.Data;
//                if (entity != null && result.ResultStatus == ResultStatus.Success)
//                {
//                    model.Id = entity.Id;
//                    model.isim = entity.isim;
//                    model.soyisim = entity.soyisim;
//                    model.adres = entity.adres;
//                    model.telefonNumarasi1 = entity.telefonNumarasi1;
//                    model.telefonNumarasi2 = entity.telefonNumarasi2;
//                    model.tcKimlik = entity.tcKimlik;
//                    model.bankaHesapNo = entity.bankaHesapNo;
//                    model.vergiNo = entity.vergiNo;
//                    model.vergiDairesiAdi = entity.vergiDairesiAdi;
//                    model.aciklama = entity.aciklama;



//                    model.departmanId = entity.departmanId.ToString();
//                    model.pozisyonId = entity.pozisyonId.ToString();
//                    model.subeId = entity.subeId.ToString();
//                    model.yillikIzinGunSayisi = entity.yillikIzinGunSayisi.ToString();
//                    model.performansNotu = entity.performansNotu.ToString();
//                    model.sgkSicilNo = entity.sgkSicilNo.ToString();

//                    model.haftalikSaat = entity.haftalikSaat.ToString();
//                    model.saatlikUcret = entity.saatlikUcret.ToString();



//                    model.dogumTarihi = entity.dogumTarihi;
//                    model.baslangicTarihi = entity.baslangicTarihi;
//                    model.bitisTarihi = entity.bitisTarihi;

//                    model.fazlaMesaiUygun = entity.fazlaMesaiUygun;

//                    model.CalismaTipi = entity.CalismaTipi;
//                    model.Cinsiyet = entity.Cinsiyet;
//                    model.VardiyaTuru = entity.VardiyaTuru;


//                }

//                else
//                {
//                    ModelState.AddModelError("ErrorDetail", "Personel bulunamadı.");
//                }

//            }

//            //SelectListler için gerekli verileri alıyoruz
//            var departmentResult = await _departmentService.GetAll();
//            var subeResult = await _subeService.GetAll();
//            var positionResult = await _positionService.GetAll();

//            model.DepartmentSel = new SelectList(departmentResult?.Data, "Id", "Name");
//            model.SubeSel = new SelectList(subeResult?.Data, "Id", "Subeisim");
//            model.PozisyonSel = new SelectList(positionResult?.Data, "Id", "Name");



//            return PartialView(model);
//        }

//        //Departman ekleme ve güncelleme işlemleri için kullanacağımız endpoint kaydı yapar
//        [Route("{area}/save-personel")]
//        [HttpPost]
//        public async Task<IActionResult> Edit(PersonelDto model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState); // Hatalar varsa BadRequest döner
//            }



//            var message = "resultMessage";
//            var id = model.Id;
//            var personel = new Personel()

//            {
//                Id = model.Id,
//                isim = model.isim,
//                soyisim = model.soyisim,
//                adres = model.adres,
//                telefonNumarasi1 = model.telefonNumarasi1,
//                telefonNumarasi2 = model.telefonNumarasi2,
//                tcKimlik = model.tcKimlik,
//                bankaHesapNo = model.bankaHesapNo,
//                vergiNo = model.vergiNo,
//                vergiDairesiAdi = model.vergiDairesiAdi,
//                aciklama = model.aciklama,


//                departmanId = int.TryParse(model.departmanId, out int departmanIdInt) ? departmanIdInt : 227,
//                pozisyonId = int.TryParse(model.pozisyonId, out int pozisyonIdInt) ? pozisyonIdInt : 227,
//                subeId = int.TryParse(model.subeId, out int subeIdInt) ? subeIdInt : 227,
//                yillikIzinGunSayisi = int.TryParse(model.yillikIzinGunSayisi, out int yillikIzinGunSayisiInt) ? yillikIzinGunSayisiInt : 227,
//                performansNotu = int.TryParse(model.performansNotu, out int performansNotuInt) ? performansNotuInt : 227,
//                sgkSicilNo = int.TryParse(model.sgkSicilNo, out int sgkSicilNoInt) ? sgkSicilNoInt : 227,

//                haftalikSaat = decimal.TryParse(model.haftalikSaat, out decimal haftalikSaatInt) ? haftalikSaatInt : 227,
//                saatlikUcret = decimal.TryParse(model.saatlikUcret, out decimal saatlikUcretInt) ? saatlikUcretInt : 227,


//                dogumTarihi = model.dogumTarihi,
//                baslangicTarihi = model.baslangicTarihi,
//                bitisTarihi = model.bitisTarihi,

//                fazlaMesaiUygun = model.fazlaMesaiUygun,

//                CalismaTipi = model.CalismaTipi,
//                Cinsiyet = model.Cinsiyet,
//                VardiyaTuru = model.VardiyaTuru,
//            };
//            personel.Id = id;
//            var result = _personelService.Edit(personel);
//            if (!string.IsNullOrEmpty(result.Result.Message))
//            {
//                message = result.Result.Message;
//            }

//            return Json(message);
//        }

//        //Departman silme işlemleri için kullanacağımız endpoint
//        [Route("{area}/delete-personel")]
//        [HttpPost]
//        public async Task<IActionResult> Delete([FromBody] int? id)
//        {
//            string message = string.Empty;
//            if (id.HasValue && id > 0)
//            {
//                var result = await _personelService.Delete(id.Value);
//                message = result.Message;
//            }
//            else
//            {
//                ModelState.AddModelError("ErrorDetail", "Silinirken bir hata oluştu!");
//            }
//            return Json(message);
//        }
//    }
//}