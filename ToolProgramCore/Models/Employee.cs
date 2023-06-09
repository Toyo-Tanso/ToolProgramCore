using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models
{
    public class Employee
    {
        // TODO set lengths, like max and min

        // Employee Factors
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        // Internal value
        public string? FullName { get; set; }

        [Display(Name = "Clock Code")]
        public string? Clock_Code { get; set; }


    }
}
