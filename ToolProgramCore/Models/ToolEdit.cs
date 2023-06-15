using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models
{
    // TODO: finish implementation
    public class ToolEdit

    {
        [Key]
        public int? ID { get; set; }

        // TODO: remote action that ensures tool starts with "TTU "
        [Required]
        [MinLength(4)]
        [MaxLength(40)]
        [Display(Name = "Tool Name")]
        [Remote(action: "EnsureToolValidation", controller: "ToolEdit")]

        public string? Tool_ID { get; set; }

        [MaxLength(49)]
        [MinLength(2)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public string? Active { get; set; }

    }
}
