using KayraExportCase2.Application.Abstract;
using KayraExportCase2.Application.Result;
using KayraExportCase2.DataAccess.Abstract;
using KayraExportCase2.Domain;
using KayraExportCase2.Domain.Dtos;
using KayraExportCase2.Domain.Entities;
using KayraExportCase2.Domain.ListDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Identity> _identityRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IServiceProvider serviceProvider)
        {
            _identityRepository = (IRepository<Identity>)serviceProvider.GetRequiredService(typeof(IRepository<Identity>));
            _userRepository = (IRepository<User>)serviceProvider.GetRequiredService(typeof(IRepository<User>));
            _sessionRepository = (IRepository<Session>)serviceProvider.GetRequiredService(typeof(IRepository<Session>));
            _httpContextAccessor = (IHttpContextAccessor)serviceProvider.GetRequiredService(typeof(IHttpContextAccessor));
        }

        public async Task<SystemResult<SessionListDto>> Login(IdentityDto ıdentity)
        {
            var result = new SystemResult<SessionListDto>();
            var user = await _userRepository.Get(x => x.Email == ıdentity.Email) ?? throw new UserException("Kullanıcı bulunamadı.");
            var identityData = await _identityRepository.Get(x => x.UserId == user.Id) ?? throw new UserException("Kullanıcı kimlik bilgileri bulunamadı.");
            var hash = GenerateMD5Hash(ıdentity.Password, identityData.PasswordSalt);
            if (hash != identityData.PasswordHash)
            {
                throw new UserException("Hatalı şifre.");
            }

            var session = new Session
            {
                UserId = user.Id,
                Token = GenerateToken(user.Id.ToString(),user.Email,120),
                ExpiresAtUtc = DateTime.UtcNow.AddHours(2),
                IsRevoked=false,
                IpAddress=_httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                UserAgent=_httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString()
            };
            await _sessionRepository.Add(session);
            result.Data = session;
            return result;
        }

        public async Task<SystemResult<UserListDto>> SignUp(UserDto dto)
        {
            var result = new SystemResult<UserListDto>();
            var oldUser = await _userRepository.Get(x => x.Email == dto.Email);
            if (oldUser != null)
            {
                throw new UserException("Bu email ile kayıtlı bir kullanıcı bulunmaktadır.");
            }
            if(string.IsNullOrEmpty(dto.Password) || dto.Password.Length<6)
            {
                throw new UserException("Şifre en az 6 karakter olmalıdır.");
            }
            var user = (User)dto;
            await _userRepository.Add(user);
            var salt = Guid.NewGuid().ToString();
            var identity = new Identity
            {
                UserId = user.Id,
                PasswordSalt = salt,
                PasswordHash = GenerateMD5Hash(dto.Password, salt)
            };
            await _identityRepository.Add(identity);
            result.Data = (UserListDto)user;
            return result;
        }

        private string GenerateToken(string userId, string username, int expireMinutes = 60)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(AppSettings.instance?.JWTSecret??"");

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateMD5Hash(string password, string salt)
        {
            using (var md5 = MD5.Create())
            {
                string combined = password + salt;
                byte[] inputBytes = Encoding.UTF8.GetBytes(combined);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
    }
}
