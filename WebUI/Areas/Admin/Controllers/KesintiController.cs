using Application.Services.Abstract; // IKesintiService, IPersonelService için
using Core.Dtos.Concrete; // PageRequest için
using Core.Enums; // ResultStatus için
using Domain.Entities; // Kesinti entity'si için
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectListItem için
using System.Dynamic;
using WebUI.Areas.Admin.Models.Kesinti; // KesintiDto için (Adım 19'da oluşturulacak)

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")] // Alanı belirtiyoruz
    [Authorize] // Yetkilendirme gerekli
    public class KesintiController : Controller
    {
        // Gerekli servisleri inject etmek için değişkenler
        private readonly IKesintiService _kesintiService;
        private readonly IPersonelService _personelService;
        // private readonly IPlanlananVardiyaSnapshotService _snapshotService; // Snapshot servisi varsa eklenmeli

        // Constructor ile servisleri inject ediyoruz
        public KesintiController(IKesintiService kesintiService, IPersonelService personelService /*, IPlanlananVardiyaSnapshotService snapshotService */)
        {
            _kesintiService = kesintiService;
            _personelService = personelService;
            // _snapshotService = snapshotService; // Snapshot servisi varsa atanmalı
        }

        // Index sayfası (Grid'i içeren view)
        public IActionResult Index()
        {
            return View();
        }

        // DevExtreme Grid için veri getiren endpoint
        [Route("{area}/get-kesintiler")] // Route Kesinti için güncellendi
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PageRequest pageRequest)
        {
            // Kesinti servisini çağırıyoruz
            var result = await _kesintiService.GetToGrid(pageRequest);
            // Sadece datayı JSON olarak döndürüyoruz (SubeController'daki gibi)
            return Json(result.Data);
        }

        // Ekleme/Düzenleme modal'ını getiren endpoint
        [Route("{area}/get-kesinti")] // Route Kesinti için güncellendi
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] int? id)
        {
            // KesintiDto örneği oluşturuyoruz (Adım 19'da tanımlanacak)
            var model = new KesintiDto();

            // --- YENİ: Dropdown Listelerini Doldurma ---
            // Personel Listesi
            var personelResult = await _personelService.GetAll(); // Tüm aktif personelleri al
            if (personelResult.ResultStatus == ResultStatus.Success)
            {
                model.PersonelListesi = personelResult.Data
                                            .Select(p => new SelectListItem { Text = $"{p.isim} {p.soyisim}", Value = p.Id.ToString() })
                                            .ToList();
            }
            else
            {
                model.PersonelListesi = new List<SelectListItem>(); // Hata durumunda boş liste
                // Opsiyonel: Hata loglanabilir veya kullanıcıya mesaj gösterilebilir
            }

            // Planlanan Vardiya Snapshot Listesi (Örnek - Snapshot servisi ve GetAll metodu varsayıldı)
            // var snapshotResult = await _snapshotService.GetAll(); // Tüm snapshotları al (veya filtrelenmiş)
            // if (snapshotResult.ResultStatus == ResultStatus.Success)
            // {
            //     model.SnapshotListesi = snapshotResult.Data
            //                                 .Select(s => new SelectListItem { Text = $"Snapshot {s.Id} - {s.BaslangicZamani:dd.MM.yyyy HH:mm}", Value = s.Id.ToString() }) // Örnek metin
            //                                 .ToList();
            // }
            // else
            // {
            model.SnapshotListesi = new List<SelectListItem>(); // Şimdilik boş liste
            // }
            // --- Dropdown Doldurma Sonu ---


            if (id.HasValue && id.Value > 0) // ID varsa düzenleme modundayız
            {
                var result = await _kesintiService.GetById(id.Value);
                var entity = result.Data;

                if (entity != null && result.ResultStatus == ResultStatus.Success)
                {
                    // Entity'den DTO'ya mapliyoruz
                    model.Id = entity.Id;
                    model.PersonelId = entity.PersonelId;
                    model.PlanlanmisVardiyaSnapshotId = entity.PlanlanmisVardiyaSnapshotId;
                    model.UygulanacakTarih = entity.UygulanacakTarih;
                    model.CezaMiktari = entity.CezaMiktari;
                    // Diğer alanlar varsa buraya eklenebilir
                }
                else
                {
                    // SubeController'daki gibi hata yönetimi
                    ModelState.AddModelError("ErrorDetail", result.Message ?? "Kesinti bulunamadı.");
                }
            }
            // ID yoksa veya kayıt bulunamazsa, boş DTO (ama dropdown listeleri dolu) ile PartialView döndürülür
            return PartialView("Edit", model); // Edit.cshtml'i model ile döndür
        }

        // Ekleme/Güncelleme işlemini yapan endpoint
        [Route("{area}/save-kesinti")] // Route Kesinti için güncellendi
        [HttpPost]
        public async Task<IActionResult> Edit(KesintiDto model) // Formdan KesintiDto alıyoruz
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Hatalar varsa BadRequest döner
            }

            // DTO'dan Entity'ye mapliyoruz
            var kesinti = new Kesinti()
            {
                Id = model.Id,
                PersonelId = model.PersonelId,
                //PlanlanmisVardiyaSnapshotId = model.PlanlanmisVardiyaSnapshotId,
                PlanlanmisVardiyaSnapshotId = 1, //şimdilik
                UygulanacakTarih = model.UygulanacakTarih,
                CezaMiktari = model.CezaMiktari,
                // CreatedDate ve UpdatedDate Manager içinde ayarlanıyor
            };

            // Servis katmanındaki Edit metodunu çağırıyoruz (Add/Update yapar)
            var result = await _kesintiService.Edit(kesinti);

            // Sonucu JSON olarak döndürüyoruz (Başarı durumu ve mesaj)
            return Json(result.Message);
        }

        // Silme işlemini yapan endpoint
        [Route("{area}/delete-kesinti")] // Route Kesinti için güncellendi
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int? id)
        {
            string message = "Silme işlemi sırasında bir hata oluştu."; // Varsayılan hata mesajı
            bool success = false;

            if (id.HasValue && id > 0)
            {
                var result = await _kesintiService.Delete(id.Value);
                message = result.Message; // Servisten gelen mesajı al
                success = result.ResultStatus == ResultStatus.Success; // Başarı durumunu al
            }
            else
            {
                message = "Geçersiz ID.";
            }
            // Sonucu JSON olarak döndürüyoruz
            return Json(new { success = success, message = message });
        }
    }
}

