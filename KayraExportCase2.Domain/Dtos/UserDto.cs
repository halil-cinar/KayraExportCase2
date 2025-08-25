using KayraExportCase2.Domain.Abstract;
using System.ComponentModel.DataAnnotations;

namespace KayraExportCase2.Domain.Dtos
{
    public class UserDto : Dto
    {
        [Required, MaxLength(128)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(128)]
        public string? FullName { get; set; }

        public string? Password { get; set; }

        public bool IsActive { get; set; } = true;

        public static implicit operator UserDto(Entities.User v)
        {
            return new UserDto
            {
                Id = v.Id,
                Email = v.Email,
                FullName = v.FullName,
                IsActive = v.IsActive
            };
        }

        public static explicit operator Entities.User(UserDto v)
        {
            return new Entities.User
            {
                Id = v.Id,
                Email = v.Email,
                FullName = v.FullName,
                IsActive = v.IsActive
            };
        }
    }
}
