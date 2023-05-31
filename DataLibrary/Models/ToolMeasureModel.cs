using System.ComponentModel.DataAnnotations;

namespace DataLibrary.Models
{
    public class ToolMeasureModel
    {
        [Key]

        public String? ID { get; set; }

        public String? T_Date { get; set; }

        public String? WC { get; set; }

        public string? ToolNo { get; set; }

        public string? S_Size { get; set; }

        public String? EmpNo { get; set; }

        public String? Condition { get; set; }


    }
}
