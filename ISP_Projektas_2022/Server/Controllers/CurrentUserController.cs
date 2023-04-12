using Microsoft.AspNetCore.Mvc;
using ISP_Projektas_2022.Server.Services;
using ISP_Projektas_2022.Shared;

namespace ISP_Projektas_2022.Server.Controllers
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