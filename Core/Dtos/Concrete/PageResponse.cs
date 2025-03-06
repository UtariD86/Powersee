using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Concrete
{
    /// <summary>
    /// Sayfalı Response dönüş tipi
    /// </summary>
    /// <typeparam name="TItemDto">Listelenecek dto türü</typeparam>
    public class PageResponse<TItemDto>
    {
        /// <summary>
        /// Sayfa numarası
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Sayfa boyutu
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Toplam kayıt sayısı
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Toplam sayfa sayısı
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Önceki sayfa var mı?
        /// </summary>
        public bool HasPrevious { get; set; }

        /// <summary>
        /// Sonraki sayfa var mı?
        /// </summary>
        public bool HasNext { get; set; }

        /// <summary>
        /// Sayfa içeriği
        /// </summary>
        public List<TItemDto> Items { get; set; } = default!;
    }
}
