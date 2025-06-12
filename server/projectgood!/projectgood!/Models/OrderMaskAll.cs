using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectChinesesActstion.Models
{
    public class OrderMaskAll
    {
        [Key]
       //public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Customers { get; set; }
        public int Count { get; set; }
    }
}
