using KayraExportCase2.Domain.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace KayraExportCase2.Domain.Entities
{
    public class Identity : Entity
    {
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(256)]
        public string PasswordSalt { get; set; } = string.Empty;

    }
}
