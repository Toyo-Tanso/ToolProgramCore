
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models { 
    public class ReturnModel
        {

        public int? ID { get; set; }

        [Display(Name = "Tool Number")]
        public string? ToolNo { get; set; }

        [Display(Name = "Date Borrowed ")]
        [Required]
        [DataType(DataType.Date)]
        public string? Date_Removed { get; set; }

        [Display(Name = "Promise Return Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Promise_Return_Date { get; set; }


        [Display(Name = "Returned by")]
        [Required]
        public string? Return_EmpNo { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime? Returned_Date { get; set; }

        [Display(Name = "Borrowed From")]
        [Required]
        public string? WC_From { get; set; }

        [Display(Name = "Taken To")]
        [Required]
        public string? WC_To { get; set; }




    }
 }
