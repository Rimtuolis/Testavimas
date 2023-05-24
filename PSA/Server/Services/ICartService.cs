using PSA.Shared;

namespace PSA.Server.Services
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
