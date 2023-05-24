using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuilderController : ControllerBase
    {
        private readonly IManualBuildingService _builderService;

        public BuilderController(IManualBuildingService builderService)
        {
            _builderService = builderService;
        }

        // returns cart contents
        // GET: api/<CartController>
        [HttpGet]
        public Robot GetRobot()
        {
            return _builderService.GetBuilder();
        }

        // used to add new product to cart
        // POST api/<CartController>/add
        [HttpPost("add")]
        public void AddBuilderItem([FromBody] Product value)
        {
            _builderService.AddProductToBuilder(value);
        }

        // used to remove product from the cart
        // post api/<CartController>/remove
        [HttpPost("remove")]
        public void DeleteBuilderItemQuantity([FromBody] Product value)
        {
            _builderService.RemoveProductQuantityByOneFromBuilder(value);
        }

        // used to remove product from the cart
        // post api/<CartController>/remove
        [HttpPost("removeall")]
        public void DeleteBuilderItem([FromBody] Product value)
        {
            _builderService.RemoveProductFromBuilder(value);
        }
    }
}
