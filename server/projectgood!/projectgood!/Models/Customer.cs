using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectChinesesActstion.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        public string Password { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        //public virtual ICollection<Gift> gifts { get; set; } = new List<Gift>();
    }
}
