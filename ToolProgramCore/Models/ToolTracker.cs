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
        [Remote(action: "VerifyCorrectTool", controller: "ToolTracker")]
        [Display(Name = "Tool Number")]
        public string? ToolNo { get; set; }

        [Display(Name = "Date Borrowed ")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date_Removed { get; set; }

        [Display(Name = "Promise Return Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Promise_Return_Date { get; set; }

        [Display(Name = "Borrowed From")]
        [Required]
        public String? WC_From { get; set; }

        [Display(Name = "Taken To")]
        [Required]
        [Remote(action: "isValidWC", controller: "ToolTracker", AdditionalFields = "WC_From")]
        public String? WC_To { get; set; }

        [Display(Name = "Employee Number")]
        [Required]
        public String? EmpNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Returned_Date { get; set; }

        [Display(Name = "Returned by")]
        public String? Return_EmpNo {get; set;}


        public List<List<string>>? WCdropDownList { get; set; }
        public List<List<string>>? EmplDropDownList { get; set; }
        public List<List<string>>? ToolNoDropDownList { get; set; }

        public string? EmpName { get; set; }

        public int? TotalPages { get; set; }

    }
}
