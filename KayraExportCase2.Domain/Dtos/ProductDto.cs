using KayraExportCase2.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Domain.Dtos
{
    public class ProductDto:Dto
    {
        [Required(ErrorMessage ="Başlık girilmesi zornludur.")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage ="Fiyat girilmesi zorunludur")]
        [Range(0.01,double.MaxValue,ErrorMessage ="Fiyat 0'dan büyük olmalıdır.")]
        public decimal Price { get; set; }

        public static implicit operator ProductDto(Entities.Product v)
        {
            return new ProductDto
            {
                Title = v.Title,
                Price = v.Price,
                Id=v.Id
            };
        }
        public static explicit operator Entities.Product(ProductDto v)
        {
            return new Entities.Product
            {
                Title = v.Title,
                Price = v.Price,
                Id=v.Id
            };
        }
    }
}
