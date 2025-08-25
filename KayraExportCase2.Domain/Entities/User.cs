using KayraExportCase2.Domain.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace KayraExportCase2.Domain.Entities
{
    public class User : Entity
    {
        [Required, MaxLength(128)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(128)]
        public string? FullName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
