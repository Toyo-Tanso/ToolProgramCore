using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Xml.Linq;

namespace ToolProgramCore.Models
{
    public class ToolMeasure
    {
        [Key]
        public string ? ID { get; set; }

        // Added in DataLibrary
        [Display(Name = "Measured Date")]
        [Required]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? T_Date { get; set; }

        [Display(Name = "Work Center(s)")]
        [Required]
        public String? WC { get; set; }

        [Required]
        [Display(Name = "Tool Number")]
        public string? ToolNo { get; set; }

        [Display(Name = "Employee Number")]
        [Required]
        public String? EmpNo { get; set; }

        [Display(Name = "Standard Size")]
        [Required]
        public String? S_Size { get; set; }


        [Display(Name = "Standard Deviation")]
        [Required]
        public String? Condition { get; set; }

        public List<string>? WCdropDownList { get; set; }
        public List<string>? EmplDropDownList { get; set; }
        public List<string>? ToolNoDropDownList { get; set; }
    }
}
