using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models
{
    public class ToolReturn
    {
        [Key]
        [Display(Name = "Tool Number")]
        //[StringLength(10, MinimumLength = 6, ErrorMessage = "The tool number must be between 6 and 8 Characters")]
        public string? ToolNo { get; set; }

        [Display(Name = "Date Borrowed ")]
        [Required]
        [RegularExpression("[0-9]{2}/[0-9]{2}/[0-9]{4}",
            ErrorMessage = "Must be in correct format: mm/dd/yyyy")]
        public string? D_Remove { get; set; }

        [Display(Name = "Promise Return Date")]
        [Required]
        [DisplayFormat(DataFormatString = " {0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime? P_Return { get; set; }

        [Display(Name = "Work Center")]
        [Required]
        public String? WC { get; set; }

        [Display(Name = "Employee Number")]
        [Required]
        public String? EmpNo { get; set; }


        public List<string>? WCdropDownList { get; set; }
        public List<string>? EmplDropDownList { get; set; }
        public List<string>? ToolNoDropDownList { get; set; }
    }
}
