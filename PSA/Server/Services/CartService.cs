using PSA.Shared;

namespace PSA.Server.Services
{
    public class CartService : ICartService
    {
        private List<Product> _productsInCart = new List<Product>();

        public void AddProductToCart(Product product)
        {
            //var item = _productsInCart.FirstOrDefault(x => x.Id_Preke == product.Id_Preke);
            //if (item != null)
            //{
            //    item.Kiekis++;
            //}
            //else
            //{
            //    product.Kiekis = 1;
            //    _productsInCart.Add(product);
            //}
        }

        public void RemoveProductQuantityByOneFromCart(Product product)
        {
            //var item = _productsInCart.FirstOrDefault(x => x.Id_Preke == product.Id_Preke);
            //if (item?.Kiekis == 1)
            //{
            //    _productsInCart.Remove(item);
            //}
            //else
            //{
            //    if (item != null)
            //    {
            //        item.Kiekis--;
            //    }
            //}
        }

        public void RemoveProductFromCart(Product product)
        {
            //_productsInCart.RemoveAll(x => x.Id_Preke == product.Id_Preke);
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
