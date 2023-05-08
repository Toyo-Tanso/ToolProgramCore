using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    internal class ToolReturnModel
    {
        [Key]

        public String? toolNumber { get; set; }

        public string? D_Remove { get; set; }

        public String? P_Return { get; set; }

        public String? WC { get; set; }

        public String? EmpNo { get; set; }

        public String? DateReturned { get; set; }

        public String? ID { get; set; }
    }
}
