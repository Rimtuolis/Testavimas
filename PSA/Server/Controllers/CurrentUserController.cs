using Microsoft.AspNetCore.Mvc;
using PSA.Server.Services;
using PSA.Shared;

namespace PSA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentUserController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public CurrentUserController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        // GET: api/<CurrentUserController>
        [HttpGet]
        public CurrentUser Get()
        {
            return _currentUserService.GetUser();
        }
    }
}