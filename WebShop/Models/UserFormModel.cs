using System.ComponentModel.DataAnnotations;
namespace WebShop.Models
{
    public record class UserFormModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        public string Role { get; set; }
    }
}
