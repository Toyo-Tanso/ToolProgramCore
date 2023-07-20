using System.ComponentModel.DataAnnotations;

namespace ToolProgramCore.Models
{
    public class PowerUser
    {
        [Key]
        [Display(Name = "User")]
        [MaxLength(10)]
        [MinLength(3)]
        public string? UserName { get; set; }

        [Display(Name = "Changed")]
        [DataType(DataType.Date)]
        public DateTime ? DateChanged { get; set; }

        [Required]
        public bool? Access { get; set; }
        
        [Required]
        public string? UpdatedBy { get; set; }

        [Required]
        [Display(Name = "Elevated")]
        public bool? SuperAdmin { get; set; }

        
        public bool? isAddUserAdmin { get; set; }

    }
}
