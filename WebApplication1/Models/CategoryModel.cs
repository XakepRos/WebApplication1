using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string CategoryName { get; set; }
        public DateTime? CategoryCreatedOn { get; set; }
    }
}
