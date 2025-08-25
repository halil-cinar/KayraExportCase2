using KayraExportCase2.Application.Abstract;
using KayraExportCase2.Application.Result;
using KayraExportCase2.Domain.Dtos;
using KayraExportCase2.Domain.ListDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KayraExportCase2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICacheService _cache;

        public ProductController(IProductService productService, ICacheService cache)
        {
            _productService = productService;
            _cache = cache;
        }

        /// <summary>Ürün oluşturur.</summary>
        /// <remarks>Body içine ürün verisini (ProductDto) sağlayın.</remarks>
        /// <response code="201">Ürün başarıyla oluşturuldu.</response>
        /// <response code="400">Geçersiz istek.</response>
        [HttpPost]
        [ProducesResponseType(typeof(SystemResult<ProductListDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            var result = await _productService.Save(dto);
            if (!result.IsSuccess) return BadRequest(result);
            await InvalidateProductListCache();
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        /// <summary>Ürün günceller.</summary>
        /// <remarks>Body içine güncellenecek ürün verisini (ProductDto) sağlayın.</remarks>
        /// <response code="200">Ürün başarıyla güncellendi.</response>
        /// <response code="400">Geçersiz istek.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(SystemResult<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductDto dto)
        {
            dto.Id = id;
            var result = await _productService.Save(dto);
            if (!result.IsSuccess) return BadRequest(result);
            await InvalidateProductListCache();
            return Ok(result);
        }

        /// <summary>Ürün siler.</summary>
        /// <response code="200">Ürün başarıyla silindi.</response>
        /// <response code="404">Ürün bulunamadı.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(SystemResult<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _productService.Delete(id);
            if (!result.IsSuccess) return NotFound(result);
            await InvalidateProductListCache();
            return Ok(result);
        }

        /// <summary>Tekil ürün getirir.</summary>
        /// <response code="200">Ürün bulundu.</response>
        /// <response code="404">Ürün bulunamadı.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SystemResult<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _productService.Get(id);
            if (!result.IsSuccess || result.Data is null) return NotFound(result);
            return Ok(result);
        }

        /// <summary>Sayfalı ürün listesi döner.</summary>
        /// <remarks>Sonuç Redis Cache ile hızlandırılır.</remarks>
        /// <response code="200">Liste döndü.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PaggingResult<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int count = 20)
        {
            var cacheKey = BuildListKey(page, count);
            var cached = await _cache.GetAsync<PaggingResult<ProductListDto>>(cacheKey);
            if (cached is not null) return Ok(cached);

            var result = await _productService.GetAll(page, count);
            await _cache.SetAsync(cacheKey, result, absoluteTtl: TimeSpan.FromMinutes(5));
            await RegisterCacheKey(cacheKey);
            return Ok(result);
        }

        private string BuildListKey(int page, int count) => $"products:list:p{page}:c{count}";
        private const string CacheKeyRegistry = "products:list:keys";

        private async Task RegisterCacheKey(string key)
        {
            var keys = await _cache.GetAsync<HashSet<string>>(CacheKeyRegistry) ?? new HashSet<string>();
            if (!keys.Contains(key))
            {
                keys.Add(key);
                await _cache.SetAsync(CacheKeyRegistry, keys, absoluteTtl: TimeSpan.FromHours(1));
            }
        }

        private async Task InvalidateProductListCache()
        {
            var keys = await _cache.GetAsync<HashSet<string>>(CacheKeyRegistry) ?? new HashSet<string>();
            foreach (var key in keys)
                await _cache.RemoveAsync(key);
            await _cache.RemoveAsync(CacheKeyRegistry);
        }
    }
}
