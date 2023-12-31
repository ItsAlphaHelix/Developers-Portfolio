﻿using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Services.Dtos.AccountsDtos
{
    public class ApplicationUserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
