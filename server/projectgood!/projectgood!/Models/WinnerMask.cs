using System.ComponentModel.DataAnnotations;

namespace ProjectChinesesActstion.Models
{
    public class WinnerMask
    {
        [Key]
        public int Id { get; set; }
        public string CustName { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string Gift { get; set; }
    }
}
