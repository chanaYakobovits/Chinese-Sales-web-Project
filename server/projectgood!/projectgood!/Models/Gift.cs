using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectChinesesActstion.Validation;


namespace ProjectChinesesActstion.Models;

public partial class Gift
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "שדה חובה")]
    [MinLength(3, ErrorMessage = "שם המתנה חייב להכיל לפחות 3 תווים.")]
    [MaxLength(50, ErrorMessage = "שם המתנה יכול להכיל לכל היותר 100 תווים.")]
    [UniqueGiftName(ErrorMessage = "שם המתנה כבר קיים במערכת")]
    public string Name { get; set; }

    [Required(ErrorMessage = "שדה חובה")]
    public int DonatorId { get; set; }

    [Required(ErrorMessage = "שדה חובה")]
    [DefaultValue(10)]
    public int Pay { get; set; }

    [Required(ErrorMessage = "שדה חובה")]
    public int CategoryId{ get; set; }

    public string? Image { get; set; }

    [ForeignKey("DonatorId")]
    public virtual Donator? Donator { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }

    //public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

}
