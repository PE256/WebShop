using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Web;
using WebShop.Controllers;

namespace WebShop.Models
{
    public class ProfileModel
    {
        public string Name { get; set; }
        public string Coins { get; set; }
        public ProfileModel(string userName, string userCoins)
        {
            Name = userName;
            Coins = userCoins;
        }
    }
}
