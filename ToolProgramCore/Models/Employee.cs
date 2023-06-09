using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models
{
    public class Employee
    {
        // TODO set lengths, like max and min

        // Employee Factors
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        // Internal value
        public string? FullName { get; set; }

        public string? Clock_Code { get; set; }


    }
}
