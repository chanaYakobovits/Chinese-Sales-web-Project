﻿using System.ComponentModel.DataAnnotations;

namespace ProjectChinesesActstion.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
