using KayraExportCase2.Application.Abstract;
using KayraExportCase2.Application.Result;
using KayraExportCase2.DataAccess.Abstract;
using KayraExportCase2.Domain.Dtos;
using KayraExportCase2.Domain.Entities;
using KayraExportCase2.Domain.ListDtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        public ProductService(IServiceProvider serviceProvider)
        {
            _productRepository = (IRepository<Product>)serviceProvider.GetRequiredService(typeof(IRepository<Product>));
        }
        public async Task<SystemResult<ProductListDto>> Delete(int id)
        {
            var result = new SystemResult<ProductListDto>();

            var entity = await _productRepository.Delete(x => x.Id == id);
            result.Data = entity ?? throw new UserException("Kayıt bulunamadı");

            return result;
        }

        public async Task<SystemResult<ProductListDto>> Get(int id)
        {
            var result = new SystemResult<ProductListDto>();

            var entity = await _productRepository.Get(x => x.Id == id);
            result.Data = entity ?? throw new UserException("Kayıt bulunamadı");

            return result;
        }

        public async Task<PaggingResult<ProductListDto>> GetAll(int page, int count)
        {
            var result = new PaggingResult<ProductListDto>();

            if (page < 0 || count <= 0)
                throw new UserException("Sayfa numarası 0'dan büyük, sayfa başı kayıt sayısı 0'dan büyük olmalıdır.");
            var totalCount = await _productRepository.Count();
            var entities = await _productRepository.GetAll();
            result.Data = entities.Skip((page - 1) * count).Take(count).Select(x => (ProductListDto)x).ToList();
            result.AllCount = totalCount;
            result.CurrentPage = page;
            result.ItemCount = count;
            result.AllPageCount = (int)Math.Ceiling((double)totalCount / count);

            return result;
        }

        public async Task<SystemResult<ProductListDto>> Save(ProductDto dto)
        {
            var result = new SystemResult<ProductListDto>();

            var oldEntity = await _productRepository.Get(x => x.Id == dto.Id);
            if(dto.Id>0 && oldEntity == null)
            {
                throw new UserException("Ürün Bulunamadı");
            }
            if (oldEntity == null)
            {
                var newEntity = (Product)dto;
                var addedEntity = await _productRepository.Add(newEntity);
                result.Data = addedEntity;
            }
            else
            {
                var updatedEntity = await _productRepository.Update(oldEntity);
                result.Data = updatedEntity;
            }

            return result;
        }
    }
}
