using KayraExportCase2.Domain.Abstract;
using KayraExportCase2.Domain.Entities;
using System;

namespace KayraExportCase2.Domain.ListDtos
{
    public class SessionListDto : ListDto
    {
        public int UserId { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
        public bool IsRevoked { get; set; }
        public string? Token { get; set; }

        public static implicit operator SessionListDto(Session v)
        {
            return new SessionListDto
            {
                Id = v.Id,
                CreatedDate = v.CreatedDate,
                UpdatedDate = v.UpdatedDate,
                UserId = v.UserId,
                ExpiresAtUtc = v.ExpiresAtUtc,
                IsRevoked = v.IsRevoked,
                Token = v.Token
            };
        }
    }
}
