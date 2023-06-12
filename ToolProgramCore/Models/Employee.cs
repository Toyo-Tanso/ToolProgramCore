using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models
{
    public class Employee
    {
        // TODO set lengths, like max and min
        [Key]
        public string? ID { get; set; }

        // Employee Factors
        [Required]
        [MinLength(2)]
        [MaxLength(24)]
        [Display(Name = "First Name")]
        //[RegularExpression("[A-Z][a-z]*", ErrorMessage = "Must begin with capital letter")]
        [Remote(action: "VerifyCapitalization", controller: "Employee")]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(24)]
        [MinLength(2)]
        [Remote(action: "VerifyCapitalization", controller: "Employee")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        // Internal value
        public string? FullName { get; set; }

        [Key]
        [Required]
        // Check if clock code is in there
        [RegularExpression("B\\d\\d\\d\\d", ErrorMessage = "Must be in the format: B#### ")]
        [Display(Name = "Clock Code")]
        [Remote(action: "VerifyClockCode", controller: "Employee")]
        public string? Clock_Code { get; set; }


    }
}
