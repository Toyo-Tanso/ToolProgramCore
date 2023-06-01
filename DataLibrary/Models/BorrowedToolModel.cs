using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class BorrowedToolModel
    {
        [Key]

        public String? ID { get; set; }

        public String? ToolNo { get; set; }

        public string? Date_Removed { get; set; }

        public String? Promise_Return_Date { get; set; }

        public String? WC_From { get; set; }

        public String? WC_To { get; set; }

        public String? EmpNo { get; set; }

        public String? Returned_Date { get; set; }

        public String? Return_EmpNo { get; set; }

    }
}
