using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace ProjectChinesesActstion.Models
{
    public class Donator
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public virtual ICollection<Gift> gifts { get; set; } = new List<Gift>();
    }
}
