using PSA.Shared;

namespace PSA.Server.Services
{
    public class CartService : ICartService
    {
        private List<Product> _productsInCart = new List<Product>();

        public void AddProductToCart(Product product)
        {
            var item = _productsInCart.FirstOrDefault(x => x.Id == product.Id);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                product.Quantity = 1;
                _productsInCart.Add(product);
            }
        }

        public void RemoveProductQuantityByOneFromCart(Product product)
        {
            var item = _productsInCart.FirstOrDefault(x => x.Id == product.Id);
            if (item?.Quantity == 1)
            {
                _productsInCart.Remove(item);
            }
            else
            {
                if (item != null)
                {
                    item.Quantity--;
                }
            }
        }

        public void RemoveProductFromCart(Product product)
        {
            _productsInCart.RemoveAll(x => x.Id == product.Id);
        }

        public List<Product> GetCart()
        {
            return _productsInCart;
        }

        public void ClearCart()
        {
            _productsInCart = new List<Product>();
        }
    }
}
