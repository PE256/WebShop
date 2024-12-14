namespace WebShop.Models
{
    public record class Transaction(
        int Id,
        int Id_User,
        int Id_Vendor,
        int Id_Product,
        decimal SaleAmount,
        int Quantity);
}
