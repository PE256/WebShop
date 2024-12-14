namespace WebShop.Models
{
    public record class Product(
        int Id,
        int Name,
        int Id_Vendor,
        decimal Cost,
        int Quantity);
}
