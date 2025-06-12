
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjectChinesesActstion.Validation;

namespace ProjectChinesesActstion.Models;

public partial class User
{
    
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "שדה חובה")]
    [MinLength(3, ErrorMessage = "שם חייב להכיל לפחות 3 תווים.")]
    [MaxLength(50, ErrorMessage = "שם יכול להכיל לכל היותר 50 תווים.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "שדה חובה")]
    [EmailAddress(ErrorMessage = "כתובת המייל אינה תקינה")]
    [UniqueEmail(ErrorMessage = "המייל כבר קיים במערכת")]
    public string Email { get; set; }

    [MaxLength(25, ErrorMessage = "הכתובת יכולה להכיל לכל היותר 25 תווים.")]
    public string? Adress { get; set; }

    [Required(ErrorMessage = "שדה חובה")]
    [MinLength(10, ErrorMessage = "מספר הטלפון חייב להכיל לפחות 10 ספרות.")]
    [MaxLength(10, ErrorMessage = "מספר הטלפון יכול להכיל לכל היותר 10 ספרות.")]
    public string Phone { get; set; }


}
