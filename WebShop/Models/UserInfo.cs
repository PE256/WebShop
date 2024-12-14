using System.ComponentModel.DataAnnotations;
namespace WebShop.Models
{
    public class UserInfo
    {
        public string Id{ get; set; }

        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Coins")]
        public decimal Coins { get; set; }

        public UserInfo(string id, string name, decimal coins)
        {
            Id = id;
            Name = name;
            Coins = coins;
        }
    }

    //public class User
    //{
    //    public int Id { get; set; }
    //    [Required]
    //    [StringLength(50)]
    //    [Display(Name = "Name")]
    //    public string Name { get; set; }
    //    [Required]
    //    [StringLength(50)]
    //    [Display(Name = "Password")]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }
    //}
}
