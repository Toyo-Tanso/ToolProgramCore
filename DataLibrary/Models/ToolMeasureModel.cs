using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    internal class ToolMeasureModel
    {
        [Key]

        public String ? ID { get; set; }

        public String ? T_Date { get; set; }

        public String ? WC { get; set; }

        public string? ToolNo { get; set; }

        public string ? S_Size { get; set; }        

        public String ? EmpNo { get; set; }

        public String ? Condition { get; set; }

        
    }
}
