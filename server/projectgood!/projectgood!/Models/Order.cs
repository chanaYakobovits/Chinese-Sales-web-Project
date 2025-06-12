using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectChinesesActstion.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int GiftId { get; set; }
        public DateTime DateOrder { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDraft { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("GiftId")]
        public virtual Gift? Gift { get; set; }
    }
}
