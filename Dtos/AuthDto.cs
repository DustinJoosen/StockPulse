﻿using System.ComponentModel.DataAnnotations;

namespace StockPulse.Dtos
{
    public class AuthDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
