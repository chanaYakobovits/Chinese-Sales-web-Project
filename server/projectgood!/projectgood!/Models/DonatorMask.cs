using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectChinesesActstion.Models
{
    public class DonatorMask
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public List<string> Gifts { get; set; }
        //public record donetorDTO (string Name,string Email)
    }
    }
