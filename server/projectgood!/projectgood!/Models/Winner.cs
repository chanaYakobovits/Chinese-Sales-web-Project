using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectChinesesActstion.Models
{
    public class Winner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GiftId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("GiftId")]
        public virtual Gift? Gift { get; set; }
    }
}
