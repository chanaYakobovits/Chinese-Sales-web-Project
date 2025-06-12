using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectChinesesActstion.Models
{
    public class OrderMask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Customer { get; set; }

    }
}
