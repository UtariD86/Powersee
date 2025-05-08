using Core.Helpers.Abstract;
using Application.Helpers.Concrete;
using Application.Helpers.Concrete.Filtering;
using Application.Services.Abstract;
using Azure.Core;
using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Bibliography;
using Domain.Dtos;
using Domain.Entities;
using Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using QRCoder;

namespace Application.Services
{

    public class PersonelManager : IPersonelService
    {

        private readonly IUnitOfWork _unitOfWork;


        private readonly FilterHelper _filterHelper;


        public PersonelManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {

            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;


        }


        public async Task<IDataResult<Personel>> Edit(Personel personel)
        {

            try
            {

                if (personel != null && personel.Id != 0)
                {

                    personel.UpdatedDate = DateTime.UtcNow;

                    await _unitOfWork.Personels.UpdateAsync(personel);


                    await _unitOfWork.SaveChangesAsync();


                    return new DataResult<Personel>(
                        data: personel,
                        resultStatus: ResultStatus.Success,
                        message: $"{personel.isim} isimli personel başarıyla güncellendi."
                        );
                }
                else
                {

                    personel.UpdatedDate = DateTime.UtcNow;
                    personel.CreatedDate = DateTime.UtcNow;
                    personel.Code = KodOlustur(personel.soyisim);

                    await _unitOfWork.Personels.AddAsync(personel);


                    await _unitOfWork.SaveChangesAsync();

                    return new DataResult<Personel>(ResultStatus.Success, personel.isim + "Başarıyla Eklendi", personel);
                }
            }
            catch (Exception ex)
            {

                return new DataResult<Personel>(
                   resultStatus: ResultStatus.Error,
                   message: $"Malesef bir sorun oluştu...",
                   exception: ex,
                   data: null
                   );
            }
        }


        public async Task<IResult> Delete(int Id)
        {

            var _personel = _unitOfWork.Personels.Get(Id);

            if (_personel != null)
            {

                _personel.DeletedDate = DateTime.UtcNow;


                _personel.UpdatedDate = DateTime.UtcNow;


                await _unitOfWork.Personels.UpdateAsync(_personel);


                await _unitOfWork.SaveChangesAsync();


                return new Result(ResultStatus.Success, $"{_personel.isim} Başarıyla Silindi");
            }

            return new Result(ResultStatus.Error, "Seçili personel bulunamadı");
        }


        public async Task<IDataResult<IList<Personel>>> GetAll()
        {

            var personels = await _unitOfWork.Personels.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );


            if (personels.Count > -1)
            {

                return new DataResult<IList<Personel>>(ResultStatus.Success, personels);
            }

            return new DataResult<IList<Personel>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
        }


        public async Task<IDataResult<PageResponse<PersonelListDto>>> GetToGrid(PageRequest request)
        {
            try
            {

                Expression<Func<Personel, bool>> predicate = x => !x.DeletedDate.HasValue;
                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<Personel>(request.Filter, deleted: false);


                var personels = await _unitOfWork.Personels.GetAllPersonelsAsync(
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate),
                    request.PageIndex,
                    request.PageSize
                );


                if (personels.Items.Any())
                {
                    return new DataResult<PageResponse<PersonelListDto>>(ResultStatus.Success, personels);
                }


                return new DataResult<PageResponse<PersonelListDto>>(ResultStatus.Error, "Hiç Personel bulunamadı", null);
            }
            catch (Exception ex)
            {

                return new DataResult<PageResponse<PersonelListDto>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }




        public async Task<IDataResult<Personel>> GetById(int id)
        {

            var personel = _unitOfWork.Personels.Get(id);


            if (personel != null)
            {

                return new DataResult<Personel>(ResultStatus.Success, personel);
            }

            return new DataResult<Personel>(ResultStatus.Error, "Personel bulunamadı", null);
        }


        public async Task<IDataResult<Personel>> GetByCode(string code)
        {

            var personel = _unitOfWork.Personels.GetAllAsync(predicate: k => k.Code == code).GetAwaiter().GetResult().FirstOrDefault();


            if (personel != null)
            {

                return new DataResult<Personel>(ResultStatus.Success, personel);
            }

            return new DataResult<Personel>(ResultStatus.Error, "Personel bulunamadı", null);
        }

        private string? KodOlustur(string soyad)
        {
            // İlk 5 karakteri al, eksikse 'X' ile tamamla
            string ilkBes = soyad.ToUpper().PadRight(5, 'X').Replace("Ç", "C").Replace("ç", "c")
        .Replace("Ğ", "G").Replace("ğ", "g")
        .Replace("İ", "I").Replace("ı", "i")
        .Replace("Ö", "O").Replace("ö", "o")
        .Replace("Ş", "S").Replace("ş", "s")
        .Replace("Ü", "U").Replace("ü", "u").Substring(0, 5);

            // Rastgele 5 karakter oluştur
            string rastgeleKisim = RastgeleKodOlustur(5);

            // Birleştir
            string kod = $"{ilkBes}-{rastgeleKisim}";

            return kod;
        }

        public string RastgeleKodOlustur(int uzunluk)
        {
            const string karakterler = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < uzunluk; i++)
            {
                int index = random.Next(karakterler.Length);
                sb.Append(karakterler[index]);
            }

            return sb.ToString();
        }

    }
}
