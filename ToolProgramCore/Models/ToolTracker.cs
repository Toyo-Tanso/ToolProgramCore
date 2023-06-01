using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models
{
    public class ToolTracker
    {
        [Key]
        // TODO: Set max Lengths

        public string? ID { get; set; }

        [Required]
        [Display(Name = "Tool Number")]
        public string? ToolNo { get; set; }

        [Display(Name = "Date Borrowed ")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date_Removed { get; set; }

        [Display(Name = "Promise Return Date")]
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Promise_Return_Date { get; set; }

        [Display(Name = "Borrowed From")]
        [Required]
        public String? WC_From { get; set; }

        [Display(Name = "Taken To")]
        [Required]
        public String? WC_To { get; set; }

        [Display(Name = "Employee Number")]
        [Required]
        public String? EmpNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Returned_Date { get; set; }

        [Display(Name = "Returned by")]
        public String? Return_EmpNo {get; set;}


        public List<string>? WCdropDownList { get; set; }
        public List<string>? EmplDropDownList { get; set; }
        public List<string>? ToolNoDropDownList { get; set; }

        public string? EmpName { get; set; }

        public int? TotalPages { get; set; }

    }
}
