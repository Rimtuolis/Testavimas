using ISP_Projektas_2022.Server.Services;
using ISP_Projektas_2022.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ISP_Projektas_2022.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // returns cart contents
        // GET: api/<CartController>
        [HttpGet]
        public List<Product> GetCart()
        {
            return _cartService.GetCart();
        }

        // used to add new product to cart
        // POST api/<CartController>/add
        [HttpPost("add")]
        public void AddCartItem([FromBody] Product value)
        {
            _cartService.AddProductToCart(value);
        }

        // used to remove product from the cart
        // post api/<CartController>/remove
        [HttpPost("remove")]
        public void DeleteCartItemQuantity([FromBody] Product value)
        {
            _cartService.RemoveProductQuantityByOneFromCart(value);
        }

        // used to remove product from the cart
        // post api/<CartController>/remove
        [HttpPost("removeall")]
        public void DeleteCartItem([FromBody] Product value)
        {
            _cartService.RemoveProductFromCart(value);
        }
    }
}
