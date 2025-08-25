// KayraExportCase2.API/Controllers/AuthController.cs
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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(SystemResult<UserListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SystemResult<UserListDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new SystemResult<UserListDto>("Geçersiz istek gövdesi."));

            var result = await _auth.SignUp(dto);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(SystemResult<SessionListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SystemResult<SessionListDto>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] IdentityDto identity)
        {
            if (!ModelState.IsValid)
                return Unauthorized(new SystemResult<SessionListDto>("Kimlik doğrulama başarısız."));
               

            var result = await _auth.Login(identity);

            if (result.IsSuccess)
                return Ok(result);

            return Unauthorized(result);
        }
    }
}
