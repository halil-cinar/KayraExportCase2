using KayraExportCase2.Domain.Abstract;
using KayraExportCase2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Domain.ListDtos
{
    public class ProductListDto:ListDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }


        public static implicit operator ProductListDto(Product v)
        {
            return new ProductListDto
            {
                Title = v.Title,
                Price = v.Price,
                Id = v.Id,
                CreatedDate=v.CreatedDate,
                UpdatedDate=v.UpdatedDate
            };
        }
    }
}
