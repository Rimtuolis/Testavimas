using ISP_Projektas_2022.Shared;

namespace ISP_Projektas_2022.Server.Services
{
    public interface ICartService
    {
        void AddProductToCart(Product product);
        void RemoveProductQuantityByOneFromCart(Product product);
        List<Product> GetCart();
        void RemoveProductFromCart(Product product);
        void ClearCart();
    }
}
