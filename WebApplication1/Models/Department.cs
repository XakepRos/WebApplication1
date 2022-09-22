using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? DepartmentCode { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? DepartmentName { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Required")]
        public string? Remarks { get; set; }

        public DateTime? DepartmentCreatedOn { get; set; }
       
    }
}
