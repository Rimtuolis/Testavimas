using PSA.Server.Services;
using PSA.Shared;
using System.Collections.Generic;

namespace PSA.Server.Controllers
{
    
    public class CartServiceStub : ICartService
    {
        private readonly List<Product> cartContents;

        public CartServiceStub(List<Product> cartContents)
        {
            this.cartContents = cartContents;
        }

        public void AddProductToCart(Product product)
        {
            throw new NotImplementedException();
        }

        public void ClearCart()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetCart()
        {
            return cartContents;
        }

        public void RemoveProductFromCart(Product product)
        {
            throw new NotImplementedException();
        }

        public void RemoveProductQuantityByOneFromCart(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
