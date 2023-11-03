using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models
{
    // Corresponds to the Daily_check database
    public class ToolMeasure
    {
        [Key]
        public string? ID { get; set; }

        // Added in DataLibrary
        [Display(Name = "Measured Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? T_Date { get; set; }

        [Display(Name = "Work Center(s)")]
        // Verify that the WC corresponds with the correct Tool
        [Remote(action: "VerifyCorrectWC", controller: "Measure",
            AdditionalFields = "ToolNo,WCdropDownList,ToolNoDropDownList," +
            "ToolLocationsList")]
        [Required]
        [MaxLength(50)]
        public String? WC { get; set; }

        [Required]
        [Display(Name = "Tool Number")]
        [MaxLength(25)]
        public string? ToolNo { get; set; }

        [Display(Name = "Employee Number")]
        [Required]
        [Remote(action: "VerifyEmpNo", controller: "Measure")]
        public String? EmpNo { get; set; }

        [Range(0, 25)]
        [MaxLength(10)]
        [Display(Name = "Standard Size")]
        [Required]

        public double? S_Size { get; set; }

        [Display(Name = "Deviation")]
        [Required]
        [Range(-15, 15)]
        [MaxLength(10)]
        public double? Condition { get; set; }

        public string? EmpName { get; set; }

        // Dropdown data placeholders
        public List<List<string>>? WCdropDownList { get; set; }

        public List<List<string>>? EmplDropDownList { get; set; }

        public List<List<string>>? ToolNoDropDownList { get; set; }

        public List<List<string>>? ToolLocationsList { get; set; }

        public int? TotalPages { get; set; }
    }
}
