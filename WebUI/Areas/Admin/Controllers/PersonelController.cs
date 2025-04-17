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
using System.Dynamic;
using WebUI.Areas.Admin.Models.Personel;

namespace WebUI.Areas.Admin.Controllers
{
    //Bu satır arealar arasında farklı izinlere sahip endpointler oluşturabilmemizi sağlar.
    [Area("Admin")]

    //Bu satır yalnızca yetkilendirilmiş kullanıcıların bu controller sınıfındaki işlemleri yapabilmesini sağlar.
    [Authorize]
    //Bunu daha sonra role bazında olacak şekilde düzenleyeceğiz.


    //Controller sınıflarımız web uygulamamızda kullanacağımız endpointlerin yönlendirilmesini sağlar.
    //Sayfalar, json veriler vb işlemlerin yönetimi ve gösterimi için kullanılır.
    public class PersonelController : Controller
    {
        //Tanımladığımız servis sınıfına erişim sağlamak için bir değişken tanımlıyoruz.
        private readonly IPersonelService dm;
        public PersonelController(IPersonelService _dm)
        {
            dm = _dm;
        }

        //Index Sayfamıza yönlendirme yapar
        public IActionResult Index()
        {
            return View();
        }

        //Departmanlarımızı listelemek için kullanacağımız endpoint Gride getirir
        [Route("{area}/get-personels")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            var data = await dm.GetToGrid(pageRequest);
            return Json(data.Data);
        }

        //Departman ekleme ve güncelleme işlemleri için kullanacağımız endpoint modalı açar
        [Route("{area}/get-personel")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            var model = new PersonelDto();
            if (id.HasValue)
            {

                var result = await dm.GetById(id.Value);
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



                    model.dogumTarihi = entity.dogumTarihi.ToString();
                    model.baslangicTarihi = entity.baslangicTarihi.ToString();
                    model.bitisTarihi = entity.bitisTarihi.ToString();

                    model.fazlaMesaiUygun = entity.fazlaMesaiUygun;

                    model.CalismaTipi = entity.CalismaTipi;
                    model.Cinsiyet = entity.Cinsiyet;
                    model.VardiyaTuru = entity.VardiyaTuru;


                }

                else
                {
                    ModelState.AddModelError("ErrorDetail", "Personel bulunamadı.");
                }

            }
            return PartialView(model);
        }

        //Departman ekleme ve güncelleme işlemleri için kullanacağımız endpoint kaydı yapar
        [Route("{area}/save-personel")]
        [HttpPost]
        public async Task<IActionResult> Edit(PersonelDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Hatalar varsa BadRequest döner
            }
            var message = "resultMessage";
            var id = model.Id;
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
                yillikIzinGunSayisi = int.TryParse(model.yillikIzinGunSayisi, out int yillikIzinGunSayisiInt) ? yillikIzinGunSayisiInt : 227,
                performansNotu = int.TryParse(model.performansNotu, out int performansNotuInt) ? yillikIzinGunSayisiInt : 227,
                sgkSicilNo = int.TryParse(model.sgkSicilNo, out int sgkSicilNoInt) ? sgkSicilNoInt : 227,

                haftalikSaat = decimal.TryParse(model.haftalikSaat, out decimal haftalikSaatInt) ? haftalikSaatInt : 227,
                saatlikUcret = decimal.TryParse(model.saatlikUcret, out decimal saatlikUcretInt) ? saatlikUcretInt : 227,


                dogumTarihi = DateTime.TryParse(model.dogumTarihi, out DateTime dogumTarihiDateTime) ? dogumTarihiDateTime : new DateTime(2003, 4, 11),
                baslangicTarihi = DateTime.TryParse(model.baslangicTarihi, out DateTime baslangicTarihiDateTime) ? baslangicTarihiDateTime : new DateTime(2003, 4, 11),
                bitisTarihi = DateTime.TryParse(model.bitisTarihi, out DateTime bitisTarihiDateTime) ? bitisTarihiDateTime : new DateTime(2003, 4, 11),

                fazlaMesaiUygun = model.fazlaMesaiUygun,

                CalismaTipi = model.CalismaTipi,
                Cinsiyet = model.Cinsiyet,
                VardiyaTuru = model.VardiyaTuru,
            };
            personel.Id = id;
            var result = dm.Edit(personel);
            if (!string.IsNullOrEmpty(result.Result.Message))
            {
                message = result.Result.Message;
            }

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