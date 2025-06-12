using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectChinesesActstion.Models
{
    public class OrderMaskGift
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        public int Pay { get; set; }
        public bool IsDart { get; set; }
        public int? Count { get; set; }

    }
}
