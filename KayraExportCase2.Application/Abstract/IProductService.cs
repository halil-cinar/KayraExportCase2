using KayraExportCase2.Application.Result;
using KayraExportCase2.Domain.Dtos;
using KayraExportCase2.Domain.ListDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Application.Abstract
{
    public interface IProductService
    {
        public Task<SystemResult<ProductListDto>> Save(ProductDto dto);
        public Task<SystemResult<ProductListDto>> Get(int id);
        public Task<SystemResult<ProductListDto>> Delete(int id);
        public Task<PaggingResult<ProductListDto>> GetAll(int page,int count);

    }
}
