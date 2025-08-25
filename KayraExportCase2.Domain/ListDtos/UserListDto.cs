using KayraExportCase2.Domain.Abstract;
using KayraExportCase2.Domain.Entities;

namespace KayraExportCase2.Domain.ListDtos
{
    public class UserListDto : ListDto
    {
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator UserListDto(User v)
        {
            return new UserListDto
            {
                Id = v.Id,
                CreatedDate = v.CreatedDate,
                UpdatedDate = v.UpdatedDate,
                Email = v.Email,
                FullName = v.FullName,
                IsActive = v.IsActive
            };
        }
    }
}
