using KayraExportCase2.Domain.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace KayraExportCase2.Domain.Entities
{
    public class Session : Entity
    {
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(512)]
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAtUtc { get; set; }

        public bool IsRevoked { get; set; } = false;

        [MaxLength(64)]
        public string? IpAddress { get; set; }

        [MaxLength(256)]
        public string? UserAgent { get; set; }
    }
}
