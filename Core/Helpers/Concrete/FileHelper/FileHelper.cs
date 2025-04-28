using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;



public static class FileHelper
{
    // 📁 Dosya Kaydetme
    public static string DosyaKaydet(IFormFile dosya, string klasorYolu, IWebHostEnvironment env)
    {
        if (dosya == null || dosya.Length == 0)
            return null;

        // Dosya adı: benzersiz olsun diye GUID + uzantı
        var dosyaAdi = Guid.NewGuid().ToString() + Path.GetExtension(dosya.FileName);

        // Kaydedilecek tam fiziksel yol (örnek: wwwroot/uploads/kullanicilar/abc.jpg)
        var fizikselYol = Path.Combine(env.WebRootPath, klasorYolu, dosyaAdi);

        // Klasör yoksa oluştur
        Directory.CreateDirectory(Path.GetDirectoryName(fizikselYol));

        // Dosya yazılır
        using (var stream = new FileStream(fizikselYol, FileMode.Create))
        {
            dosya.CopyTo(stream);
        }

        // Veritabanına kaydedilecek yol: wwwroot’tan itibaren
        return Path.Combine(klasorYolu, dosyaAdi).Replace("\\", "/");
    }

    // 🗑️ Dosya Silme
    public static void DosyaSil(string dosyaYolu, IWebHostEnvironment env)
    {
        if (string.IsNullOrEmpty(dosyaYolu))
            return;

        // Tam yol oluşturulur
        var fizikselYol = Path.Combine(env.WebRootPath, dosyaYolu.Replace("/", "\\"));

        // Dosya varsa silinir
        if (File.Exists(fizikselYol))
        {
            File.Delete(fizikselYol);
        }
    }

    // 🔄 Dosya Güncelleme (eski dosyayı siler, yenisini yükler)
    public static string DosyaGuncelle(IFormFile yeniDosya, string eskiDosyaYolu, string klasorYolu, IWebHostEnvironment env)
    {
        // Önce eski dosyayı sil
        DosyaSil(eskiDosyaYolu, env);

        // Yeni dosyayı kaydet ve yolunu dön
        return DosyaKaydet(yeniDosya, klasorYolu, env);
    }
}