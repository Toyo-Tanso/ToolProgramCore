using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Xml.Linq;

namespace ToolProgramCore.Models
{
    public class ToolMeasure
    {

        // TODO: check value length to ensure it aligns with the Database  

        [Key]
        public string ? ID { get; set; }

        // Added in DataLibrary
        [Display(Name = "Measured Date")]
        [Required]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? T_Date { get; set; }

        // TODO add verification that the tool number is in the WC group
        [Display(Name = "Work Center(s)")]
        [Required]
        public String? WC { get; set; }

        [Required]
        [Display(Name = "Tool Number")]
        public string? ToolNo { get; set; }

        [Display(Name = "Employee Number")]
        [Required]
        [Remote(action: "VerifyEmpNo", controller: "Measure")]
        public String? EmpNo { get; set; }

        [Range(0.5, 25)]
        [Display(Name = "Standard Size")]
        [Required]
        
        public double? S_Size { get; set; }


        [Display(Name = "Deviation")]
        [Required]
        [Range(-15, 15)]
        public double? Condition { get; set; }

        // TODO: replace index view with this after changing it
        public string ? EmpName { get; set; }

        public List<List<string>>? WCdropDownList { get; set; }

        public List<List<string>> ? EmplDropDownList { get; set; }

        public List<List<string>>? ToolNoDropDownList { get; set; }
    }
}
