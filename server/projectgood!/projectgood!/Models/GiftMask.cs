using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectChinesesActstion.Models
{
    public partial class GiftMask
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Donator { get; set; }

        public int Pay { get; set; }

        public string Category { get; set; }

        public string? Image { get; set; }   
    }
}
