using System.ComponentModel.DataAnnotations;
using System.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectChinesesActstion.Models;


    public class Category
    {
        [Key]
        public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(20)]
    public string Name { get; set; }
    }


