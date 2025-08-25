using KayraExportCase2.Domain.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace KayraExportCase2.Domain.Dtos
{
    public class SessionDto : Dto
    {
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(512)]
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAtUtc { get; set; }

        public bool IsRevoked { get; set; }

        [MaxLength(64)]
        public string? IpAddress { get; set; }

        [MaxLength(256)]
        public string? UserAgent { get; set; }

        public static implicit operator SessionDto(Entities.Session v)
        {
            return new SessionDto
            {
                Id = v.Id,
                UserId = v.UserId,
                Token = v.Token,
                ExpiresAtUtc = v.ExpiresAtUtc,
                IsRevoked = v.IsRevoked,
                IpAddress = v.IpAddress,
                UserAgent = v.UserAgent
            };
        }

        public static explicit operator Entities.Session(SessionDto v)
        {
            return new Entities.Session
            {
                Id = v.Id,
                UserId = v.UserId,
                Token = v.Token,
                ExpiresAtUtc = v.ExpiresAtUtc,
                IsRevoked = v.IsRevoked,
                IpAddress = v.IpAddress,
                UserAgent = v.UserAgent
            };
        }
    }
}
