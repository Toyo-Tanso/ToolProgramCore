﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models
{
    public class WorkCenter
    {
        // TODO set lengths, like max and min
        [Key]
        public string? ID { get; set; }

        // Employee Factors
        [Required]
        [MinLength(2)]
        [MaxLength(10)]
        [Remote(action: "VerifyWC", controller: "WorkCenter")]
        [Display(Name = "WC Name")]
        public string? Name { get; set; }

        [MaxLength(50)]
        [MinLength(2)]
        [Display(Name = "Short Description")]
        // TODO : set required in DB schema
        public string? Description { get; set; }

        // Not a list under DB come in the format of: "(WC, WC)"
        [Display(Name = "All WC Included Under")]
        public List<string>? WCUnder { get; set; }

        // TODO : set required in DB schema
        [Display(Name = "Currently Active")]
        public string? Active { get; set; }

        public List<List<string>>? WCdropDownList { get; set; }

        // This is used when loading partial view
        public List<List<string>> ? Tools { get; set; }

        // This is used when loading partial view
        public bool? IsRemoval { get; set; }

        // used to verify there are no checked out tools for the WC
        public bool? canDelete { get; set; }
    }
}
